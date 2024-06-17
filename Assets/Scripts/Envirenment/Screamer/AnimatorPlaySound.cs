using UnityEngine;

public class AnimatorPlaySound : MonoBehaviour
{
    public void PlaySound(string sound) => AudioManager.Instance.PlayOneShot(sound);
}
