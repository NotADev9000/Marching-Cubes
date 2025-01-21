using UnityEngine;

public class CameraController : MonoBehaviour
{
    public bool IsActive = false;
    
    [SerializeField] Camera _mainCamera;
    [Tooltip("Cam distance from pivot is pointsPerChunk * this value")]
    [SerializeField] float _extraDistanceRatioFromPivot = 1.25f;
    [SerializeField] float _maxXAngle = 90f;

    private float _xAngle;
    private float _yAngle;

    private void Awake()
    {
        ResetPivot();
        ResetCamera();
    }

    private void Update()
    {
        if (!IsActive)
        {
            return;
        }
        
        HandleCameraRotation();
    }

    private void ResetPivot()
    {
        float halfPointsPerChunk = GridMetrics.PointsPerChunk / 2f;
        transform.position = new Vector3(halfPointsPerChunk, halfPointsPerChunk, halfPointsPerChunk);

    }

    private void ResetCamera()
    {
        _mainCamera.transform.localPosition = new Vector3(0, 0, -GridMetrics.PointsPerChunk * _extraDistanceRatioFromPivot);
    }

    private void HandleCameraRotation()
    {
        RotateCameraOnX(Input.GetKey(KeyCode.S) ? -1 : Input.GetKey(KeyCode.W) ? 1 : 0);
        RotateCameraOnY(Input.GetKey(KeyCode.D) ? -1 : Input.GetKey(KeyCode.A) ? 1 : 0);
    }

    private void RotateCameraOnX(float angleChange)
    {
        _xAngle += angleChange;
        _xAngle = Mathf.Clamp(_xAngle, -_maxXAngle, _maxXAngle);
        transform.rotation = Quaternion.Euler(_xAngle, _yAngle, 0f);
    }

    private void RotateCameraOnY(float angleChange)
    {
        _yAngle += angleChange;
        transform.rotation = Quaternion.Euler(_xAngle, _yAngle, 0f);
    }
}