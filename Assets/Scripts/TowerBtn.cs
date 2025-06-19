using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TowerBtn : MonoBehaviour
{
    [SerializeField]
    private GameObject TowerPrefab;
    [SerializeField]
    private Sprite sprite;
    [SerializeField]
    private Image Image;
    [SerializeField]
    public int Cost = 0;
    [SerializeField]
    private TextMeshProUGUI TextCost;
    [SerializeField]
    public float targetRange;
    private Color originalCorlor;

    private void Start()
    {
        Tower tower = TowerPrefab.GetComponent<Tower>();
        Cost = tower.GetCost();
        TextCost.text = Cost.ToString();
        targetRange = tower.GetTargetRange();
        originalCorlor = Image.color;
    }

    private void Update()
    {
        if(LeverManager.main.TotalCoin < Cost && Image.color == originalCorlor)
        {
            Image.color = new Color(77f / 255f, 77f / 255f, 77f / 255f);
        }
        else if(LeverManager.main.TotalCoin >= Cost && Image.color != originalCorlor)
        {
            Image.color = originalCorlor;
        }
    }

    public GameObject GetPrefab()
    {
        return TowerPrefab;
    }

    public int GetCost()
    {
        return Cost;
    }

    public float GetTargetRange()
    {
        return targetRange;
    }

    public Sprite GetSprite()
    {
        return sprite;
    } 
}
