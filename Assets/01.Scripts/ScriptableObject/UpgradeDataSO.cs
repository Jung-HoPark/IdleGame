using UnityEngine;
using System.Numerics;

public enum UpgradeType
{
    ClickPower, // 클릭 당
    AutoIncome, // CPS: 초당
    OutSourcing,
    SelfDevelopmnet,

    GlobalMultiplier, // 전체 퍼센트
    SelfDevelopmentMulti, // 자체 개발 최소 최대 배율
    OutSourcingMulti, // 아웃 소싱 치명타 배율
    AutoClicker, // 추가 클릭 횟수

    SalaryReduction
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

    [Header("인트 성장 계수")]
    public string intGrowth = "5";

    public int maxLevel = 300; // 최대 레벨

    public BigInteger GetCost(int currentLevel)
    {
        if (!BigInteger.TryParse(baseCostStr, out BigInteger baseCost)) return 0;
        double cost = (double)baseCost * Mathf.Pow(costMultiplier, currentLevel);
        return new BigInteger(cost);
    }

    public BigInteger GetReward(int currentLevel)
    {
        if(type == UpgradeType.ClickPower || type == UpgradeType.AutoIncome || type == UpgradeType.OutSourcing || type == UpgradeType.SelfDevelopmnet)
        {
            return BigIntegerReward(currentLevel);
        }
        if (type == UpgradeType.GlobalMultiplier || type == UpgradeType.SelfDevelopmentMulti || type == UpgradeType.OutSourcingMulti || type == UpgradeType.AutoClicker)
        {
            return BigIntegerReward(currentLevel);
        }
        return 0;
    }

    public BigInteger BigIntegerReward(int currentLevel)
    {
        if (!BigInteger.TryParse(baseRewardStr, out BigInteger baseReward)) return 0; // return 0 = 안전장치 
        if (currentLevel == 0) return 0;

        // '구간 가속형'
        float accelFactor = currentLevel / 10f;
        double addedReward = currentLevel * rewardGrowth * accelFactor;

        //                    형변환                     초반 밸붕방지
        return baseReward + (BigInteger)addedReward + (currentLevel - 1);
    }

    public BigInteger PercentReward(int currentLevel)
    {
        if (!BigInteger.TryParse(intGrowth, out BigInteger growth)) return 0;
        if (currentLevel == 0) return 0;

        return growth * currentLevel;
    }




}