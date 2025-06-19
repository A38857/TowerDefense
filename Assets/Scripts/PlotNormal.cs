using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlotNormal : MonoBehaviour
{
    [SerializeField] private Color HoverColor;
    private GameObject Tower;
    private Color OriginColor;

    private void Start()
    {
    }

    private void OnMouseEnter()
    {

    }

    private void OnMouseExit()
    {
    }

    private void OnMouseDown()
    {
        if (Tower != null) return;
        if (!EventSystem.current.IsPointerOverGameObject() && BuildManager.main.GetSelectedButton() != null)
        {
            Hover.Instance.DeactiveSprite();
            BuildManager.main.ResetBtn();
        }
    }
}
