using UnityEngine;
using UnityEngine.Events;

public class EventTrigger : MonoBehaviour
{
    [SerializeField] UnityEvent EventToCall;

    //local
    BoxCollider _collider;

    void Awake()
    {
        _collider = GetComponent<BoxCollider>();
    }

    void OnTriggerEnter(Collider collision)
    {
        EventToCall.Invoke();

        if (collision.gameObject.tag != "Player") Debug.Log("Remove" + collision.gameObject.tag);

        _collider.enabled = false;
    }
}
