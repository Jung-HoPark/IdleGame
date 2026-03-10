using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
    // 테스트용 데이터
    public int asset = 1000;
    public int CPS = 1;
    public int secondIncome = 10;


    public List<UpgradeSO> upgrades;

    Dictionary<string, UpgradeSO> upgradeDict = new Dictionary<string, UpgradeSO>(); // <ID, 업그레이드SO>
    Dictionary<string, int> upgradeLevels = new Dictionary<string, int>(); // <ID, 레벨>

    void Start()
    {
        foreach (UpgradeSO upgrade in upgrades)
        {
            upgradeDict.Add(upgrade.ID, upgrade);
            upgradeLevels.Add(upgrade.ID, 0);
        }
    }
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Q))
        {
            Upgrade("PLAYER_CLICK_01");
        }
        if(Input.GetKeyDown(KeyCode.W))
        {
            Upgrade("PLAYER_SECOND_01");
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log(GetUpgradeLevel("PLAYER_CLICK_01"));
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            Debug.Log(GetUpgradeLevel("PLAYER_SECOND_01"));
        }

    }
    public bool Upgrade(string id)
    {
        UpgradeSO upgrade = upgradeDict[id];
        int level = upgradeLevels[id];

        if (level >= upgrade.maxLevel) return false;


        int cost = upgrade.GetCost(level); // 코스트 만큼 차감
        if (asset < cost)
        {
            Debug.Log("돈 모자람");
            return false;
        }
        asset -= cost;

        level++;
        upgradeLevels[id] = level;

        int value = upgrade.GetValue(level);

        ApplyStat(upgrade.statType, value);
        Debug.Log($"{upgrade.Name} 구매 성공");
        return true;
    }

    void ApplyStat(StatType statType, int value)
    {
        switch (statType)
        {
            case StatType.ClickIncome:
                CPS += value;
                break;
            case StatType.SecondIncome:
                secondIncome += value;
                break;
        }
    }

    public int GetUpgradeLevel(string id)
    {
        if (!upgradeLevels.ContainsKey(id)) return 0;

        return upgradeLevels[id];
    }

}
