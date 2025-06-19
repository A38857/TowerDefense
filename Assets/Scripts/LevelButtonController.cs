using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Runtime.CompilerServices;

public class LevelButtonController : MonoBehaviour
{
    [SerializeField] private Image backgroundImage;          
    [SerializeField] private Sprite unlockedBG;             
    [SerializeField] private Sprite lockedBG;               

    [SerializeField] private TextMeshProUGUI levelText;     
    [SerializeField] private Image starImage;               
    [SerializeField] private Sprite[] starSprites;           

    [SerializeField] private Button button;
    [SerializeField] private GameObject levelSelect;

    private int level;
    private bool isUnlocked;

    public void Setup(int levelIndex, int levelReached, int stars)
    {
        levelText.SetText((levelIndex + 1).ToString());
        level = levelIndex + 1;
        isUnlocked = level <= levelReached;

        // Bg
        backgroundImage.sprite = isUnlocked ? unlockedBG : lockedBG;

        // UI
        levelText.gameObject.SetActive(isUnlocked);
        starImage.gameObject.SetActive(isUnlocked);
        button.interactable = isUnlocked;

        // Level Check
        if (isUnlocked)
        {
            levelText.SetText((levelIndex + 1).ToString());
            starImage.sprite = starSprites[Mathf.Clamp(stars, 0, 3)];
        }
    }

    public void OnClickLevel()
    {
        if (isUnlocked)
        {
            GameManager.Instance.SetLevel(level);
            gameObject.transform.parent.transform.parent.gameObject.SetActive(false);
            gameObject.transform.parent.GetComponent<LevelSelectManager>().Page = 0;
            MenuUIManager.Instance.SetLevelButton();
        }
    }
}