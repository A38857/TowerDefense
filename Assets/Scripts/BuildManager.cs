using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildManager : MonoBehaviour
{
    public static BuildManager main;
    [SerializeField] private TowerInfor[] TowerList;
    private int SelectedTower = 0;

    [SerializeField]
    private Sprite spriteTower;
    private TowerBtn selectedButton;
    private void Awake()
    {
        main = this;
    }
   
    public TowerInfor GetSelectedTower()
    {
        TowerInfor TowerPrefab = new TowerInfor(selectedButton.GetPrefab().GetComponent<Tower>().name, selectedButton.GetCost(), selectedButton.GetPrefab());
        return TowerPrefab;
    }

    public TowerBtn GetSelectedButton()
    {
        return selectedButton;
    }

    public void SetSelectedTower(TowerBtn TowerBtn)
    {
        selectedButton = TowerBtn;
        if (LeverManager.main.TotalCoin >= TowerBtn.Cost)  Hover.Instance.ActiveSprite(selectedButton.GetSprite(), selectedButton.targetRange);
    }

    public void ResetBtn()
    {
        selectedButton = null;
    }
}
