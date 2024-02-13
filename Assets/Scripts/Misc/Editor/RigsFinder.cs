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

        foreach (Transform transf in allTransforms)
        {
            foreach (var bodyPart in bodyParts)
            {
                if (transf.gameObject.name.Contains(bodyPart.BodyPartName))
                {
                    var rb = transf.gameObject.GetComponent<Rigidbody>();
                    var ac = transf.gameObject.GetComponent<AimConstraint>();

                    bodyPart.BodyPartRb = rb;
                    bodyPart.BodyPartConstraint = ac;

                    Debug.Log(transf.gameObject.name);
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
}

//#endif
