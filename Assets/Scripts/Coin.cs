using UnityEngine;

public class Coin : Pickup
{
    [Header("Floating")]
    [SerializeField] private float floatHeight = 0.15f;
    [SerializeField] private float floatSpeed = 2f;

    [Header("Rotation")]
    [SerializeField] private float rotationSpeed = 120f;

    [Header("Squash & Stretch")]
    [SerializeField] private float squashAmount = 0.05f;
    [SerializeField] private float squashSpeed = 2f;

    private Vector3 startLocalPosition;
    private Vector3 originalScale;

    private void Start()
    {
        // Remember where the coin was placed relative to its Chunk
        startLocalPosition = transform.localPosition;

        // Remember the original size of the coin
        originalScale = transform.localScale;
    }

    private void Update()
    {
        AnimateFloating();
        AnimateRotation();
        AnimateSquashAndStretch();
    }

    private void AnimateFloating()
    {
        float floatOffset =
            Mathf.Sin(Time.time * floatSpeed) * floatHeight;

        transform.localPosition = new Vector3(
            startLocalPosition.x,
            startLocalPosition.y + floatOffset,
            startLocalPosition.z
        );
    }

    private void AnimateRotation()
    {
        transform.Rotate(
            Vector3.up,
            rotationSpeed * Time.deltaTime,
            Space.Self
        );
    }

    private void AnimateSquashAndStretch()
    {
        float scaleOffset =
            Mathf.Sin(Time.time * squashSpeed) * squashAmount;

        transform.localScale = new Vector3(
            originalScale.x - scaleOffset,
            originalScale.y + scaleOffset,
            originalScale.z - scaleOffset
        );
    }
}