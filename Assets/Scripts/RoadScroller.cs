using UnityEngine;

public class RoadScroller : MonoBehaviour
{
    [SerializeField] public float multiplier = 0.5f;
    [SerializeField]private Renderer myRenderer;
    private float offsetY;

    private void Start()
    {
        //For Set Rode Size On Different Size Of Devices
        FitToScreen();
    }
    void Update()
    {
        if (GameManager.instance.currentState == GameState.Playing)
        {
            float GlobalSpeed = GameManager.instance.globalSpeed;
            offsetY += Time.deltaTime * GlobalSpeed * multiplier;
            myRenderer.material.mainTextureOffset = new Vector2(0, offsetY);
        }
        
    }
    void FitToScreen()
    {
        Camera cam = Camera.main;
        float height = cam.orthographicSize * 2;
        float width = height * cam.aspect;
        transform.localScale = new Vector3(width, height, 1);
    }
}