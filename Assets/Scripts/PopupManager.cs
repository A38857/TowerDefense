using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopupManager : MonoBehaviour
{
    public static PopupManager Instance;

    public GameObject pausePanel;
    public GameObject winLosePanel;
    public GameObject imageWin;
    public GameObject imageLose;

    public GameObject textWin_1;
    public GameObject textWin_2;
    public GameObject textLose_1;
    public GameObject textLose_2;

    public GameObject buttonHome;
    public GameObject buttonRestart;
    public GameObject buttonNext;

    public Image ImageStar1;
    public Image ImageStar2;
    public Image ImageStar3;
    public GameObject textScore;

    public Button ButtonSound;
    public Button ButtonMusic;
    public Sprite SpriteSoundOn;
    public Sprite SpriteSoundOff;
    public Sprite SpriteMusicOn;
    public Sprite SpriteMusicOff;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        ButtonSound.image.sprite = (SoundManager.Instance.IsMute ? SpriteSoundOff : SpriteSoundOn);
        ButtonMusic.image.sprite = (MusicManager.Instance.IsMute ? SpriteMusicOff : SpriteMusicOn);
    }

    public void ShowWin(int Star)
    {
        winLosePanel.SetActive(true);

        imageWin.SetActive(true);
        textWin_1.SetActive(true);
        textWin_2.SetActive(true);

        imageLose.SetActive(false);
        textLose_1.SetActive(false);
        textLose_2.SetActive(false);

        buttonNext.SetActive(true);
        buttonRestart.SetActive(false);
        buttonHome.SetActive(true);

        ImageStar1.gameObject.SetActive(true);
        ImageStar2.gameObject.SetActive(true);
        ImageStar3.gameObject.SetActive(true);
        textScore.SetActive(true);
        if (Star == 1)
        {
            ImageStar1.sprite = Resources.Load<Sprite>("Sprites/Gui/star");
            ImageStar2.sprite = Resources.Load<Sprite>("Sprites/Gui/unstar");
            ImageStar3.sprite = Resources.Load<Sprite>("Sprites/Gui/unstar");
        }
        else if (Star == 2)
        {
            ImageStar1.sprite = Resources.Load<Sprite>("Sprites/Gui/star");
            ImageStar2.sprite = Resources.Load<Sprite>("Sprites/Gui/star");
            ImageStar3.sprite = Resources.Load<Sprite>("Sprites/Gui/unstar");
        }
        else if (Star == 3)
        {
            ImageStar1.sprite = Resources.Load<Sprite>("Sprites/Gui/star");
            ImageStar2.sprite = Resources.Load<Sprite>("Sprites/Gui/star");
            ImageStar3.sprite = Resources.Load<Sprite>("Sprites/Gui/star");
        }
    }

    public void ClickSoundButton()
    {
        SoundManager.Instance.SetSound();
        ButtonSound.image.sprite = (SoundManager.Instance.IsMute ? SpriteSoundOff : SpriteSoundOn);
    }

    public void ClickMusicButton()
    {
        MusicManager.Instance.SetMusic();
        ButtonMusic.image.sprite = (MusicManager.Instance.IsMute ? SpriteMusicOff : SpriteMusicOn);
    }

    public void ShowLose()
    {
        winLosePanel.SetActive(true);

        imageWin.SetActive(false);
        textWin_1.SetActive(false);
        textWin_2.SetActive(false);

        imageLose.SetActive(true);
        textLose_1.SetActive(true);
        textLose_2.SetActive(true);

        buttonNext.SetActive(false);
        buttonRestart.SetActive(true);
        buttonHome.SetActive(true);
    }

    public void HideWinLosePanel()
    {
        winLosePanel.SetActive(false);
    }

    public void ShowPausePanel()
    {
        pausePanel.SetActive(true);
    }

    public void HidePausePanel()
    {
        pausePanel.SetActive(false);
    }
}
