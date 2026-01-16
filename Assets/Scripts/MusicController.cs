using UnityEngine;

public class MusicController : MonoBehaviour
{
    [SerializeField] private AudioSource musicSource;
    private bool isPaused = false;
    void Update()
    {
        if (GameManager.instance.currentState == GameState.Playing)
        {
            
            if (isPaused)
            {
                musicSource.UnPause();
                isPaused = false; 
            }
            else if (!musicSource.isPlaying)
            {
                musicSource.Play();
            }
        }
        else
        {
            if (musicSource.isPlaying)
            {
                musicSource.Pause();
                isPaused = true; 
            }
        }
    }
}