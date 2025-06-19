using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeSpriteTower : MonoBehaviour
{
    public SpriteRenderer Eyes;
    public SpriteRenderer Body;
    public SpriteRenderer LeftHand;
    public SpriteRenderer RightHand;
    public SpriteRenderer BodyTop;
    public SpriteRenderer BodyBottom;
    public SpriteRenderer Pot;

    public void UpgradeSprite(int Level)
    {
        string name = gameObject.GetComponent<Tower>().GetName();
        if (Eyes != null) Eyes.sprite = Resources.Load<Sprite>("Sprites/Tower/" +name+"/"+name+"_"+Level+"/eyes_1");
        if (Body != null) Body.sprite = Resources.Load<Sprite>("Sprites/Tower/" + name + "/" + name + "_" + Level + "/body"); ;
        if (LeftHand != null) LeftHand.sprite = Resources.Load<Sprite>("Sprites/Tower/" + name + "/" + name + "_" + Level + "/left_hand"); ;
        if (RightHand != null) RightHand.sprite = Resources.Load<Sprite>("Sprites/Tower/" + name + "/" + name + "_" + Level + "/right_hand"); ;
        if (BodyTop != null) BodyTop.sprite = Resources.Load<Sprite>("Sprites/Tower/" + name + "/" + name + "_" + Level + "/body_top"); ;
        if (BodyBottom != null) BodyBottom.sprite = Resources.Load<Sprite>("Sprites/Tower/" + name + "/" + name + "_" + Level + "/body_bottom"); ;
        if (Pot != null) Pot.sprite = Resources.Load<Sprite>("Sprites/Tower/" + "pot_"+Level);
    }
}
