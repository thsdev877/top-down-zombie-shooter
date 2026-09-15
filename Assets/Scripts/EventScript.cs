using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EventScript : MonoBehaviour
{
    // This decides whe
    public bool Looped = true;
    // This is used for writing the code
    public List<string> eventLines = new List<string>();
    // These store the actual events themselves
    private List<string> events = new List<string>();

    private bool touchingPlayer = false;
    // This is the index of the current event, so if you write "wait" it still knows where to continue from
    int eventIndex = 0;

    // This is how long the script halts execution if you write "wait <number>"
    float waitTimer = 0f;

    bool RunEvent()
    {
        // This will loop through all the commands, for some info:
        // events[eventIndex] is referring to the command/input we are looking at right now
        // eventIndex++; means get the next input (if you write `wait 5`, it will see wait, then do eventIndex++; which means its now looking at 5)
        while (eventIndex < events.Count)
        {
            switch(events[eventIndex])
            {
                // In the case that you wrote an if
                case "if":
                    eventIndex++;
                    // Check the next input (if <input>)
                    switch (events[eventIndex])
                    {
                        // Check if they're touching the player
                        case "touchingPlayer":
                            if (!touchingPlayer)
                                return false;
                            break;

                        // If you wrote a condition that isnt valid
                        default:
                            Debug.LogError("Unknown condition " + events[eventIndex]);
                            break;
                    }
                    break;

                // Debug printing
                case "print":
                    eventIndex++;
                    Debug.Log(events[eventIndex]);
                    break;
                
                // Wait in seconds
                case "wait":
                    eventIndex++;
                    // Set the time that we are waiting to the inputted seconds
                    waitTimer = float.Parse(events[eventIndex]);
                    eventIndex++;

                    // Return that we are waiting
                    return true;

                // if we want to destroy an object in the world
                case "destroy":
                    eventIndex++;
                    // If you write "self" as the target object, it will not look for self but instead it will remove the gameobject
                    if (events[eventIndex] == "self")
                        Destroy(gameObject);
                    // If not self, search for an object in the world and remove it.
                    else
                        Destroy(GameObject.Find(events[eventIndex]));
                    break;

                // Change the level
                case "loadScene":
                    eventIndex++;
                    SceneManager.LoadScene(events[eventIndex]);
                    break;

                // This lets you enable and disable components on any object
                case "setComponentActive":
                    eventIndex++;
                    // First we get the component
                    string component = events[eventIndex];
                    eventIndex++;
                    // Then we ensure it says "in" next (it looks a bit cleaner)
                    if (events[eventIndex] != "in")
                        Debug.LogError("Expected in, got " + events[eventIndex]);
                    eventIndex++;
                    // Then we want to get the game object we are changing the component on
                    string gameobjectName = events[eventIndex];
                    eventIndex++;
                    // Then we ensure it says "to" next (again, it looks a bit cleaner)
                    if (events[eventIndex] != "to")
                        Debug.LogError("Expected to, got " + events[eventIndex]);
                    eventIndex++;

                    // Get the true/false to see if you want to hide or show it
                    bool targetState = bool.Parse(events[eventIndex]);

                    // Find the GameObject
                    GameObject targetObj = GameObject.Find(gameobjectName);
                    if (targetObj == null)
                    {
                        Debug.LogError("Could not find active GameObject named: " + gameobjectName);
                        break; // Stop here so it doesn't crash
                    }

                    // Find the component
                    Component foundComponent = targetObj.GetComponent(component);

                    // Check its type and enable/disable it
                    if (foundComponent == null)
                    {
                        Debug.LogError("Could not find component: " + component + " on " + gameobjectName);
                    }
                    else if (foundComponent is Behaviour behaviour)
                    {
                        behaviour.enabled = targetState;
                    }
                    else if (foundComponent is Renderer renderer)
                    {
                        renderer.enabled = targetState;
                    }
                    else if (foundComponent is Collider collider)
                    {
                        collider.enabled = targetState;
                    }
                    else
                    {
                        Debug.LogError("Component " + component + " cannot be toggled this way!");
                    }
                    break;

                // This lets you change the camera to a different camera object
                case "setCamera":
                    eventIndex++;
                    // Go through EVERY camera in the game and disable it so we can enable the right one after
                    foreach (GameObject camera in GameObject.FindGameObjectsWithTag("MainCamera"))
                    {
                        camera.GetComponent<Camera>().enabled = false;
                        camera.GetComponent<AudioListener>().enabled = false;
                    }
                    // Find the camera that was inputted and enable that specific one
                    GameObject.Find(events[eventIndex]).GetComponent<Camera>().enabled = true;
                    GameObject.Find(events[eventIndex]).GetComponent<AudioListener>().enabled = true;
                    break;

                // In the case of an error
                default:
                    Debug.LogError("Unknown command " + events[eventIndex]);
                    break;
            }
            
            eventIndex++;
        }
        return false;
    }

    void Start()
    {
        // Hide the event
        gameObject.GetComponent<SpriteRenderer>().enabled = false;

        // Loop through all the lines
        foreach (string line in eventLines)
        {
            // Split by the " character first
            string[] parts = line.Split('"');

            // For every "
            for (int i = 0; i < parts.Length; i++)
            {
                // If it is an odd index we are looking at the text inside quotes
                if (i % 2 == 1)
                {
                    // Add them to the parts to be split later
                    events.Add(parts[i]);
                }
                else 
                // If its an even index we are outside the quotes
                {
                    // Split these parts by space
                    events.AddRange(parts[i].Split(' ', StringSplitOptions.RemoveEmptyEntries));
                }
            }
        }

        // Run the event once at the start
        RunEvent();
        eventIndex = 0;
    }
    void Update()
    {
        // If the script isnt waiting
        if (waitTimer <= 0)
            // Run the code, and if it finished also reset the event entirely
            if (!RunEvent())
                eventIndex = 0;
        waitTimer -= Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // So we can check player collision
        if (collision.CompareTag("Player"))
            touchingPlayer = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
            touchingPlayer = false;
    }
}