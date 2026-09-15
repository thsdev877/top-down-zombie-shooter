using UnityEngine;

public class Movement : MonoBehaviour
{
    // The default walkspeed of the player
    public float walkSpeed = 3f;
    // The default runspeed of the player
    public float runSpeed = 6f;

    // Player's rigidbody
    Rigidbody2D rb;

    // This turns the player towards the mouse cursor on the screen
    void MousePosToRot()
    {
        Vector2 direction = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        // This is the angle the player must rotate around to face the cursor
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        // Vector3.forward is z axis
        Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        // Set player rotation to the calculated rotation
        transform.rotation = rotation;
    }

    void Start()
    {
        // Assign the value to the rigidbody variable
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (Input.GetKeyDown("escape"))
        {
            UIManager.instance.TogglePauseMenu();
        }

        if (UIManager.instance.gamePaused) return;

        // This stores the direction the player wants to walk towards
        // Set the default position the player is walking in to 0
        Vector2 dir = Vector2.zero;
        // This stores if the player is running or not
        bool running = false;

        // Turn player to mouse
        MousePosToRot();

        // Movement input
        if (Input.GetKey("w") || Input.GetKey("up"))
        {
            // Move up
            dir.y++;
        }
        if (Input.GetKey("s") || Input.GetKey("down"))
        {
            // Move down
            dir.y--;
        }
        if (Input.GetKey("a") || Input.GetKey("left"))
        {
            // Move left
            dir.x--;
        }
        if (Input.GetKey("d") || Input.GetKey("right"))
        {
            // Move right
            dir.x++;
        }

        if (Input.GetKey("left shift"))
        {
            // Enable running
            running = true;
        }

        // Set the player's velocity to the intended direction, add the walkspeed
        // running ? runSpeed : walkSpeed means, if running is true, it will use runSpeed, if it is false, it uses walkSpeed
        rb.linearVelocity = dir * (running ? runSpeed : walkSpeed);
    }
}
