using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class Menu : MonoBehaviour
{
    [SerializeField] private MenuElementAnimation[] elementsAnimations;

    private void Start()
    {
        foreach (var animation in elementsAnimations)
        {
            animation.Initialize();

        }
        MenuManager.Instance.RegisterMenu(gameObject.name, this);
        _ = CloseMenuAsync();
    }

    public async Task OpenMenuAsync()
    {
        List<Task> animationTasks = new List<Task>();

        foreach (var animation in elementsAnimations)
        {
            if (animation != null && animation.Element != null)
                animationTasks.Add(PlayAnimationAsync(animation, true));
        }

        await Task.WhenAll(animationTasks);
    }

    public async Task CloseMenuAsync()
    {
        List<Task> animationTasks = new List<Task>();

        foreach (var animation in elementsAnimations)
        {
            if (animation != null && animation.Element != null)
                animationTasks.Add(PlayAnimationAsync(animation, false));

            if (animation.Element.CanvasGroup.alpha == 0)
                animation.Element.gameObject.SetActive(false);
            else
                animation.Element.gameObject.SetActive(true);
        }

        await Task.WhenAll(animationTasks);
    }

    private async Task PlayAnimationAsync(MenuElementAnimation animation, bool isOpening)
    {

        switch (animation.Type)
        {
            case AnimationType.Fade:
                await FadeAnimation(animation, isOpening);
                break;

            case AnimationType.Push:
                await PushAnimation(animation, isOpening);
                break;

            case AnimationType.Bezier:
                await BezierAnimation(animation, isOpening);
                break;
        }
    }

    private async Task FadeAnimation(MenuElementAnimation animation, bool isOpening)
    {
        float startAlpha = isOpening ? animation.StartAlpha : animation.EndAlpha;
        float endAlpha = isOpening ? animation.EndAlpha : animation.StartAlpha;
        float time = 0;

        while (time < animation.Duration)
        {
            if (animation.Element.CanvasGroup.alpha == 0)
                animation.Element.gameObject.SetActive(false);
            else
                animation.Element.gameObject.SetActive(true);

            animation.Element.CanvasGroup.alpha = Mathf.Lerp(startAlpha, endAlpha, time / animation.Duration);
            time += Time.deltaTime;
            await Task.Yield();
        }
        animation.Element.CanvasGroup.alpha = endAlpha;
        if (animation.Element.CanvasGroup.alpha == 0)
            animation.Element.gameObject.SetActive(false);
        else
            animation.Element.gameObject.SetActive(true);
    }

    private async Task PushAnimation(MenuElementAnimation animation, bool isOpening)
    {
        Vector2 startPosition = isOpening ? animation.StartPosition : animation.EndPosition;
        Vector2 endPosition = isOpening ? animation.EndPosition : animation.StartPosition;
        float time = 0;

        while (time < animation.Duration)
        {
            animation.Element.RectTransform.anchoredPosition = Vector2.Lerp(startPosition, endPosition, time / animation.Duration);
            time += Time.deltaTime;
            await Task.Yield();
        }
        animation.Element.RectTransform.anchoredPosition = endPosition;
    }

    private async Task BezierAnimation(MenuElementAnimation animation, bool isOpening)
    {
        Vector2 startPosition = isOpening ? animation.StartPosition : animation.EndPosition;
        Vector2 endPosition = isOpening ? animation.EndPosition : animation.StartPosition;
        float time = 0;

        while (time < animation.Duration)
        {
            float t = animation.CustomCurve.Evaluate(time / animation.Duration);
            animation.Element.RectTransform.anchoredPosition = Vector2.Lerp(startPosition, endPosition, t);
            time += Time.deltaTime;
            await Task.Yield();
        }
        animation.Element.RectTransform.anchoredPosition = endPosition;
    }
}
