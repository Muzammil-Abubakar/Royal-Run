using UnityEngine;
using Unity.Cinemachine;

public class CameraController : MonoBehaviour
{
    [Header("FOV Settings")]
    [SerializeField] private float minFOV = 45f;
    [SerializeField] private float maxFOV = 80f;
    [SerializeField] private float fovLerpSpeed = 5f;

    private CinemachineCamera cinemachineCamera;

    private float targetFOV;

    void Start()
    {
        cinemachineCamera = GetComponent<CinemachineCamera>();

        targetFOV = maxFOV;
        cinemachineCamera.Lens.FieldOfView = targetFOV;
    }

    void Update()
    {
        UpdateFOV();
    }

    private void UpdateFOV()
    {
        cinemachineCamera.Lens.FieldOfView = Mathf.Lerp(
            cinemachineCamera.Lens.FieldOfView,
            targetFOV,
            fovLerpSpeed * Time.deltaTime
        );
    }

    public void SetSpeed(float currentSpeed, float minSpeed, float maxSpeed)
    {
        float speedPercentage = Mathf.InverseLerp(
            minSpeed,
            maxSpeed,
            currentSpeed
        );

        targetFOV = Mathf.Lerp(
            minFOV,
            maxFOV,
            speedPercentage
        );
    }
}