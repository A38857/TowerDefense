 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Unity.Burst.CompilerServices;
using UnityEngine.UI;

public class Tower : MonoBehaviour
{
    const int TYPE_MELEE    = 0;
    const int TYPE_RANGE    = 1;
    const int MAX_LEVEL     = 3;
    [SerializeField] private float TargetRange = 2.5f;
    [SerializeField] private LayerMask EnermyMask;
    [SerializeField] private string NameTower;
    [SerializeField] private int Cost = 100;
    [SerializeField] private int UpgradeCost = 25;
    [SerializeField] private int Level = 1;
    [SerializeField] private int Type = TYPE_MELEE;
    [SerializeField] private GameObject UpgradeUI;
    [SerializeField] private Button ButtonUpgrade;
    [SerializeField] private Button ButtonSell;
    private Transform target;
    private Plot Plot;
    private IAttackBehavior attackBehavior;
    private Color originalCorlorBtnUp;

    private void Start()
    {
        attackBehavior = GetComponent<IAttackBehavior>();
        ButtonUpgrade.onClick.AddListener(Upgrade);
        ButtonSell.onClick.AddListener(Sell);
        originalCorlorBtnUp = ButtonUpgrade.image.color;
    }

    // Update is called once per frame
    void Update()
    {
        if ((UpgradeCost > LeverManager.main.TotalCoin || Level == MAX_LEVEL) &&  ButtonUpgrade.enabled == true)
        {
            ButtonUpgrade.enabled = false;
            ButtonUpgrade.image.color = new Color(77f / 255f, 77f / 255f, 77f / 255f);
        }
        else if (UpgradeCost <= LeverManager.main.TotalCoin && Level < MAX_LEVEL && ButtonUpgrade.enabled == false)
        {
            ButtonUpgrade.enabled = true;
            ButtonUpgrade.image.color = originalCorlorBtnUp;
        }

        if (!target)
        {
            // Target
            FindTarget();

            // Strawberry
            if (!target && gameObject.GetComponent<StrawberryAttack>() != null)
            {
                gameObject.GetComponent<StrawberryAttack>().hasTarget = false;
            }
        }
        else
        {
            attackBehavior?.Attack(target);
            if (!CheckTargetInRange() || target.GetComponent<Enermy>().GetIsDie())
            {
                target = null;
            }
        }
    }

    public int GetLevel()
    {
        return Level;
    }

    private void FindTarget()
    {
        // Find
        RaycastHit2D[] hitList = Physics2D.CircleCastAll(transform.position, TargetRange, Vector2.zero, 0f, EnermyMask);

        float minDistance = Mathf.Infinity;
        Transform nearestTarget = null;
        int TypeEnemy = 0;

        for (int i = 0; i < hitList.Length; i++)
        {
            Enermy enermy = hitList[i].transform.GetComponent<Enermy>();
            if (enermy != null && enermy.GetIsDie() == false)
            {
                float distance = Vector2.Distance(transform.position, hitList[i].transform.position);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    nearestTarget = hitList[i].transform;
                    TypeEnemy = enermy.GetType();
                }
            }
        }

        // Set target
        target = nearestTarget;
        if (Type == TYPE_MELEE && TypeEnemy == 2) target = null;
    }

    private bool CheckTargetInRange()
    {
        return Vector2.Distance(target.position, transform.position) <= TargetRange;
    }

    private void OnDrawGizmos()
    {
#if UNITY_EDITOR
        Handles.color = Color.cyan;
        Handles.DrawWireDisc(transform.position, transform.forward, TargetRange);
#endif
    }

    public float GetTargetRange()
    {
        return TargetRange;
    }

    public string GetName()
    {
        return NameTower;
    }

    public void SetPlot(Plot NPlot)
    {
        Plot = NPlot;
    }

    public void OpenUpgrade()
    {
        UpgradeUI.SetActive(true);
    }

    public void CloseUpgrade()
    {
        UpgradeUI.SetActive(false);
    }

    public void Upgrade()
    {
        if (UpgradeCost > LeverManager.main.TotalCoin) return;
        if (Level < MAX_LEVEL)
        {
            LeverManager.main.SpendCoin(UpgradeCost);
            Level++;
            attackBehavior?.Upgrade(Level);
            this.GetComponent<ChangeSpriteTower>().UpgradeSprite(Level);

            // Sound
            SoundManager.Instance.PlayUpgrade();
        }
    }
    
    public void Sell()
    {
        int refund = Cost / 2;
        LeverManager.main.IncreseCoin(refund);
        if (UpgradeUI.activeSelf) UpgradeUI.SetActive(false);
        Plot.RemoveTower();
        Plot = null;

        //sound
        SoundManager.Instance.PlaySell();

        Destroy(gameObject);
    }    

    public int GetCost()
    {
        return Cost;
    }
}
