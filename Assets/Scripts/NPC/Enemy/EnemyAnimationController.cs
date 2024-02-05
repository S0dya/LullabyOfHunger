using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.Animations;

public class EnemyAnimationController : MonoBehaviour
{
    [Header("rigging")]
    [SerializeField] Rig LookingRig;
    [SerializeField] Rig ArmsRig;
    [SerializeField] AimConstraint[] Acshands = new AimConstraint[2];

    [SerializeField] MultiAimConstraint[] MacsLooking = new MultiAimConstraint[2];
    [SerializeField] TwoBoneIKConstraint[] TbikcsArms = new TwoBoneIKConstraint[2];

    //local
    Enemy _enemy;

    //ragdoll
    List<Rigidbody> _rbs;

    void Awake()
    {
        SetupRigs();

        _enemy = GetComponent<Enemy>();

        _rbs = new List<Rigidbody>(GetComponentsInChildren<Rigidbody>());
    }
    void SetupRigs()
    {
        foreach (var macLooking in MacsLooking) macLooking.data.sourceObjects = GetAssignedWeightArr("PlayerHeadTarget");
        foreach (var tbikcArm in TbikcsArms) tbikcArm.data.target = GameObject.FindGameObjectWithTag("PlayerGunTarget").transform;

        RigBuilder rigs = GetComponent<RigBuilder>(); rigs.Build();
    }

    void OnEnable()
    {
        ToggleKinematic(true);
    }
    void OnDisable()
    {
        ToggleKinematic(false);
    }

    //cors

    //outside methods
    public void Push(Vector3 force, Vector3 pos)
    {
        Rigidbody nearest = _rbs.OrderBy(rb => Vector3.Distance(rb.position, pos)).First();

        this.enabled = false;
        nearest.AddForceAtPosition(force * 100, pos, ForceMode.Impulse);

        StartCoroutine(DelayCor(_enemy.Die));
    }

    IEnumerator DelayCor(Action action)//myb move to game manager
    {
        yield return null;

        action.Invoke();
    }

    //other methods
    void ToggleKinematic(bool toggle)
    {
        foreach (var rb in _rbs) rb.isKinematic = toggle;
    }

    WeightedTransformArray GetAssignedWeightArr(string tag)
    {
        return new WeightedTransformArray { new WeightedTransform(GameObject.FindGameObjectWithTag(tag).transform, 1f) };
    }
}
