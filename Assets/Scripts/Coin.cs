using UnityEngine;

public class Coin : Pickup
{
    private Scoreboard scoreboard;

    public void Init(Scoreboard scoreboard)
    {
        this.scoreboard = scoreboard;
    }

    protected override void OnPickup(Collider player)
    {
        scoreboard.AddScore(100);
    }
}