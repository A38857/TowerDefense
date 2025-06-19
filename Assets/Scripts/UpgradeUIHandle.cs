using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UpgradeUIHandle : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public bool mouseOver = false;
    private float targetRange;
    [SerializeField]
    private GameObject TargetRange;

    void Start()
    {
        TargetRange.SetActive(true);
        TargetRange.SetActive(false);
        targetRange = GetComponentInParent<Tower>().GetTargetRange();
        TargetRange.transform.localScale = new Vector3(targetRange* 2, targetRange* 2, 1f);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        mouseOver = true;
        UIManager.main.SetHoveringState(true);
        TargetRange.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        mouseOver = false;
        UIManager.main.SetHoveringState(false);
        gameObject.SetActive(false);
        TargetRange.SetActive(false);
    }
}
