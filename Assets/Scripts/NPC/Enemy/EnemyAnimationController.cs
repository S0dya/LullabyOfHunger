using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.Animations;

public class EnemyAnimationController : MonoBehaviour
{
    [Header("settings")]
    public float LookingSpeed;

    [Header("rigging")]
    [SerializeField] Rig LookingRig;
    [SerializeField] Rig ArmsRig;
    //[SerializeField] AimConstraint[] AcsHands = new AimConstraint[2];

    [SerializeField] MultiAimConstraint[] MacsLooking = new MultiAimConstraint[2];
    [SerializeField] TwoBoneIKConstraint[] TbikcsArms = new TwoBoneIKConstraint[2];

    [Header("Editor")]
    public BodyPart[] BodyParts;

    //local
    Enemy _enemy;

    //anim

    //ragdoll
    List<Rigidbody> _rbs;

    //cors
    Coroutine _increaseWeightOfLookingCor;
    Coroutine _decreaseWeightOfLookingCor;

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
    IEnumerator IncreaseWeightOfLookingCor()
    {
        while (LookingRig.weight < 1f)
        {
            LookingRig.weight = Mathf.Lerp(LookingRig.weight, 1.05f, LookingSpeed * Time.deltaTime);

            yield return null;
        }

        LookingRig.weight = 1;
    }
    IEnumerator DecreaseWeightOfLookingCor()
    {
        while (LookingRig.weight > 0)
        {
            LookingRig.weight = Mathf.Lerp(LookingRig.weight, -0.05f, LookingSpeed * Time.deltaTime);

            yield return null;
        }

        LookingRig.weight = 0;
    }

    //outside methods
    public void IncreaseHandsWeight(float distanceWeight)
    {
        //Debug.Log(distanceWeight);
        ArmsRig.weight = distanceWeight;
    }

    public void LookAtPlayer()
    {
        StopCor(_decreaseWeightOfLookingCor);
        StopCor(_increaseWeightOfLookingCor);
        _increaseWeightOfLookingCor = StartCoroutine(IncreaseWeightOfLookingCor());
    }
    public void StopLookAtPlayer()
    {
        StopCor(_increaseWeightOfLookingCor);
        StopCor(_decreaseWeightOfLookingCor);
        _decreaseWeightOfLookingCor = StartCoroutine(DecreaseWeightOfLookingCor());
    }

    public void Shot(Vector3 force, Vector3 pos)
    {
        var nearestBodyPart = BodyParts.OrderBy(bodyPart => Vector3.Distance(bodyPart.BodyPartRb.position, pos)).First();

        nearestBodyPart.ShootsAmount--;

        if (nearestBodyPart.ShootsAmount == 0)
        {
            Push(force, pos);

            StartCoroutine(DelayCor(_enemy.Die));
        }
        else
        {
            VisualiseDamageOnShot(nearestBodyPart.BodyPartEnum);
        }
    }

    void Push(Vector3 force, Vector3 pos)
    {
        var nearestRb = _rbs.OrderBy(rb => Vector3.Distance(rb.position, pos)).First();
        
        this.enabled = false;
        nearestRb.AddForceAtPosition(force * 100, pos, ForceMode.Impulse);
    }

    void VisualiseDamageOnShot(EnumsBodyParts enumBodyPart)
    {

        switch(enumBodyPart)
        {
            case EnumsBodyParts.Spine:

                break;

            default: break;
        }

    }

    //other methods
    IEnumerator DelayCor(Action action)
    {
        yield return null;

        action.Invoke();
    }
    void StopCor(Coroutine cor)
    {
        if (cor != null) StopCoroutine(cor);
    }
    void ToggleKinematic(bool toggle)
    {
        foreach (var rb in _rbs) rb.isKinematic = toggle;
    }

    WeightedTransformArray GetAssignedWeightArr(string tag)
    {
        return new WeightedTransformArray { new WeightedTransform(GameObject.FindGameObjectWithTag(tag).transform, 1f) };
    }
}
