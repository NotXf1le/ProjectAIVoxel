using System;
using UnityEngine;
[Serializable]
public class MenuElementAnimation
{
    [SerializeField]public UIElement Element;
    public AnimationType Type;
    public float Duration = 0.5f;

    //[ConditionalHide("Type", AnimationType.Fade)]
    public float StartAlpha = 0;
    //[ConditionalHide("Type", AnimationType.Fade)]
    public float EndAlpha = 1;

    //[ConditionalHide("Type", AnimationType.Push)]
    public Vector2 StartPosition;
    //[ConditionalHide("Type", AnimationType.Push)]
    public Vector2 EndPosition;

    //[ConditionalHide("Type", AnimationType.Bezier)]
    public AnimationCurve CustomCurve = AnimationCurve.Linear(0, 0, 1, 1);
    public void Initialize()
    {
        if(Type == AnimationType.Fade)
            Element.CanvasGroup.alpha = StartAlpha;
        else
            Element.RectTransform.anchoredPosition = StartPosition;

    }
}

public enum AnimationType
{
    Fade,
    Push,
    Bezier
}
public class MenuElement : MonoBehaviour
{
    [SerializeField] public MenuElementAnimation animation;

    private void Start()
    {
        // Инициализируем анимацию при старте
        animation.Initialize();
    }
}