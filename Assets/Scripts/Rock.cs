
using UnityEngine;
using Unity.Cinemachine;

public class Rock : MonoBehaviour
{
    private Camera mainCamera;
    private CinemachineImpulseSource impulseSource;

    [SerializeField] private float maxDistance = 20f;
    [SerializeField] private float audioDistance = 10f;

    [SerializeField] private float sfxCooldown = 1f;
    [SerializeField] private float vfxCooldown = 1f;

    [SerializeField] private AudioSource audioSource;
    [SerializeField] private ParticleSystem particleEffect;

    private float lastSfxTime = -Mathf.Infinity;
    private float lastVfxTime = -Mathf.Infinity;

    private void Awake()
    {
        mainCamera = Camera.main;
        impulseSource = GetComponent<CinemachineImpulseSource>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        float intensity = CalculateImpulseIntensity();

        GenerateImpulse(intensity);
        PlayAudio();
        PlayImpactParticles(collision);
    }

    private float CalculateImpulseIntensity()
    {
        float distance = Vector3.Distance(
            transform.position,
            mainCamera.transform.position
        );

        // 0 = at maxDistance or further, 1 = right next to camera
        return 1f - Mathf.Clamp01(distance / maxDistance);
    }

    private void GenerateImpulse(float intensity)
    {
        impulseSource.GenerateImpulseWithForce(intensity);
    }

    private void PlayAudio()
    {
        float distance = Vector3.Distance(
            transform.position,
            mainCamera.transform.position
        );

        if (distance > audioDistance)
            return;

        if (Time.time - lastSfxTime < sfxCooldown)
            return;

        audioSource.Play();
        lastSfxTime = Time.time;
    }

    private void PlayImpactParticles(Collision collision)
    {
        if (Time.time - lastVfxTime < vfxCooldown)
            return;

        particleEffect.transform.position = collision.contacts[0].point;
        particleEffect.Play();

        lastVfxTime = Time.time;
    }
}

