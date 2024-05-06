using UnityEngine;
using UnityEngine.Events;

public class AnimatorAdditionalEvent : MonoBehaviour
{
    [SerializeField] UnityEvent EventOnAnimationEnd;

    public void OnAnimationEnd() => EventOnAnimationEnd.Invoke();
}
