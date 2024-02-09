using UnityEngine;
using UnityEngine.Animations;

[System.Serializable]
public class BodyPart 
{
    public string BodyPartName;
    public EnumsBodyParts BodyPartEnum;

    public Rigidbody BodyPartRb;
    public AimConstraint BodyPartConstraint;
    
    public int ShootsAmount = 1;
}
