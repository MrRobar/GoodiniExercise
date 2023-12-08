using System;
using UnityEngine;
using Task = System.Threading.Tasks.Task;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] private float _zoomSpeed = 1f;
    [SerializeField] private float _distanceOffset = 1f;
    [SerializeField] private float _distance = 4f;
    [SerializeField] private float _rotationThreshold = 1f;
    [SerializeField] private float offsetY = 2.0f;
    [SerializeField] private float offsetX = 0.5f;
    [SerializeField] private BuildingsReferencer _buildingsReferencer;
    private bool _isZooming = false;

    private Transform _currentTarget;

    public event Action<bool> OnZoomProcess;

    private void OnEnable()
    {
        _buildingsReferencer.OnButtonClicked += ZoomToTargetAsync;
    }

    private void OnDisable()
    {
        _buildingsReferencer.OnButtonClicked -= ZoomToTargetAsync;
    }


    private async void ZoomToTargetAsync(int targetID)
    {
        if (_isZooming)
        {
            return;
        }

        if (targetID < 0 || targetID >= _buildingsReferencer.Buildings.Length)
        {
            return;
        }

        _currentTarget = _buildingsReferencer.Buildings[targetID];

        await ZoomToTargetTask();
    }

    private async Task ZoomToTargetTask()
    {
        if (_isZooming)
        {
            return;
        }

        _isZooming = true;
        OnZoomProcess?.Invoke(_isZooming);

        try
        {
            while (true)
            {
                if (_currentTarget == null)
                {
                    _isZooming = false;
                    return;
                }

                Vector3 targetPosition = _currentTarget.position - _currentTarget.forward * _distanceOffset +
                                         Vector3.up * offsetY + _currentTarget.right * offsetX;
                transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * _zoomSpeed);

                Vector3 directionToTarget = (_currentTarget.position - transform.position).normalized;
                Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 3f);
                if (Vector3.Distance(transform.position, _currentTarget.position) < _distance &&
                    Vector3.Angle(transform.forward, directionToTarget) < _rotationThreshold)
                {
                    break;
                }

                await Task.Yield();
            }
        }
        finally
        {
            _isZooming = false;
            OnZoomProcess?.Invoke(_isZooming);
        }
    }
}