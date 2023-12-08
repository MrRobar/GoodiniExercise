using UnityEngine;
using UnityEngine.EventSystems;

public class Joystick : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    [SerializeField] private RectTransform _joystickBackground;
    [SerializeField] private RectTransform _joystickHandle;

    private Vector2 _inputDirection;

    public void OnPointerDown(PointerEventData eventData)
    {
        OnDrag(eventData);
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 position;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(_joystickBackground, eventData.position,
                eventData.pressEventCamera, out position))
        {
            _inputDirection = (position - _joystickBackground.rect.center) / _joystickBackground.rect.width;

            _inputDirection = (_inputDirection.magnitude > 1.0f) ? _inputDirection.normalized : _inputDirection;

            _joystickHandle.anchoredPosition = _inputDirection * _joystickBackground.rect.width * 0.5f;
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        _inputDirection = Vector2.zero;
        _joystickHandle.anchoredPosition = Vector2.zero;
    }

    public Vector2 GetInputDirection()
    {
        return _inputDirection;
    }
}