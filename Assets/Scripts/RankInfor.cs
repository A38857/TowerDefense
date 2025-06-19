using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RankInfor : MonoBehaviour
{
    public TextMeshProUGUI placeText;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI scoreText;

    public void Setup(int place, string name, int score, bool isMe)
    {
        placeText.SetText(place.ToString());
        if(isMe) nameText.SetText("You");
        else nameText.SetText(name);
        scoreText.SetText(score.ToString());

        if (isMe)
        {
            placeText.color = Color.yellow;
            nameText.color = Color.yellow;
            scoreText.color = Color.yellow;
        }
    }
}
