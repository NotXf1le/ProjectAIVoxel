using System.Threading.Tasks;
using UnityEngine;

[RequireComponent(typeof(CanvasGroup), typeof(RectTransform))]
public class UIAnimator : MonoBehaviour, IAnimatedUI
{
    private CanvasGroup canvasGroup;
    private RectTransform rectTransform;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        rectTransform = GetComponent<RectTransform>();
    }

    public async Task ShowAsync()
    {
        await Fade(0, 1, 0.5f);  // Стандартный Fade-in
    }

    public async Task HideAsync()
    {
        await Fade(1, 0, 0.5f);  // Стандартный Fade-out
    }

    public void SetText(string text)
    {
        var textComponent = GetComponent<TMPro.TextMeshProUGUI>();
        if (textComponent != null)
            textComponent.text = text;
    }

    public void UpdatePosition() => rectTransform.anchoredPosition = Vector2.zero;

    public void Reset()
    {
        canvasGroup.alpha = 1;
        rectTransform.anchoredPosition = Vector2.zero;
    }

    public void SetFade(float fromAlpha, float toAlpha, float duration)
    {
        StartCoroutine(FadeRoutine(fromAlpha, toAlpha, duration));
    }

    public void SetPush(Vector2 start, Vector2 end, float duration)
    {
        StartCoroutine(PushRoutine(start, end, duration));
    }

    public void SetBezierAnimation(Vector2 start, Vector2 end, AnimationCurve curve, float duration)
    {
        StartCoroutine(BezierRoutine(start, end, curve, duration));
    }

    public void PlayAnimation()
    {
        // Запуск последней установленной анимации
    }

    private async Task Fade(float fromAlpha, float toAlpha, float duration)
    {
        float time = 0;
        while (time < duration)
        {
            canvasGroup.alpha = Mathf.Lerp(fromAlpha, toAlpha, time / duration);
            time += Time.deltaTime;
            await Task.Yield();
        }
        canvasGroup.alpha = toAlpha;
    }

    private System.Collections.IEnumerator FadeRoutine(float fromAlpha, float toAlpha, float duration)
    {
        float time = 0;
        while (time < duration)
        {
            canvasGroup.alpha = Mathf.Lerp(fromAlpha, toAlpha, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        canvasGroup.alpha = toAlpha;
    }

    private System.Collections.IEnumerator PushRoutine(Vector2 start, Vector2 end, float duration)
    {
        float time = 0;
        while (time < duration)
        {
            rectTransform.anchoredPosition = Vector2.Lerp(start, end, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        rectTransform.anchoredPosition = end;
    }

    private System.Collections.IEnumerator BezierRoutine(Vector2 start, Vector2 end, AnimationCurve curve, float duration)
    {
        float time = 0;
        while (time < duration)
        {
            float t = curve.Evaluate(time / duration);
            rectTransform.anchoredPosition = Vector2.Lerp(start, end, t);
            time += Time.deltaTime;
            yield return null;
        }
        rectTransform.anchoredPosition = end;
    }
}
