using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance;

    public AudioClip menuMusic;
    public AudioClip gameMusic;

    public bool IsMute;

    private AudioSource musicSource;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            musicSource = GetComponent<AudioSource>();
            musicSource.loop = true;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    private void Start()
    {
        IsMute = musicSource.mute;
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "MainMenu")
        {
            Play(menuMusic);
        }
        else if (scene.name == "PlayScene")
        {
            Play(gameMusic);
        }
    }

    private void Play(AudioClip clip)
    {
        if (musicSource.clip == clip || clip == null) return;

        musicSource.clip = clip;
        musicSource.Play();
    }

    public void SetMusic()
    {
        musicSource.mute = !musicSource.mute;
        IsMute = musicSource.mute;
    }

    public bool IsPlay() => musicSource.isPlaying;

    public void PlayMusic() => musicSource.Play();

    public void StopMusic() => musicSource.Stop();
}
