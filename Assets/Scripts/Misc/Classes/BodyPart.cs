using UnityEngine;

[System.Serializable]
public class BodyPart 
{
    public string BodyPartName;
    public Rigidbody BodyPartRb;
    public EnumsBodyParts BodyPartEnum;
    public int ShootsAmount = 1;
}
