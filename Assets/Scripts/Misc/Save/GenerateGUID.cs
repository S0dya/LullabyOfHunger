using UnityEngine;

[ExecuteAlways]
public class GenerateGUID : MonoBehaviour
{
    [SerializeField] public string GUID = "";

    void Awake()
    {
        if (!Application.IsPlaying(gameObject)) if (GUID == "") PushGenerateGUID();
    }

    public void PushGenerateGUID()
    {
        GUID = System.Guid.NewGuid().ToString();
    }
}
