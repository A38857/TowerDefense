using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuUIManager : MonoBehaviour
{
    public static MenuUIManager Instance;
    [SerializeField] private GameObject levelSelect;
    [SerializeField] private Transform levelSelectPanel;
    [SerializeField] private GameObject levelButtonPrefab;
    [SerializeField] private GameObject RankCanvas;
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private Image starImage;
    [SerializeField] private Button ButtonSound;
    [SerializeField] private Sprite[] starSprites;
    private Color originColorBtnSound;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        SetLevelButton(GameManager.Instance.GetLevelReached());
        originColorBtnSound = ButtonSound.image.color;
        ButtonSound.onClick.RemoveAllListeners();
        ButtonSound.onClick.AddListener(SoundManager.Instance.SetSound);
        ButtonSound.onClick.AddListener(MusicManager.Instance.SetMusic);
        ButtonSound.onClick.AddListener(SetColorBtnSound);
    }

    public void OnLevelButtonClick()
    {
        levelSelect.SetActive(true);
    }

    public void SetColorBtnSound()
    {
        if (SoundManager.Instance.IsMute) ButtonSound.image.color = new Color(128f/255, 128f/255, 128f / 255);
        else ButtonSound.image.color = originColorBtnSound;
    }

    public void OnHomeButtonClick()
    {
        levelSelect.SetActive(false);
        levelSelectPanel.GetComponent<LevelSelectManager>().Page = 0;
    }

    public void SetLevelButton(int level = 0)
    {
        if(level != 0)
        {
            levelText.SetText(level.ToString());
            int star = GameManager.Instance.GetStarList()[level-1];
            GameManager.Instance.SetLevel(level);
            starImage.sprite = starSprites[Mathf.Clamp(star, 0, 3)];
        }
        else
        {
            levelText.SetText(GameManager.Instance.GetLevel().ToString());
            int star = GameManager.Instance.GetStarList()[GameManager.Instance.GetLevel() - 1];
            starImage.sprite = starSprites[Mathf.Clamp(star, 0, 3)];
        }
    }

    public void OnPlayButton()
    {
        SceneManager.LoadScene("PlayScene");
    }

    public void OnRankButton()
    {
        RankCanvas.gameObject.SetActive(true);
#if UNITY_WEBGL && !UNITY_EDITOR
        Application.ExternalCall("ShowLeaderboard", GameManager.Instance.GetUserId());
#endif
    }
    public void OnRankCloseButton()
    {
        RankCanvas.gameObject.SetActive(false);
    }
}
