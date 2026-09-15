using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class ZMovement : MonoBehaviour
{
    // These are the waypoints the zombie has to follow
    public GameObject[] waypoints;
    // The index of the waypoint
    int currentWaypointIndex = 0;

    // This is the player object
    GameObject plr;
    // This is the target the zombie moves towards
    GameObject target;
    // This is the rigidbody of the zombie
    Rigidbody2D rb;

    // This stores how long the zombie remembers the player without seeing them, the zombie only follows the player if they remember them
    float spotTimer = 0f;

    // Whether or not the zombie can always see the player
    public bool alwaysSpot = false;
    
    void Start()
    {
        // Assign values to the player and rigidbody variables
        plr = GameObject.FindWithTag("Player");
        rb = GetComponent<Rigidbody2D>();

        // The starting target is the first waypoint
        target = waypoints[0];
    }

    void Update()
    {
        // Get the direction from the zombie to the target
        Vector2 dir = (target.transform.position - transform.position).normalized;
        // Get the direction from the zombie to the player
        Vector2 dirToPlayer = (plr.transform.position - transform.position).normalized;
        // Cast a ray from the zombie to the player
        RaycastHit2D vision = Physics2D.Raycast(transform.position, dirToPlayer, 10);
        // Draw the ray for debugging
        Debug.DrawRay(transform.position, dirToPlayer * 10f, Color.red);

        // Check if the ray hit something, and if that something is the player
        if (vision && vision.collider.CompareTag("Player")
            // The way dot works is that it checks if 2 vectors are facing the same way.
            // In this case, it is checked if the direction from zombie to the player and the direction of the zombie looking forward are similar.
            && Vector2.Dot(transform.right, dirToPlayer) > 0.2f) 
        {
            // Set how long the zombie will remember the player for to 3 seconds
            spotTimer = 3f;
        }

        // Round out the positions to ensure that the checks are approximate. Otherwise the zombie will get stuck trying to be exactly on the right place.
        float3 targetRound = math.round(target.transform.position * 10);
        float3 transformRound = math.round(transform.position * 10);
        // Since targetRound == transformRound is a bool3 and not a bool, we need to check them all with math.all
        if (math.all(targetRound == transformRound))
            // Go to the next waypoint, or back to the first one using %.
            currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;

        // If the player is spotted or if always spot is enabled
        if (spotTimer > 0f || alwaysSpot)
        {
            // Move to the player
            target = plr;
        } else
        {
            // Move to the desired waypoint
            target = waypoints[currentWaypointIndex];
        }

        // Move the zombie towards the player
        rb.linearVelocity = Vector3.Normalize(dir) * 3.5f;
        // Turn the zombie character to look at the player
        transform.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg);

        // Lower the time that the zombie will remember the player for
        spotTimer -= Time.deltaTime;
    }
}
