public class PBRScript : Enemy
{
    public override void DropConsumable()
    {
        throw new System.NotImplementedException();
    }

    void Start()
    {
        health = 400f;
        maxHealth = 400f;
        agent.speed = movementSpeed;

        pathFindScript.stoppingDistance = 6;
        pathFindScript.shootingDistance = 30;
    }

}
