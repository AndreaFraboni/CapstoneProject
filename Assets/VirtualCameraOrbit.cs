using Cinemachine;
using System.Linq;
using UnityEngine;

public class VirtualCameraOrbit : MonoBehaviour
{
    [SerializeField] private Transform _target;

    [SerializeField] private CinemachineVirtualCamera _virtualCamera;

    [SerializeField] private float _mouseSensitivity = 5f;
    [SerializeField] private float _bottomClamp = -20f;
    [SerializeField] private float _topClamp = 60f;
    [SerializeField] private float _startYaw = 0f;
    [SerializeField] private float _startPitch = 15f;
    [SerializeField] private float _zoomSpeed = 2f;
    [SerializeField] private float _minZoomDistance = 2f;
    [SerializeField] private float _maxZoomDistance = 10f;
    [SerializeField] private float _startDistance = 5f;
    [SerializeField] private Vector3 _pivotOffset = new Vector3(0f, 2f, 0f);

    private float _yaw;
    private float _pitch;

    private float _currentDistance;

    private void Start()
    {
        if (_virtualCamera == null) _virtualCamera = GetComponent<CinemachineVirtualCamera>();

        _yaw = _startYaw;
        _pitch = Mathf.Clamp(_startPitch, _bottomClamp, _topClamp);
        _currentDistance = Mathf.Clamp(_startDistance, _minZoomDistance, _maxZoomDistance);

        UpdateCameraPosition();
    }

    private void UpdateCameraPosition()
    {
        Vector3 pivot = _target.position + _pivotOffset;
        Quaternion rotation = Quaternion.Euler(_pitch, _yaw, 0f);
        Vector3 cameraDirection = rotation * new Vector3(0f, 0f, -_currentDistance);
        Vector3 desiredPosition = pivot + cameraDirection;
        transform.position = desiredPosition;
        transform.rotation = Quaternion.LookRotation(pivot - desiredPosition);
    }

    private void LateUpdate()
    {
        if (_target == null) return;

        if (Input.GetMouseButton(1))
        {
            _yaw += Input.GetAxis("Mouse X") * _mouseSensitivity;
            _pitch -= Input.GetAxis("Mouse Y") * _mouseSensitivity;
            _pitch = Mathf.Clamp(_pitch, _bottomClamp, _topClamp);
        }

        float scroll = Input.mouseScrollDelta.y;
        if (Mathf.Abs(scroll) > 0.01f)
        {
            _currentDistance -= scroll * _zoomSpeed;
            _currentDistance = Mathf.Clamp(_currentDistance, _minZoomDistance, _maxZoomDistance);
        }

        UpdateCameraPosition();
    }
}
