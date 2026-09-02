using UnityEngine;

public class Coin : Pickup
{
    protected override void OnPickup(Collider player)
    {
        Scoreboard scoreboard = FindAnyObjectByType<Scoreboard>();

        if (scoreboard != null)
        {
            scoreboard.AddScore(100);
        }
    }
}