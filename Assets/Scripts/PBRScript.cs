public class PBRScript : Enemy
{
    void Start()
    {
        health = 400f;
        maxHealth = 400f;
        agent.speed = movementSpeed;

        pathFindScript.stoppingDistance = 6;
        pathFindScript.shootingDistance = 30;
    }

}
