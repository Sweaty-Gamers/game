using System;

public class PBRScript : Enemy
{
    public static float newHealth = 50f;
    public static float healthCap = 300f;
    void Start()
    {
        health = 400f;
        maxHealth = MathF.Min(newHealth, healthCap);
        agent.speed = movementSpeed;

        pathFindScript.stoppingDistance = 6;
        pathFindScript.shootingDistance = 30;
    }

}
