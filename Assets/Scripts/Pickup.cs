using UnityEngine;

public abstract class Pickup : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            OnPickup(other);
            Destroy(gameObject);
        }
    }

    protected abstract void OnPickup(Collider player);

    protected void RotatePickup(float rotationSpeed)
    {
        transform.Rotate(0f, rotationSpeed * Time.deltaTime, 0f);
    }
}