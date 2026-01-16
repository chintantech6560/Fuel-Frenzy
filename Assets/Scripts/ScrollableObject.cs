using UnityEngine;

public class ScrollableObject : MonoBehaviour
{
    [Header("Object Movment Speed")]
    [SerializeField] float speedMultiplayer = 1f;

    private float bottomBoundary = -10f;

    void Update()
    {
        if (GameManager.instance.currentState == GameState.Playing)
        {
            float globleSpeed = GameManager.instance.globalSpeed;
            transform.Translate(Vector3.down * globleSpeed * speedMultiplayer * Time.deltaTime);

            //  Return to Pool Logic
            if (transform.position.y < bottomBoundary)
            {
                gameObject.SetActive(false);
            }
        }
        
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (gameObject.CompareTag("Obstacle"))
            {
                Debug.Log("Player hit Obstacle! Take Damage.");
            }
            else if (gameObject.CompareTag("Fuel"))
            {
                Debug.Log("Player collected Fuel! Refill.");
            }
            gameObject.SetActive(false);
        }
    }
}