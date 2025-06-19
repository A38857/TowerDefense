using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TopBarPanel : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI CoinUI;
    [SerializeField] TextMeshProUGUI HealthPointUI;
    private void OnGUI()
    {
        CoinUI.text = LeverManager.main.TotalCoin.ToString();
        HealthPointUI.text = LeverManager.main.HealthPoint.ToString();
    }
}
