using UnityEngine;

public class Apple : Pickup
{
    private LevelGenerator levelGenerator;

    public void Init(LevelGenerator levelGenerator)
    {
        this.levelGenerator = levelGenerator;
    }

    protected override void OnPickup(Collider player)
    {
        levelGenerator.IncreaseMoveSpeed(1f);
    }
}