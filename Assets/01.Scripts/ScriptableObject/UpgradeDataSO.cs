using UnityEngine;
using System.Numerics;

public enum UpgradeType
{
    ClickPower, AutoIncome, GlobalMultiplier,
    OfflineReward, StockEfficiency, SalaryReduction, AutoClicker
}

[CreateAssetMenu(fileName = "NewUpgradeData", menuName = "PangyoDev/UpgradeData")]
public class UpgradeDataSO : ScriptableObject
{
    [Header("기본 정보")]
    public string upgradeID;
    public string upgradeName;
    [TextArea] public string upgradeDesc;

    public Sprite icon;
    public UpgradeType type;

    [Header("초기 설정값")]
    public string baseCostStr = "10";
    public string baseRewardStr = "1";

    [Header("성장 밸런스 계수")]
    public float costMultiplier = 1.15f;
    public float rewardGrowth = 0.5f; // 수익 상승 가속도

    public int maxLevel = 300; // 최대 레벨

    public BigInteger GetCost(int currentLevel)
    {
        if (!BigInteger.TryParse(baseCostStr, out BigInteger baseCost)) return 0;
        double cost = (double)baseCost * Mathf.Pow(costMultiplier, currentLevel);
        return new BigInteger(cost);
    }

    // 밸런스 해결의 핵심! 외부에서 currentLevel을 던져주면 보상을 계산해줌
    public BigInteger GetReward(int currentLevel)
    {
        if (!BigInteger.TryParse(baseRewardStr, out BigInteger baseReward)) return 0; // return 0 = 안전장치 
        if (currentLevel == 0) return 0;

        // '구간 가속형'
        float accelFactor = currentLevel / 10f;
        double addedReward = currentLevel * rewardGrowth * accelFactor;

        //                    형변환                     초반 밸붕방지
        return baseReward + (BigInteger)addedReward + (currentLevel - 1);
    }
}