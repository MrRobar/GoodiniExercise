using System;
using UnityEngine;

public class BuildingsReferencer : MonoBehaviour
{
    [SerializeField] private Transform[] _buildings;
    [SerializeField] private BuidlingUIElement[] _uiElements;
    [SerializeField] private CameraMovement _cameraMovement;

    public event Action<int> OnButtonClicked;

    public Transform[] Buildings => _buildings;

    private void OnEnable()
    {
        foreach (var element in _uiElements)
        {
            element.OnButtonPressed += ButtonPressed;
        }

        _cameraMovement.OnZoomProcess += SetButtonsState;
    }

    private void OnDisable()
    {
        foreach (var element in _uiElements)
        {
            element.OnButtonPressed -= ButtonPressed;
        }

        _cameraMovement.OnZoomProcess -= SetButtonsState;
    }

    private void ButtonPressed(int id)
    {
        OnButtonClicked?.Invoke(id);
    }

    private void SetButtonsState(bool enabled)
    {
        foreach (var element in _uiElements)
        {
            element.SetButtonState(enabled);
        }
    }
}