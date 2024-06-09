using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using FMODUnity;

public class Enemy : Subject
{
    [Header("settings")]
    public bool RisesHands;
    public float ArmsDistance = 2.5f;
    public float ArmsDistanceOffset = -1f;

    public float TimeBeforeFollowingPlayer = 1;

    public MonsterNameEnum MonsterName = MonsterNameEnum.Long;

    [Header("other")]
    [SerializeField] Transform CameraFollowTransf;

    [SerializeField] GameObject EnemyTriggers;

    [SerializeField] Transform KillPlayersTransf;

    [SerializeField] bool PatrolsRandomly;
    [SerializeField] List<Transform> PatrolPointsTransfs;

    [Header("animation & sound")]
    public float MinimumSpeedToPlayFootStepSound = 1;

    //local
    NavMeshAgent _agent;
    Animator _animator;
    StudioEventEmitter _seePhrase;

    EnemyAnimationController _enemyAnimationController;

    //general
    float _maxSpeed;

    bool _canRiseHands;

    int _curPatrolI = 0;
    Transform _lastPatrolPointTransf;

    //nav
    Transform _curDestination;

    //anim
    int _animIDMotionSpeed;

    //cons
    Vector3 Destination
    {
        get { return _agent.destination; }
        set { if (_agent.destination != value) _agent.destination = value; }
    }

    //bools
    bool _seesPlayer;
    bool _isFollowingPlayer;

    //threshold
    float _curDistanceToTarget;

    float _curSideVal;
    Vector3 _curDirection;

    float _clampedDistance;

    protected override void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();
        _seePhrase = GetComponent<StudioEventEmitter>();

        _enemyAnimationController = GetComponentInChildren<EnemyAnimationController>();

        _animIDMotionSpeed = Animator.StringToHash("MotionSpeed");

        StopWalking();

        if (PatrolPointsTransfs.Count > 0)
        {
            if (PatrolsRandomly) _curDestination = _lastPatrolPointTransf = PatrolPointsTransfs[UnityEngine.Random.Range(0, PatrolPointsTransfs.Count)]; 
            else _curDestination = PatrolPointsTransfs[_curPatrolI % 2];
        }
        AddAction(EnumsActions.OnDeath, StopWalking);
    }

    void Start()
    {
        _maxSpeed = _agent.speed;
    }

    void Update()
    {
        Destination = _curDestination.position;

        _curDistanceToTarget = Vector3.Distance(transform.position, Destination);
        _animator.SetFloat(_animIDMotionSpeed, _agent.velocity.magnitude);

        //patrol
        if (PatrolPointsTransfs.Count > 0 && Vector3.Distance(transform.position, Destination) < 0.3f && !_seesPlayer)
        {
            if (PatrolsRandomly)
            {
                _curDestination = PatrolPointsTransfs[UnityEngine.Random.Range(0, PatrolPointsTransfs.Count)];
                PatrolPointsTransfs.Add(_lastPatrolPointTransf);
                _lastPatrolPointTransf = _curDestination;
                PatrolPointsTransfs.Remove(_lastPatrolPointTransf);
            }
            else
            {
                if (_curPatrolI > PatrolPointsTransfs.Count - 1) _curPatrolI = 0;
                _curDestination = PatrolPointsTransfs[_curPatrolI]; _curPatrolI++;
            }
        }

        if (_seesPlayer)
        {
            _curDirection = (Destination - transform.position).normalized;
            _curSideVal = Mathf.Clamp(Vector3.Dot(_curDirection, transform.forward), 0, 1f);

            _clampedDistance = _curSideVal - Mathf.Clamp01(_curDistanceToTarget / ArmsDistance - ArmsDistanceOffset);

            if (RisesHands && _canRiseHands) _enemyAnimationController.IncreaseHandsWeight(_clampedDistance);
            _agent.speed = _maxSpeed - Mathf.Clamp(_clampedDistance, 0, 1);
        }
    }

    //animation&sound
    public void PlayFootStep()
    {
        if (_agent.velocity.magnitude > MinimumSpeedToPlayFootStepSound) AudioManager.Instance.PlayOneShot(MonsterName.ToString() + "FootSteps", transform.position);
    }
    
    //other methods
    IEnumerator StartFollowingPlayer()
    {
        yield return new WaitForSeconds(TimeBeforeFollowingPlayer);

        _curDestination = Player.Instance.transform; _canRiseHands = true;
    }
    IEnumerator DelayCor(Action action)
    {
        yield return null;

        action.Invoke();
    }
    void ToggleOffEnemy()
    {
        _animator.enabled = _agent.enabled = _seePhrase.enabled = this.enabled = false;

        EnemyTriggers.SetActive(false);
    }

    //outside methods
    public void PlayerNoticed()
    {
        if (_seesPlayer) return;

        _enemyAnimationController.LookAtPlayer();
        if (!_isFollowingPlayer)
        {
            _isFollowingPlayer = true;
            StartCoroutine(StartFollowingPlayer());
            AudioManager.Instance.PlayOneShot(MonsterName.ToString() + "PlayerNoticed", Vector3.Lerp(transform.position, Player.Instance.transform.position, 0.5f));
            _seePhrase.Play();
        }

        _seesPlayer = true;

        IsometricCamera.Instance.NewPositionForCameraFollow(CameraFollowTransf, this);
    }
    public void PlayerLost()
    {
        if (!_seesPlayer) return;

        _enemyAnimationController.StopLookAtPlayer();
        _seesPlayer = false;

        IsometricCamera.Instance.RemoveEnemyFollow(CameraFollowTransf);
    }

    public void Scream()
    {
        if (_seePhrase.IsPlaying()) _seePhrase.Stop();
        //Invoke("StartPhrase", 0.3f);

        AudioManager.Instance.PlayOneShot(MonsterName.ToString() + "Scream", transform.position);
    }

    public void Kill()
    {
        AudioManager.Instance.PlayOneShot(MonsterName.ToString() + "KillScream");
        _enemyAnimationController.RestoreBodyParts();

        Player.Instance.Die(MonsterName, transform.position, KillPlayersTransf.position);

        if (_seePhrase.IsPlaying()) _seePhrase.Stop();
        _agent.isStopped = true; _agent.speed = 0; _agent.updateRotation = false;

        transform.rotation = Quaternion.LookRotation(Player.Instance.transform.position - transform.position, Vector3.up);

        _animator.Play("Kill"); 
    }

    public void Die()
    {
        IsometricCamera.Instance.RemoveEnemyFollow(CameraFollowTransf);

        StartCoroutine(DelayCor(ToggleOffEnemy));
    }

    //other methods
    void StopWalking()
    {
        _curDestination = transform;
    }
}