using UnityEngine;

public class Apple : Pickup
{
    private LevelGenerator levelGenerator;

    private void Start()
    {
        levelGenerator = FindAnyObjectByType<LevelGenerator>();
    }

    protected override void OnPickup(Collider player)
    {
        levelGenerator.IncreaseMoveSpeed(1f);
    }
}