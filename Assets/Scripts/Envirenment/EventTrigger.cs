using UnityEngine;
using UnityEngine.Events;

public class EventTrigger : MonoBehaviour
{
    [SerializeField] UnityEvent EventToCall;

    void OnTriggerEnter(Collider collision)
    {
        EventToCall.Invoke();

        if (collision.gameObject.tag != "Player") Debug.Log("Remove" + collision.gameObject.tag);
    }
}
