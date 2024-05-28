using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.Animations;

//#if UNITY_EDITOR

[CustomEditor(typeof(EnemyAnimationController))]
public class RigsFinder : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        serializedObject.Update();

        EnemyAnimationController enemyAnimationController = (EnemyAnimationController)target;

        if (GUILayout.Button("Set rigs"))
        {
            enemyAnimationController.BodyParts = GetRigs(enemyAnimationController.transform, enemyAnimationController.BodyParts);
            
            serializedObject.ApplyModifiedProperties();
        }

        serializedObject.ApplyModifiedProperties();
    }

    BodyPart[] GetRigs(Transform EnemyAnimationControllerTransf, BodyPart[] bodyParts)
    {
        var allTransforms = GetAllTransforms(new List<Transform>(), EnemyAnimationControllerTransf);

        foreach (var bodyPart in bodyParts)
        {
            bool found = false;

            foreach (Transform transf in allTransforms)
            {
                if (transf.gameObject.name == bodyPart.BodyPartName)
                {
                    AssignBodyPart(bodyPart, transf);

                    found = true;
                }
                else if (!found && transf.gameObject.name.Contains(bodyPart.BodyPartName))
                {
                    AssignBodyPart(bodyPart, transf);

                    found = true;
                }
            }
            
        }

        return bodyParts;
    }

    List<Transform> GetAllTransforms(List<Transform> cur, Transform parent)
    {
        foreach (Transform transf in parent)
        {
            cur.Add(transf);

            cur = GetAllTransforms(cur, transf);
        }

        return cur;
    }

    void AssignBodyPart(BodyPart bodyPart, Transform transf)
    {
        bodyPart.BodyPartRb = transf.gameObject.GetComponent<Rigidbody>();
        bodyPart.BodyPartConstraint = transf.gameObject.GetComponent<AimConstraint>();
    }
}

//#endif
