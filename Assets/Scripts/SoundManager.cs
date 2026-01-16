using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    [Header("Main Speaker")]
    [SerializeField] private AudioSource soundSource;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Scene change hone par bhi ye delete nahi hoga
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void PlaySound(AudioClip clip)
    {
        if (clip != null) 
        {
            soundSource.PlayOneShot(clip);
        }
    }
}