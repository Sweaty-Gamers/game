using System;

public class PBRScript : Enemy
{
    public static float newHealth = 15f;
    public static float healthCap = 80f;
    new void Start()
    {
        base.Start();

        maxHealth = healthCap;
        health = MathF.Min(healthCap, newHealth);
        agent.speed = movementSpeed;
        dropRate = .2f;
        pathFindScript.stoppingDistance = 6;
        pathFindScript.shootingDistance = 30;
    }

}
