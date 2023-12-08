using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CameraRotation : MonoBehaviour
{
    [SerializeField] private float _sensitivity = 1f, _joystickFactor = 5f;
    [SerializeField] private Transform _target;
    [SerializeField] private Camera _sceneCamera;
    [SerializeField] private GraphicRaycaster _graphicRaycaster;
    [SerializeField] private EventSystem _eventSystem;
    [SerializeField] private BuildingsReferencer _buildingsReferencer;
    [SerializeField] private Joystick _joystick;
    [SerializeField] private CameraMovement _cameraMovement;
    private PointerEventData _pointerEventData;
    private float _rotationX;
    private float _rotationY;
    private bool _isJoystick, _isSwiping, _zoomEnded;

    private void OnEnable()
    {
        _buildingsReferencer.OnButtonClicked += SetTarget;
        _cameraMovement.OnZoomProcess += SetZoomEndedField;
    }

    private void OnDisable()
    {
        _buildingsReferencer.OnButtonClicked -= SetTarget;
        _cameraMovement.OnZoomProcess -= SetZoomEndedField;
    }

    private void Update()
    {
        SwipeRotation();
        JoyStickRotation();
    }

    private void JoyStickRotation()
    {
        if (!_zoomEnded)
        {
            return;
        }

        if (_target == null)
        {
            return;
        }

        if (_isSwiping)
        {
            return;
        }

        Vector2 movementInput = _joystick.GetInputDirection();
        _isJoystick = movementInput != Vector2.zero;

        _rotationX = movementInput.x * _sensitivity * _joystickFactor * Time.deltaTime;
        _rotationY = movementInput.y * _sensitivity * _joystickFactor * Time.deltaTime;

        _sceneCamera.transform.RotateAround(_target.position, Vector3.up, -_rotationX);
        _sceneCamera.transform.RotateAround(_target.position, transform.right, -_rotationY);
    }

    private void SwipeRotation()
    {
        if (!_zoomEnded)
        {
            return;
        }

        if (_target == null)
        {
            return;
        }

        if (_isJoystick)
        {
            return;
        }

        if (Input.GetMouseButton(0))
        {
            _pointerEventData = new PointerEventData(_eventSystem);
            _pointerEventData.position = Input.mousePosition;

            var results = new List<RaycastResult>(1);
            _graphicRaycaster.Raycast(_pointerEventData, results);

            _isSwiping = results.Count == 1 && results[0].gameObject.CompareTag("SwipePanel");
            if (_isSwiping)
            {
                _rotationX = Input.GetAxis("Mouse X") * _sensitivity;
                _rotationY = Input.GetAxis("Mouse Y") * _sensitivity;

                _sceneCamera.transform.RotateAround(_target.position, Vector3.up, -_rotationX);
                _sceneCamera.transform.RotateAround(_target.position, transform.right, _rotationY);
            }
        }
    }

    private void SetTarget(int id)
    {
        _target = _buildingsReferencer.Buildings[id];
    }

    private void SetZoomEndedField(bool isZooming)
    {
        _zoomEnded = !isZooming;
    }
}