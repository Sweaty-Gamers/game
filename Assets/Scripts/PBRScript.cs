using System;

public class PBRScript : Enemy
{
    public static float newHealth = 15f;
    public static float healthCap = 75f;
    new void Start()
    {
        base.Start();

        health = 400f;
        maxHealth = healthCap;
        health = MathF.Min(healthCap, newHealth);
        agent.speed = movementSpeed;

        pathFindScript.stoppingDistance = 6;
        pathFindScript.shootingDistance = 30;
    }

}
