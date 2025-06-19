using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class LevelSelectManager : MonoBehaviour
{
    [SerializeField] private GameObject levelButtonPrefab;
    [SerializeField] private Transform levelPanel;
    [SerializeField] private Button ButtonNext;
    [SerializeField] private Button ButtonPrev;
    public int Page;
    public int CountLevel;
    private Color OriginNextColor;
    private Color OriginPrevColor;

    void Start()
    {
        Page = 0;
        OriginNextColor = ButtonNext.image.color;
        OriginPrevColor = ButtonPrev.image.color;
    }

    public void CreateButton()
    {
        foreach (Transform child in this.gameObject.transform)
        {
            Destroy(child.gameObject);
        }
        CountLevel = GameManager.Instance.GetStarList().Length;
        int[] starsPerLevel = GameManager.Instance.GetStarList();         
        int levelReached = GameManager.Instance.GetLevelReached();   
        int Count = (Page + 1) * 15;
        if (Count > CountLevel) Count = CountLevel;
        for (int i = Page*15; i < Count; i++)
        {
            GameObject btnObj = Instantiate(levelButtonPrefab, levelPanel);
            LevelButtonController btn = btnObj.GetComponent<LevelButtonController>();
            btn.Setup(i, levelReached, starsPerLevel[i]);
        }
    }

    void Update()
    {
        if (Page == 0 && ButtonPrev.image.color == OriginPrevColor)
        {
            ButtonPrev.image.color = new Color(128f / 255, 128f / 255, 128f / 255);
            ButtonPrev.enabled = false;
        }
        else if (Page > 0 && ButtonPrev.image.color != OriginPrevColor)
        {
            ButtonPrev.image.color = OriginPrevColor;
            ButtonPrev.enabled = true;
        }
        if (Page == ((CountLevel + 14) / 15) - 1 && ButtonNext.image.color == OriginNextColor)
        {
            ButtonNext.image.color = new Color(128f / 255, 128f / 255, 128f / 255);
            ButtonNext.enabled = false;
        }
        else if (Page < ((CountLevel + 14) / 15) - 1 && ButtonNext.image.color != OriginNextColor)
        {
            ButtonNext.image.color = OriginNextColor;
            ButtonNext.enabled = true;
        }
    }

    public void OnclickNext()
    {
        Page++;
        if (Page >= (CountLevel + 14) / 15)
        {

            Page--;
            return;
        }
        CreateButton();
    }
    public void OnclickPrev()
    {
        Page--;
        if (Page < 0)
        {

            Page = 0;
            return;
        }
        CreateButton();
    }
}
