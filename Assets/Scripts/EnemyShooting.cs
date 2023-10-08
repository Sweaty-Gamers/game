using UnityEngine;

public class EnemyShooting : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform bulletSpawnPoint;
    public float bulletForce = 10f;
    public GameObject bullet;

    public float rotationSpeed = 5f; // Adjust as needed

    // Reference to the EnemyPathfindScript
    private EnemyPathfindScript enemyPathfindScript;

    private void Start()
    {
        // Try to find the EnemyPathfindScript on the same GameObject or parent
        enemyPathfindScript = GetComponentInParent<EnemyPathfindScript>();

        if (enemyPathfindScript == null)
        {
            Debug.LogError("EnemyPathfindScript not found!");
        }
    }

    // This method is called by Animation Event
    public void Shoot()
    {
        Debug.Log("Animation Event: Shoot");

        // Set the forward direction to point along the X-axis
        bulletSpawnPoint.forward = Vector3.right;

        // Perform shooting logic here, e.g., instantiate bullet and apply forces
        GameObject bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
        bullet.SetActive(true);

        // Rotate the bullet towards the player
        RotateBulletTowardsPlayer(bullet);

        // Get the Rigidbody component of the bullet
        Rigidbody bulletRb = bullet.GetComponent<Rigidbody>();

        if (bulletRb != null)
        {
            Vector3 forceDirection;

            // Calculate the direction from the bullet to the player
            forceDirection = (enemyPathfindScript.player.position - bullet.transform.position).normalized;

            Debug.Log("Applying Force Direction: " + forceDirection);

            // Use the calculated direction for the force
            bulletRb.AddForce(forceDirection * bulletForce, ForceMode.Impulse);
        }
        else
        {
            Debug.LogError("Bullet prefab is missing Rigidbody component!");
        }
    }




    private void RotateBulletTowardsPlayer(GameObject bullet)
    {
        if (enemyPathfindScript != null)
        {
            // Access the player position from the EnemyPathfindScript instance
            Vector3 playerPosition = enemyPathfindScript.player.position;

            // Calculate the rotation to look at the player
            Quaternion targetRotation = Quaternion.LookRotation(playerPosition - bullet.transform.position);

            // Set the rotation of the bullet directly towards the player
            bullet.transform.rotation = targetRotation;

            bullet.transform.Rotate(Vector3.right, 90f);
        }
    }

}
