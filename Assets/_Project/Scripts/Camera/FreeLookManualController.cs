using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class FreeLookManualController : MonoBehaviour
{
    [SerializeField] private CinemachineFreeLook _freeLookCamera;

    [SerializeField] private float _xSpeed = 120f;
    [SerializeField] private float _ySpeed = 0.03f;

    [SerializeField] private float _zoomSpeed = 2f;
    [SerializeField] private float _minFov = 30f;
    [SerializeField] private float _maxFov = 60f;
    [SerializeField] private float _zoomSmooth = 8f;

    private float zoomInput;

    private void Awake()
    {
        _freeLookCamera = GetComponent<CinemachineFreeLook>();
    }

    private void Update()
    {
        if (_freeLookCamera == null) return;

        HandleRotation();
        HandleZoom();
    }

    private void HandleRotation()
    {
        if (!Input.GetMouseButton(1)) return;

        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        _freeLookCamera.m_XAxis.Value += mouseX * _xSpeed * Time.deltaTime;
        _freeLookCamera.m_YAxis.Value -= mouseY * _ySpeed;

        _freeLookCamera.m_YAxis.Value = Mathf.Clamp(_freeLookCamera.m_YAxis.Value, 0f, 1f);
    }

    private void HandleZoom()
    {
        float fov = _freeLookCamera.m_Lens.FieldOfView;
        fov = fov - zoomInput * _zoomSpeed;
        fov = Mathf.Clamp(fov, _minFov, _maxFov);
        _freeLookCamera.m_Lens.FieldOfView = Mathf.Lerp(_freeLookCamera.m_Lens.FieldOfView, fov, Time.deltaTime * _zoomSmooth);
    }

    public void OnZoom(InputAction.CallbackContext context)
    {
        zoomInput = context.ReadValue<float>();
    }

}
