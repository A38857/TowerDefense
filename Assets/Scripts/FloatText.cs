using UnityEngine;
using TMPro; 

public class FloatText : MonoBehaviour
{
    const int TYPE_COIN = 1;
    const int TYPE_HEALTH = 2;
    [SerializeField]
    public TextMeshProUGUI text; 
    public float moveSpeed = 20f;
    public float duration = 1f;
    [SerializeField]

    private float timer;

    public void Show(string msg, Color color)
    {
        text.text = msg;
        text.color = color;
        timer = duration;
        gameObject.SetActive(true);
    }

    void Update()
    {
        // Check
        if (!gameObject.activeSelf) return;

        // Effect
        transform.position += Vector3.up * moveSpeed * Time.deltaTime;
        timer -= Time.deltaTime;
        float alpha = Mathf.Clamp01(timer / duration);
        text.color = new Color(text.color.r, text.color.g, text.color.b, alpha);
        if (timer <= 0)
        {
            gameObject.SetActive(false);
            Destroy(gameObject);
        }
    }
}
