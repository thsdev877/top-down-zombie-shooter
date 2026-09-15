using UnityEngine;
using UnityEngine.SceneManagement;

public class PHealth : MonoBehaviour
{
    public static PHealth instance;
    public float Health, MaxHealth;
    // How long before you can get damaged again
    public float damageTime = 0.2f;

    private HealthBar healthBar;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        healthBar = UIManager.instance.healthBar;
    }

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        // If touching a zombie AND the player doesnt have i-frames
        if (collision.gameObject.CompareTag("Zombie") && damageTime < 0)
        {
            // Add health and i-frames of 0.5 seconds
            SetHealth(-20f);
            damageTime = 0.5f;
        }
        
        // If touching a medkit and the player doesn't have max health
        if (collision.gameObject.CompareTag("Medkit") && Health < MaxHealth)
        {
            // Add health and remove the medkit
            SetHealth(20f);
            Destroy(collision.gameObject);
        }
    }

    // Set the player health
    public void SetHealth(float healthChange)
    {
        Health += healthChange;
        Health = Mathf.Clamp(Health, 0, MaxHealth);

        healthBar.SetHealth(Health);

        if (Health <= 0)
        {
            healthBar.SetMaxHealth(MaxHealth);
            SetHealth(MaxHealth);
            // If the player has less or equal to 0 hp, restart the level
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    void Update()
    {
        damageTime -= Time.deltaTime;
    }
}
