using UnityEngine;

public class PlayerCollisions : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("obstacle"))
        {
            Debug.Log("Player hit an obstacle");
            Destroy(collision.gameObject);
        }
    }
}
