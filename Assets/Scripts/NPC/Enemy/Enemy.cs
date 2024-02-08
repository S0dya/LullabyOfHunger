using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : Subject
{
    [Header("Settings")]
    public float ArmsDistance = 2.5f;
    public float ArmsDistanceOffset = -1f;

    public float TimeBeforeFollowingPlayer = 1;



    //local
    NavMeshAgent _agent;
    Animator _animator;

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

        _enemyAnimationController = GetComponentInChildren<EnemyAnimationController>();

        _animIDMotionSpeed = Animator.StringToHash("MotionSpeed");

        _curDestination = transform;
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

        if (_seesPlayer)
        {
            _curDirection = (Destination - transform.position).normalized;
            _curSideVal = Mathf.Clamp(Vector3.Dot(_curDirection, transform.forward), 0, 1f);

            _clampedDistance = _curSideVal - Mathf.Clamp01(_curDistanceToTarget / ArmsDistance - ArmsDistanceOffset);

            _enemyAnimationController.IncreaseHandsWeight(_clampedDistance);
            _agent.speed = _maxSpeed - Mathf.Clamp(_clampedDistance, 0, 1);
        }
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
        _curDestination = LevelManager.Instance.GetPlayerTransform();
    }

    //outside methods
    public void PlayerNoticed()
    {
        if (_seesPlayer) return;

        _enemyAnimationController.LookAtPlayer();
        Invoke("StartFollowingPlayer", TimeBeforeFollowingPlayer);

        ToggleSeeingPlayer(true);
    }
    public void PlayerLost()
    {
        if (!_seesPlayer) return;

        _enemyAnimationController.StopLookAtPlayer();
        ToggleSeeingPlayer(false);
    }

    public void Die()
    {
        _animator.enabled = _agent.enabled = this.enabled = false;
    }
}
