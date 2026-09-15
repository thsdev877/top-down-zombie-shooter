using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;

public class Continue : MonoBehaviour
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
        UIManager.instance.TogglePauseMenu();
    }
}
