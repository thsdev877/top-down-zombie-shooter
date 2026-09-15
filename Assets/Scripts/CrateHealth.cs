using UnityEngine;

public class CrateHealth : MonoBehaviour
{
    // How much health the crate has
    public int health = 3;

    // Function to damage the crate
    public void DamageCrate(int amount)
    {
        // Remove health from the crate
        health -= amount;
        // If the crate has 0 health, remove the crate
        if (health <= 0) Destroy(gameObject);
    }
}
