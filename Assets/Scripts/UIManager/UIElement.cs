using UnityEngine;
using TMPro;

[RequireComponent(typeof(CanvasGroup), typeof(RectTransform))]
public class UIElement : MonoBehaviour
{
    public CanvasGroup CanvasGroup { get; private set; }
    public RectTransform RectTransform { get; private set; }
    public TextMeshProUGUI TextComponent { get; private set; }

    private void Awake()
    {
        CanvasGroup = GetComponent<CanvasGroup>();
        RectTransform = GetComponent<RectTransform>();
        TextComponent = GetComponentInChildren<TextMeshProUGUI>();
    }

    public void SetText(string text)
    {
        if (TextComponent != null)
            TextComponent.text = text;
    }
}
