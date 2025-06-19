using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Menu : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI CoinUI;
    [SerializeField] Animator Anim;
    private bool IsOpen = true;

    public void ToggleMenu()
    {
        IsOpen = !IsOpen;
        Anim.SetBool("MenuOpen", IsOpen);
    }

    private void OnGUI()
    {
        CoinUI.text = LeverManager.main.TotalCoin.ToString();
    }
}
