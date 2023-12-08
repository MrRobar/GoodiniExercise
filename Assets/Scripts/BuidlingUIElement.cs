using System;
using UnityEngine;
using UnityEngine.UI;

public class BuidlingUIElement : MonoBehaviour
{
    [SerializeField] private int _id;
    [SerializeField] private Button _button;

    public event Action<int> OnButtonPressed;

    private void OnEnable()
    {
        _button.onClick.AddListener(ButtonClicked);
    }

    private void OnDisable()
    {
        _button.onClick.RemoveListener(ButtonClicked);
    }

    private void ButtonClicked()
    {
        OnButtonPressed?.Invoke(_id);
    }

    public void SetButtonState(bool enabled)
    {
        _button.enabled = !enabled;
    }
}