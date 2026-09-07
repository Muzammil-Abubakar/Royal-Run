using UnityEngine;
using Unity.Cinemachine;

public class Rock : MonoBehaviour
{
    private Camera mainCamera;
    private CinemachineImpulseSource impulseSource;

    [SerializeField] private float maxDistance = 20f;

    private void Awake()
    {
        mainCamera = Camera.main;
        impulseSource = GetComponent<CinemachineImpulseSource>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        float distance = Vector3.Distance(
            transform.position,
            mainCamera.transform.position
        );

        // 0 = far away, 1 = right next to camera
        float intensity = 1f - Mathf.Clamp01(distance / maxDistance);

        impulseSource.GenerateImpulseWithForce(intensity);
    }
}