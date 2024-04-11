using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using FMODUnity;

public class Enemy : SingletonSubject<Enemy>
{
    [SerializeField] Transform[] testtrs;
    int curI = 0;

    [Header("settings")]
    public float ArmsDistance = 2.5f;
    public float ArmsDistanceOffset = -1f;

    public float TimeBeforeFollowingPlayer = 1;

    [Header("other")]
    [SerializeField] Transform CameraFollowTransf;

    [SerializeField] GameObject EnemyTriggers;

    [Header("animation & sound")]
    public float MinimumSpeedToPlayFootStepSound = 1;

    [Header("sound")]
    [SerializeField] String ScreamEventName;

    //local
    NavMeshAgent _agent;
    Animator _animator;
    StudioEventEmitter _seePhrase;

    EnemyAnimationController _enemyAnimationController;

    //general
    float _maxSpeed;

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
        base.Awake();

        _agent = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();
        _seePhrase = GetComponent<StudioEventEmitter>();

        _enemyAnimationController = GetComponentInChildren<EnemyAnimationController>();

        _animIDMotionSpeed = Animator.StringToHash("MotionSpeed");

        _curDestination = transform;

        //TEST
        if (testtrs[0] != null) _curDestination = testtrs[curI%2];
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

        //test
        if (testtrs[0] != null)
        {
            if (Vector3.Distance(transform.position, Destination) < 0.3f)
            {
                _curDestination = testtrs[curI%2];
                curI++;
            }
        }
        

        if (_seesPlayer)
        {
            _curDirection = (Destination - transform.position).normalized;
            _curSideVal = Mathf.Clamp(Vector3.Dot(_curDirection, transform.forward), 0, 1f);

            _clampedDistance = _curSideVal - Mathf.Clamp01(_curDistanceToTarget / ArmsDistance - ArmsDistanceOffset);

            _enemyAnimationController.IncreaseHandsWeight(_clampedDistance);
            _agent.speed = _maxSpeed - Mathf.Clamp(_clampedDistance, 0, 1);
        }
    }

    //animation&sound
    public void PlayFootStep()
    {
        if (_agent.velocity.magnitude > MinimumSpeedToPlayFootStepSound) AudioManager.Instance.PlayOneShot("EnemyLongFootSteps", transform.position);
    }

    //triggers
    void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
        }
    }
    
    //other methods
    void ToggleSeeingPlayer(bool toggle)
    {
        _seesPlayer = toggle;
    }

    void StartFollowingPlayer()
    {
        _curDestination = Player.Instance.transform;

        IsometricCamera.Instance.NewPositionForCameraFollow(CameraFollowTransf, this);
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
            Invoke("StartFollowingPlayer", TimeBeforeFollowingPlayer);
            AudioManager.Instance.PlayOneShot("PlayerNoticed");
            _seePhrase.Play();
        }

        ToggleSeeingPlayer(true);
    }
    public void PlayerLost()
    {
        if (!_seesPlayer) return;

        _enemyAnimationController.StopLookAtPlayer();
        ToggleSeeingPlayer(false);
    }

    public void Scream()
    {
        /*
        if (_seePhrase.IsPlaying()) _seePhrase.Stop();

        Invoke("StartPhrase", 0.3f);
        */

        AudioManager.Instance.PlayOneShot(ScreamEventName, transform.position);
    }

    public void Die()
    {
        IsometricCamera.Instance.RemoveEnemyFollow(CameraFollowTransf);

        StartCoroutine(DelayCor(ToggleOffEnemy));
    }
}
