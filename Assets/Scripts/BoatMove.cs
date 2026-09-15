using UnityEngine;
using UnityEngine.SceneManagement;

public class BoatMove : MonoBehaviour
{
    public float speed = 5f;
    public Vector2 direction = Vector2.right;

    void Update()
    {
        transform.position += (Vector3)direction.normalized * speed * Time.deltaTime;
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("EndWall"))
        {
            UIManager.instance.ShowMainMenu();
            SceneManager.LoadScene("StartScreen");
        }
    }
}
