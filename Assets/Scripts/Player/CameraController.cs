using UnityEngine;
using Unity.Cinemachine;

public class CameraController : MonoBehaviour
{
    [Header("FOV Settings")]
    [SerializeField] private float minFOV = 45f;
    [SerializeField] private float maxFOV = 100f;
    [SerializeField] private float fovLerpSpeed = 5f;

    [Header("Speed Effect")]
    [SerializeField] private ParticleSystem speedEffect;
    [SerializeField] private float speedEffectFOV = 85f;

    private CinemachineCamera cinemachineCamera;

    private float targetFOV;
    private bool initialized = false;


    private void Awake()
    {
        // Get the Cinemachine camera before LevelGenerator.Start()
        // has a chance to call SetSpeed().
        cinemachineCamera = GetComponent<CinemachineCamera>();

        if (cinemachineCamera == null)
        {
            Debug.LogError(
                "CameraController requires a CinemachineCamera component on the same GameObject."
            );

            return;
        }

        // Safe initial values.
        targetFOV = minFOV;
        cinemachineCamera.Lens.FieldOfView = targetFOV;

        SetSpeedEffect(false);
    }


    private void Start()
    {
        // Nothing needed here.
        // Initialization is handled in Awake().
    }


    private void Update()
    {
        UpdateFOV();
    }


    private void UpdateFOV()
    {
        if (cinemachineCamera == null)
            return;

        cinemachineCamera.Lens.FieldOfView = Mathf.Lerp(
            cinemachineCamera.Lens.FieldOfView,
            targetFOV,
            fovLerpSpeed * Time.deltaTime
        );
    }


    public void SetSpeed(
        float currentSpeed,
        float minSpeed,
        float maxSpeed
    )
    {
        if (cinemachineCamera == null)
            return;

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


        // First speed update:
        // immediately use the correct starting FOV.
        if (!initialized)
        {
            cinemachineCamera.Lens.FieldOfView = targetFOV;
            initialized = true;
        }


        UpdateSpeedEffect();
    }


    private void UpdateSpeedEffect()
    {
        if (targetFOV > speedEffectFOV)
        {
            SetSpeedEffect(true);
        }
        else
        {
            SetSpeedEffect(false);
        }
    }


    private void SetSpeedEffect(bool enabled)
    {
        if (speedEffect == null)
            return;

        var emission = speedEffect.emission;
        emission.enabled = enabled;
    }
}