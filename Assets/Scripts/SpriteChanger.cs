using UnityEngine;

public class SpriteChanger : MonoBehaviour
{
    public Sprite newSprite;

    public void ChangeSprite(Sprite spriteToSet)
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr != null && spriteToSet != null) sr.sprite = spriteToSet;
    }
}
