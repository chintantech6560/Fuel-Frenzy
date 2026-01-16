using UnityEngine;
using System.Collections;

public class CarController : MonoBehaviour
{
    [SerializeField] public float padding = 1f;

    private float fixedYPosition;
    private float screenHalfWidth; 

    [SerializeField] private Camera cam;
    [SerializeField] private Rigidbody2D rb;

    [Header("Visual Effects")]
    [SerializeField] private SpriteRenderer carSprite;
    private bool isInvulnerable = false;

    [Header("Audio Clips")]
    [SerializeField] private AudioClip collectSound; 
    [SerializeField] private AudioClip crashSound; 

    void Start()
    {
        float screenHeight = cam.orthographicSize * 2;
        float screenBottom = cam.transform.position.y - cam.orthographicSize;

        fixedYPosition = screenBottom + (screenHeight * 0.2f); 

        screenHalfWidth = cam.orthographicSize * cam.aspect;
    }

    void Update()
    {
        if (Input.GetMouseButton(0) && GameManager.instance.currentState == GameState.Playing)
        {
            MoveCar();
        }
        else
        { 
            rb.MovePosition(new Vector2(rb.position.x, fixedYPosition));
        }
    }

    void MoveCar()
    {
        Vector3 inputPos = Input.mousePosition;
        Vector3 worldPos = cam.ScreenToWorldPoint(inputPos);

        float boundary = screenHalfWidth - padding;

        float targetX = Mathf.Clamp(worldPos.x, -boundary, boundary);

        Vector2 newPos = new Vector2(targetX, fixedYPosition);
        rb.MovePosition(newPos);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (GameManager.instance.currentState != GameState.Playing) return;

        if (other.CompareTag("Fuel"))
        {
            SoundManager.instance.PlaySound(collectSound);
            GameManager.instance.AddFuel();
            other.gameObject.SetActive(false);
        }
        else if (other.CompareTag("Obstacle"))
        {
            if (!isInvulnerable)
            {
                SoundManager.instance.PlaySound(crashSound);
                GameManager.instance.TakeDamage(1);
                StartCoroutine(InvulnerabilityRoutine());
                other.gameObject.SetActive(false);
                
            }
        }
    }

    IEnumerator InvulnerabilityRoutine()
    {
        isInvulnerable = true;
        // Blink Effect
        for (int i = 0; i < 5; i++)
        {
            carSprite.enabled = false;
            yield return new WaitForSeconds(0.1f);
            carSprite.enabled = true;
            yield return new WaitForSeconds(0.1f);
        }
        isInvulnerable = false;
    }
}