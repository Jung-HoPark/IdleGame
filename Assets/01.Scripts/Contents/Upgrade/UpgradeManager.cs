using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
    public OutSourcing outaa;

    public List<UpgradeDataSO> upgrades;

    public Dictionary<string, UpgradeDataSO> upgradeDict = new Dictionary<string, UpgradeDataSO>();
    public Dictionary<string, int> upgradeLevels = new Dictionary<string, int>();


    public void Init()
    {
        foreach (UpgradeDataSO upgrade in upgrades)
        {
            upgradeDict.Add(upgrade.upgradeID, upgrade);
            upgradeLevels.Add(upgrade.upgradeID, 0);
        }
    }
    public bool BuyUpgrade(string id)
    {
        UpgradeDataSO upgrade = upgradeDict[id];
        int level = upgradeLevels[id];

        if (level >= upgrade.maxLevel) return false;


        BigInteger cost = upgrade.GetCost(level); 
        if (!GameManager.Instance.Asset.DeductAsset(cost))
        {
            return false;
        }

        level++;
        upgradeLevels[id] = level;
        BigInteger previousValue = upgrade.GetReward(level - 1);
        BigInteger currentValue = upgrade.GetReward(level);
        BigInteger valueIncrease = currentValue - previousValue;

        ApplyStat(upgrade.type, valueIncrease);
        return true;
    }

    void ApplyStat(UpgradeType statType, BigInteger value)
    {
        switch (statType)
        {
            case UpgradeType.ClickPower:
                GameManager.Instance.Asset.GoldPerClick += value;
                break;
            case UpgradeType.AutoIncome:
                GameManager.Instance.Asset.CPS += value;
                break;
            case UpgradeType.OutSourcing:
                GameManager.Instance.OutSourcing.IncreaseValue(value);
                break;
            case UpgradeType.SelfDevelopmnet:
                GameManager.Instance.SelfDevelopment.IncreaseValue(value);
                break;
            case UpgradeType.GlobalMultiplier:
                GameManager.Instance.Asset.GlobalIncomePercent += (int)value;
                break;
            case UpgradeType.SelfDevelopmentMulti:
                GameManager.Instance.SelfDevelopment.ValuePercent += (int)value;
                break;
            case UpgradeType.OutSourcingMulti:
                GameManager.Instance.OutSourcing.ValuePercent += (int)value;
                break;
            case UpgradeType.AutoClicker:
                // GameManager.Instance.AutoClickerManager.AutoClickCount += (int)value;
                break;

        }
    }

    public int GetUpgradeLevel(string id)
    {
        if (!upgradeLevels.ContainsKey(id)) return 0;
        return upgradeLevels[id];
    }

}
