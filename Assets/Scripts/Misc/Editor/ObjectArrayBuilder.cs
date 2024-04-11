using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ObjectArray))]
public class ObjectArrayBuilder : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        serializedObject.Update();

        ObjectArray objectArray = (ObjectArray)target;

        if (GUILayout.Button("Create array"))
        {
            while (objectArray.transform.childCount > 0) 
                foreach (Transform transf in objectArray.transform) 
                    DestroyImmediate(transf.gameObject);

            CreateArray(objectArray.Prefabs, objectArray.Direction, objectArray.Amount, objectArray.transform);
        }
        
        serializedObject.ApplyModifiedProperties();
    }

    void CreateArray(GameObject[] prefabs, Vector3 direction, int n, Transform parent)
    {
        int prefabsN = prefabs.Length;

        var objectsInfos = new ObjectArrayPrefabInfo[prefabsN];

        for (int i = 0; i < prefabsN; i++)
        {
            var renderers = prefabs[i].GetComponentsInChildren<Renderer>();
            var bounds = new Bounds();
            foreach (var renderer in renderers) bounds.Encapsulate(renderer.bounds);
            var size = bounds.size;

            var scaledDirecton = new Vector3(direction.x * size.x, direction.y * size.y, direction.z * size.z);

            objectsInfos[i] = new ObjectArrayPrefabInfo(prefabs[i], scaledDirecton);
        }

        for (int i = 0; i < n * prefabsN; i++)
        {
            Instantiate(objectsInfos[i % prefabsN].Prefab, 
                objectsInfos[i % prefabsN].ScaledDirection * i + objectsInfos[i % prefabsN].GetPos(), 
                objectsInfos[i % prefabsN].GetRotation(), parent);
        }
    }
}

//classes 
public class ObjectArrayPrefabInfo
{
    [SerializeField] public GameObject Prefab;

    [SerializeField] public Vector3 ScaledDirection;

    public Vector3 GetPos()
    {
        return Prefab.transform.position;
    }
    public UnityEngine.Quaternion GetRotation()
    {
        return Prefab.transform.rotation;
    }

    public ObjectArrayPrefabInfo(GameObject prefab, Vector3 direction)
    {
        Prefab = prefab; ScaledDirection = direction;
    }
}