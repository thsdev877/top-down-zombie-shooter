using UnityEngine;

public class BulletScript : MonoBehaviour
{
    public GameObject blood;
    public GameObject wood;

    private void SpawnDecals(GameObject decal)
    {
        // Spawn a bunch of random decals
        for (int i = 0; i < Random.Range(3, 8); i++)
        {
            Instantiate(decal, transform.position + new Vector3((Random.value - 0.5f) * 1.4f, (Random.value - 0.5f) * 1.4f, 0), transform.rotation).transform.localScale = new Vector3(Random.Range(1.2f, 1.7f), Random.Range(1.2f, 1.7f), 0);
        }
    }

    // If the bullet hit something
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if it hit a zombie
        if (collision.gameObject.CompareTag("Zombie"))
        {
            // Spawn blood
            SpawnDecals(blood);

            // Get the health of the zombie
            ZHealth zombieHealth = collision.gameObject.GetComponent<ZHealth>();
            // Damage the zombie
            zombieHealth.DamageZombie(2);
        }
        // Check if it hit a crate
        else if (collision.gameObject.CompareTag("Crate"))
        {
            // Spawn wood
            SpawnDecals(wood);

            // Get the health of the crate
            CrateHealth crateHealth = collision.gameObject.GetComponent<CrateHealth>();
            // Damage the crate
            crateHealth.DamageCrate(2);
        }
        // Remove the bullet
        Destroy(gameObject);
    }
}
