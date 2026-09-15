using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;

public class Start_ : MonoBehaviour
{
    // Get the button the script is on
    public Button btnStart;

    private void Start()
    {
        // Find the button the script is on
        Button btn = btnStart.GetComponent<Button>();
        // Assign the StartClick function to the button's onClick event
        btn.onClick.AddListener(StartClick);
    }

    public void StartClick()
    {
        // Show the game HUD
        UIManager.instance.ShowGameHUD();
        // Load the scene
        SceneManager.LoadScene("Level1");
    }
}
