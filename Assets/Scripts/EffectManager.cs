using UnityEngine;
using DG.Tweening;

public class EffectManager : MonoBehaviour
{
    [SerializeField]
    private Sprite coinSprite;
    public static EffectManager Instance;
    public Transform EffectParent;

    void Awake()
    {
        Instance = this;
    }

    public void PlayEffect(GameObject effectPrefab, Vector3 position, float destroyTime = 1.5f)
    {
        GameObject effect = Instantiate(effectPrefab, position, Quaternion.identity, EffectParent);
        Destroy(effect, destroyTime);
    }

    public void PlayDieEffect(GameObject gameObject, float fadeTime = 0.5f)
    {
        SpriteRenderer sprite = gameObject.GetComponent<SpriteRenderer>();
        if (sprite != null)
        {
            DOVirtual.DelayedCall(1f, () =>
            {
                sprite.DOFade(0f, fadeTime).OnComplete(() =>
                {
                    EnermySpawner.OnEnermyDestroy.Invoke();
                    Destroy(gameObject);
                });
            });
        }
        else
        {
            EnermySpawner.OnEnermyDestroy.Invoke();
            Destroy(gameObject, fadeTime);
        }
    }

    public void PlayCoinJumpEffect(Vector3 startPosition, float scale, int count)
    {
        // Effect Coin
        for (int i = 0; i < count; i++)
        {
            GameObject coin = new GameObject("CoinEffect");
            coin.transform.SetParent(EffectParent);
            coin.transform.position = new Vector3(startPosition.x, startPosition.y, 100);
            coin.transform.localScale = new Vector3(scale / 14, scale / 14, 1);
            coin.transform.rotation = Quaternion.identity;

            // SpriteRenderer
            SpriteRenderer sr = coin.AddComponent<SpriteRenderer>();
            sr.material = new Material(Shader.Find("Sprites/Default"));
            sr.sprite = coinSprite;
            sr.sortingLayerName = "Effect";
            sr.transform.rotation = Quaternion.identity;
            sr.sortingOrder = 10;

            float moveUpDistance = 1.0f;
            float moveSideDistance = 0.4f;
            float duration = 0.4f;

            float sideDirection = 0;
            if (count == 1)
            {
                sideDirection = Random.Range(-moveSideDistance, moveSideDistance);
            }
            else if (count == 2 || count == 3)
            {
                if (i == 0) sideDirection = -moveSideDistance;
                else if (i == 1) sideDirection = moveSideDistance;
                else sideDirection = Random.Range(-moveSideDistance, moveSideDistance);
            }
            Vector3 jumpTargetPos = startPosition + new Vector3(sideDirection, moveUpDistance, 0);
            coin.transform.DOMove(jumpTargetPos, duration)
                .SetEase(Ease.OutQuad)
                .OnUpdate(() =>
                {
                    float sine = Mathf.Sin(Time.time * 6) * 0.08f;
                    coin.transform.position = new Vector3(coin.transform.position.x + sine, coin.transform.position.y, coin.transform.position.z);
                })
                .OnComplete(() =>
                {
                    coin.transform.DOMoveY(startPosition.y - 1.5f, 0.4f)
                        .SetEase(Ease.InQuad)
                        .OnComplete(() =>
                        {
                            Destroy(coin);
                        });
                });
        }
    }
}