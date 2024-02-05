using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : Subject
{
    [Header("Settings")]
    public float MovementSpeed = 2.5f;

    //local
    NavMeshAgent _agent;
    Animator _animator;

    //general
    float _movementSpeed;

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

    protected override void Awake()
    {
        base.Awake();

        _agent = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();

        _animIDMotionSpeed = Animator.StringToHash("MotionSpeed");

        _movementSpeed = MovementSpeed;

        _curDestination = transform;
    }

    void Start()
    {
        _curDestination = LevelManager.Instance.playerTransf;
    }

    void Update()
    {
        Destination = _curDestination.position;

        if (Vector3.Distance(transform.position, Destination) < MovementSpeed)
        {
            _movementSpeed = Vector3.Distance(transform.position, Destination);

            _animator.SetFloat(_animIDMotionSpeed, _movementSpeed);
        }
    }

    //triggers
    void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            _curDestination = LevelManager.Instance.playerTransf;
        }
    }
    
    //other methods
    void ToggleSeeingPlayer(bool toggle)
    {
        _seesPlayer = toggle;
    }

    //outside methods
    public void Die()
    {
        _animator.enabled = _agent.enabled = this.enabled = false;
    }

    public void PlayerNoticed()
    {
        ToggleSeeingPlayer(true);
    }
    public void PlayerLost()
    {
        ToggleSeeingPlayer(false);
    }
}
