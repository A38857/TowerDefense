using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    [Header("Audio Clips")]
    public AudioClip buttonClickClip;
    public AudioClip soundWin;
    public AudioClip soundLose;
    public AudioClip soundBegin;
    public AudioClip musicMenu;
    public AudioClip musicGame;
    public AudioClip brocolli_shoot;
    public AudioClip cactus_shoot;
    public AudioClip pomegranate_shoot;
    public AudioClip strawberry_attack;
    public AudioClip coin_drop;
    public AudioClip upgrade_sound;
    public AudioClip sell_sound;

    private AudioSource audioSource;

    public bool IsMute;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            audioSource = GetComponent<AudioSource>();
            BindAllButtons();  
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        //Debug.Log("audioSource is null? " + (audioSource == null));
        //Debug.Log("buttonClickClip is null? " + (buttonClickClip == null));
        //Debug.Log("Volume: " + audioSource.volume + " | Mute: " + audioSource.mute);
        //Debug.Log("AudioListener: " + FindObjectOfType<AudioListener>());
        IsMute = audioSource.mute;
    }

    public void PlayButtonClick()
    {
        if (buttonClickClip != null)
        {
            audioSource.PlayOneShot(buttonClickClip);
        }
    }

    public void PlayWin()
    {
        if (soundWin != null)
        {
            audioSource.PlayOneShot(soundWin);
        }
    }

    public void PlayBrocolliShoot()
    {
        if (brocolli_shoot != null)
        {
            audioSource.PlayOneShot(brocolli_shoot);
        }
    }

    public void PlayCactusShoot()
    {
        if (cactus_shoot != null)
        {
            audioSource.PlayOneShot(cactus_shoot);
        }
    }

    public void PlayPomeShoot()
    {
        if (pomegranate_shoot != null)
        {
            audioSource.PlayOneShot(pomegranate_shoot);
        }
    }

    public void PlayStawAttack()
    {
        if (strawberry_attack != null)
        {
            audioSource.PlayOneShot(strawberry_attack);
        }
    }

    public void PlayCoinDrop()
    {
        if (coin_drop != null)
        {
            audioSource.PlayOneShot(coin_drop);
        }
    }

    public void PlayUpgrade()
    {
        if (upgrade_sound != null)
        {
            audioSource.PlayOneShot(upgrade_sound);
        }
    }

    public void PlaySell()
    {
        if (sell_sound != null)
        {
            audioSource.PlayOneShot(sell_sound);
        }
    }

    public void PlayLose()
    {
        if (soundLose != null)
        {
            audioSource.PlayOneShot(soundLose);
        }
    }

    public void PlayBegin()
    {
        if (soundBegin != null)
        {
            audioSource.PlayOneShot(soundBegin);
        }
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
        BindAllButtons(); 
    }

    public void SetSound()
    {
        audioSource.mute = !audioSource.mute;
        IsMute = audioSource.mute;
    }

    private void BindAllButtons()
    {
        Button[] buttons = FindObjectsOfType<Button>(true); 

        foreach (Button button in buttons)
        {
            button.onClick.AddListener(() =>
            {
                PlayButtonClick();
            });
        }
    }
}
