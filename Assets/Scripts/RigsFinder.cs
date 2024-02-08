using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

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
        foreach (Transform transf in EnemyAnimationControllerTransf)
        {
            foreach (var bodyPart in bodyParts)
            {
                if (transf.gameObject.name.Contains(bodyPart.BodyPartName))
                {
                    var rb = transf.gameObject.GetComponent<Rigidbody>();

                    bodyPart.BodyPartRb = rb;

                    Debug.Log(transf.gameObject.name);
                }
            }
        }

        return bodyParts;
    }
}

//#endif
