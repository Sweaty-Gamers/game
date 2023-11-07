using System;

public class PBRScript : Enemy
{
    public static float newHealth = 50f;
    public static float healthCap = 300f;
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
