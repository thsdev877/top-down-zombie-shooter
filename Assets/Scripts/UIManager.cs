using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    // The static means everything can access it from anywhere
    public static UIManager instance;

    // This is the main menu UI (quit and start and stuff)
    public GameObject mainMenu;
    // This is the in game UI (ammo and stuff)
    public GameObject gameHUD;
    // This is the in pause menu UI
    public GameObject pauseMenu;

    // Game HUD //
    // The ammo GUI in the canvas
    public TextMeshProUGUI ammoText;
    // The total ammo GUI in the canvas
    public TextMeshProUGUI totalAmmoText;
    // The Health Bar
    public HealthBar healthBar;

    public bool gamePaused = false;

    // Awake runs before start
    void Awake()
    {
        // Check if there already is a UI
        if (instance == null)
        {
            // Set the UI to this instance
            instance = this;
            // This prevents the UI from being destroyed on scene change
            DontDestroyOnLoad(gameObject);

            pauseMenu.SetActive(false);
            gamePaused = false;
        }
        else
        {
            // Prevent duplicates
            Destroy(gameObject);
        }
    }
    
    // This function will show the main menu
    public void ShowMainMenu()
    {
        mainMenu.SetActive(true);
        gameHUD.SetActive(false);
    }

    // This function will show the game hud
    public void ShowGameHUD()
    {
        mainMenu.SetActive(false);
        gameHUD.SetActive(true);
    }

    // This function will toggle the pause menu
    public void TogglePauseMenu()
    {
        // If the game is paused, set the game speed to 0, else set it to 1
        Time.timeScale = pauseMenu.activeSelf ? 1.0f : 0.0f;
        gamePaused = !gamePaused;
        pauseMenu.SetActive(!pauseMenu.activeSelf);
    }

    // This function sets the bullet count on the gui
    public void SetBulletCount(int bullets, int maxAmmo)
    {
        // Display the new ammo count
        ammoText.text = bullets.ToString() + "/" + maxAmmo.ToString();
    }

    // This function sets teh total bullet count on the gui
    public void SetTotalBulletCount(int bullets)
    {
        // Display the new ammo count
        totalAmmoText.text = bullets.ToString();
    }
}