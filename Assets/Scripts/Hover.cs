using System.Drawing;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using static UnityEngine.Rendering.HableCurve;
using Color = UnityEngine.Color;

public class Hover : Singleton<Hover>
{
    private SpriteRenderer SpRender;
    private float targetRange;
    [SerializeField]
    private GameObject TargetRange;

    // Start is called before the first frame update
    void Start()
    {
        this.SpRender = GetComponent<SpriteRenderer>();
        SpRender.transform.localScale = new Vector3(LeverManager.main.Scale, LeverManager.main.Scale, 1);
        TargetRange.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        FollowMouse();
    }

    void FollowMouse()
    {
        transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = new Vector3(transform.position.x, transform.position.y,0);
    }    

    public void ActiveSprite(Sprite sprite, float targetRange)
    {
        this.SpRender.enabled = true;
        this.SpRender.sprite = sprite;
        this.targetRange = targetRange;
        //SpRender.sprite. = new Vector3(LeverManager.main.Scale, LeverManager.main.Scale, 1);
        TargetRange.SetActive(true);
        TargetRange.transform.localScale = new Vector3(targetRange*2 , targetRange*2 , 1f);
    }

    public void DeactiveSprite()
    {
        this.SpRender.enabled = false;
        TargetRange.SetActive(false);
    }
}
