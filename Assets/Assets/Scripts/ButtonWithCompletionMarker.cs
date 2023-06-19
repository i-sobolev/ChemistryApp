using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ButtonWithCompletionMarker : MonoBehaviour
{
    public event Action<ButtonWithCompletionMarker> Clicked;

    [SerializeField] private TextMeshProUGUI _text;
    [SerializeField] private Image _completedMarker;

    private void Start()
    {
        GetComponent<Button>().onClick.AddListener(OnClick);
    }

    public void Set(string text, bool completed)
    {
        _text.SetText(text);
        _completedMarker.enabled = completed;
    }

    public virtual void OnClick()
    {
        Clicked?.Invoke(this);
    }
}
