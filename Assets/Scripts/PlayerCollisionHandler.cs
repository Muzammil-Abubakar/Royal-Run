using UnityEngine;

public class PlayerCollisionHandler : MonoBehaviour
{
    private const string HIT_PARAMETER = "Hit";

    [SerializeField] private Animator animator;
    [SerializeField] private float hitCooldown = 1.5f;
    [SerializeField] private LayerMask environmentLayer;

    private LevelGenerator levelGenerator;
    private float lastHitTime = -Mathf.Infinity;

    private void Start()
    {
        levelGenerator = FindAnyObjectByType<LevelGenerator>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        TryTriggerHit();
    }

    private void OnTriggerEnter(Collider other)
    {
        if ((environmentLayer.value & (1 << other.gameObject.layer)) == 0)
            return;

        TryTriggerHit();
    }

    private void TryTriggerHit()
    {
        if (Time.time < lastHitTime + hitCooldown)
            return;

        lastHitTime = Time.time;

        animator.SetTrigger(HIT_PARAMETER);

        levelGenerator.DecreaseMoveSpeed(1f);
    }
}