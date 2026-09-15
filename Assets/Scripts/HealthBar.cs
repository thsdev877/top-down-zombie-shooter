using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    // The health
    public float Health, MaxHealth, Width, Height;

    // The health bar
    [SerializeField]
    private RectTransform healthBar;

    // Function to set the max health
    public void SetMaxHealth(float maxhealth)
    {
        MaxHealth = maxhealth;
    }

    // Function to set the health bar
    public void SetHealth(float health)
    {
        Health = health;

        // Update the health bar in the gui
        float newWidth = (Health / MaxHealth) * Width;
        healthBar.sizeDelta = new Vector2(newWidth, Height);
    }
}
