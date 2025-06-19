using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Plot : MonoBehaviour
{
    [SerializeField] private SpriteRenderer SpriteRen;
    [SerializeField] private Color HoverColor;
    [SerializeField] private int Type;
    private GameObject Tower;
    private Color OriginColor;

    private void Start()
    {
        if (Type == 1) OriginColor = SpriteRen.color;
    }

    private void OnMouseEnter()
    {
        if(Type == 1)
        {
            if (EventSystem.current.IsPointerOverGameObject()) return;
            SpriteRen.color = HoverColor;
        }
    }

    public void RemoveTower()
    {
        if (Tower == null) return;
        Tower = null;
        Hover.Instance.DeactiveSprite();
        BuildManager.main.ResetBtn();
        UIManager.main.SetHoveringState(false);
    }

    private void OnMouseExit()
    {
        if(Type == 1) SpriteRen.color = OriginColor;
    }

    private void OnMouseDown()
    {
        if (UIManager.main.IsHoveringUI()) return;
        if(Type == 1)
        {
            if (Tower != null)
            {
                Tower.GetComponent<Tower>().OpenUpgrade();
                if (!EventSystem.current.IsPointerOverGameObject() && BuildManager.main.GetSelectedButton() != null)
                {
                    Hover.Instance.DeactiveSprite();
                    BuildManager.main.ResetBtn();
                }
                return;
            }
            if (!EventSystem.current.IsPointerOverGameObject() && BuildManager.main.GetSelectedButton() != null)
            {
                TowerInfor TowerBuild = BuildManager.main.GetSelectedTower();
                if (TowerBuild.Cost > LeverManager.main.TotalCoin) return;
                LeverManager.main.SpendCoin(TowerBuild.Cost);
                Tower = Instantiate(TowerBuild.Prefab, transform.position, Quaternion.identity);
                Tower.transform.localScale = new Vector3(LeverManager.main.Scale, LeverManager.main.Scale, 1);
                Tower.GetComponent<Tower>().SetPlot(this);
                Hover.Instance.DeactiveSprite();
                BuildManager.main.ResetBtn();
            }
        }
        else
        {
            if (Tower != null) return;
            if (!EventSystem.current.IsPointerOverGameObject() && BuildManager.main.GetSelectedButton() != null)
            {
                Hover.Instance.DeactiveSprite();
                BuildManager.main.ResetBtn();
            }
        }
    }
}
