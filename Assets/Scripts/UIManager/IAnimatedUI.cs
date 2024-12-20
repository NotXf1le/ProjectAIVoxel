using UnityEngine;

public interface IAnimatedUI : IUIElement
{
    void SetFade(float fromAlpha, float toAlpha, float duration); // Настройка анимации Fade
    void SetPush(Vector2 start, Vector2 end, float duration);      // Настройка анимации Push
    void SetBezierAnimation(Vector2 start, Vector2 end, AnimationCurve curve, float duration); // Кривая Безье
    void PlayAnimation(); // Запуск анимации
}
