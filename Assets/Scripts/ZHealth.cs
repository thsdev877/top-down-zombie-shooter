using UnityEngine;

public class ZHealth : MonoBehaviour
{
    // How much health the zombie has
    public int health = 8;

    // Function to damage the zombie
    public void DamageZombie(int amount)
    {
        // Remove health from the zombie
        health -= amount;
        // If the zombie has 0 health, remove the zombie
        if (health <= 0) Destroy(gameObject);
    }
}
