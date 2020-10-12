using UnityEngine;
using UnityEngine.UI;

public class StatsUI : MonoBehaviour
{
    #region Singleton
    public static StatsUI instance;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("More than one instance of StatsUI found!");
            return;
        }
        instance = this;
    }
    #endregion

    [SerializeField] GameObject statsUI;
    [SerializeField] StatItem damageStat;
    [SerializeField] StatItem armorStat;
    [SerializeField] StatItem moveSpeedStat;

    StatsManager manager;
    int curDamage, curArmor, curMoveSpeed;

    void Start()
    {
        statsUI.SetActive(false);
    }

    void Update()
    {
        if (Input.GetButtonDown("Stats"))
        {
            statsUI.SetActive(!statsUI.activeSelf);
        }
        if (manager != null)
        {
            CheckManagerChanges();
        }
    }

    public void SetManager(StatsManager statsManager)
    {
        manager = statsManager;
        CheckManagerChanges();
    }

    private void CheckManagerChanges()
    {
        if (curDamage != manager.damage)
        {
            curDamage = manager.damage;
            damageStat.ChangeStat(curDamage);
        }
        if (curArmor != manager.armor)
        {
            curArmor = manager.armor;
            armorStat.ChangeStat(curArmor);
        }
        if (curMoveSpeed != manager.moveSpeed)
        {
            curMoveSpeed = manager.moveSpeed;
            moveSpeedStat.ChangeStat(curMoveSpeed);
        }
    }

}
