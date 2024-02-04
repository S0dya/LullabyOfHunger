using System.Linq;
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

    //ragdoll
    List<Rigidbody> _rbs;

    protected override void Awake()
    {
        base.Awake();

        _agent = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();

        _animIDMotionSpeed = Animator.StringToHash("MotionSpeed");

        _movementSpeed = MovementSpeed;

        _curDestination = transform;

        _rbs = new List<Rigidbody>(GetComponentsInChildren<Rigidbody>());
    }

    protected override void OnEnable()
    {
        base.OnEnable();

        //foreach (var rb in _rbs) rb.isKinematic = true;
    }
    protected override void OnDisable()
    {
        base.OnDisable();

        foreach (var rb in _rbs) rb.isKinematic = false;
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

    //outside methods
    public void Push(Vector3 force, Vector3 pos)
    {
        Die();
        Rigidbody nearest = _rbs.OrderBy(rb => Vector3.Distance(rb.position, pos)).First();

        nearest.AddForceAtPosition(force, pos, ForceMode.Impulse);
    }

    public void Die()
    {
        _animator.enabled = false;
        _agent.enabled = false;

        Destroy(this);
    }
}
