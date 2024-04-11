using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public abstract class UISingletonMonobehaviour<T> : MonoBehaviour where T : MonoBehaviour
{
    [Header("main UI")]
    [SerializeField] public CanvasGroup MainCG;

    static T instance;
    public static T Instance { get { return instance; } }

    protected virtual void Awake()
    {
        if (instance == null) instance = this as T;
        else Debug.Log(gameObject.transform + " duplicated");

        if (MainCG == null) MainCG = GetComponent<CanvasGroup>();
    }

    //inherited, outside methods
    public virtual void FadeSetTime(float target, float duration)
    {
        FadeSetTime(target, duration, null);
    }
    public virtual void FadeSetTime(float target, float duration, System.Action action)
    {
        Fade(target, duration, action);
        
        SetTime(target);
    }

    public virtual void Fade(float target, float duration)
    {
        Fade(target, duration, null);
    }
    public virtual void Fade(float target, float duration, System.Action action)
    {
        MainCG.DOFade(target, duration).SetUpdate(true).OnComplete(() =>
        {
            if (action != null) action.Invoke();

            MainCG.blocksRaycasts = target == 1;
        });
    }
    
    public virtual void SwitchCGSetTime(float target)
    {
        SwitchCG(target);

        SetTime(target);
    }
    public virtual void SwitchCG(float target)
    {
        MainCG.alpha = target; MainCG.blocksRaycasts = target == 1;
    }
    
    //other methods
    void SetTime(float target) => Time.timeScale = 1 - target;
}
