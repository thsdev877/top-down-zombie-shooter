using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;

public class Quit : MonoBehaviour
{
    // The button that lets you quit the game
    public Button btnQuit;

    void Start()
    {
        // Add the quit event to the quit button
        Button btn = btnQuit.GetComponent<Button>();
        btn.onClick.AddListener(QuitClick);
    }

    // Event thats called when you quit the game
    public void QuitClick()
    {
        Debug.Log("Quit clicked!");
        Application.Quit();
    }
}
