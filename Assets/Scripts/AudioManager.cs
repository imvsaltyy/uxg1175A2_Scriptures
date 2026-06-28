using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header ("--------------Audio Sources--------------")]
    [SerializeField] AudioSource musicsource;
    [SerializeField] AudioSource SFXsource;

    [Header("--------------Audio Clip--------------")]
    public AudioClip mainmenubackground;
    public AudioClip gamebackground;
    public AudioClip characterdeath;
    public AudioClip characterdamaged;
    public AudioClip charactershoot;
    public AudioClip alienshoot;
    public AudioClip duckshoot;
    public AudioClip aliendeath;
    public AudioClip duckdeath;
    public AudioClip buttonpress;

    public static AudioManager Instance;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        musicsource.clip = mainmenubackground;
        musicsource.volume = 0.01f; // volume range is 0.0 (silent) to 1.0 (full)
        musicsource.loop = true;
        musicsource.Play();
    }

    public void PlaySFX(AudioClip clip)
    {
        SFXsource.PlayOneShot(clip);
    }

}
