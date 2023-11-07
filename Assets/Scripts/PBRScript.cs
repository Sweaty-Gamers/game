using System;

public class PBRScript : Enemy
{
    public static float newHealth = 15f;
    public static float healthCap = 80f;
    new void Start()
    {
        base.Start();

        health = 400f;
        maxHealth = MathF.Min(healthCap, newHealth);
        health = maxHealth;
        agent.speed = movementSpeed;

        pathFindScript.stoppingDistance = 6;
        pathFindScript.shootingDistance = 30;
    }

}
