using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Numerics;

[CreateAssetMenu(menuName = "Upgrade")]
public class UpgradeSO : ScriptableObject
{
    [Header("기본 정보")]
    public string ID; // 아이디
    public string Name; // 이름
    public Sprite icon; // 아이콘
    public string description; // 설명

    [Header("업그레이드 카테고리")]
    // public UpgradeCategory category; // 플레이어, 외주, 자체 개발, 컴퍼니
    public StatType statType; // 스탯 종류

    [Header("업그레이드 수치")]
    public int baseCost; // 기본 비용
    public int costIncrease; // 비용 상승량
    public int baseValue; // 기본 값
    public int valueIncrease; // 상승값
    public int maxLevel = 100; // 최대 레벨

    public int GetCost(int level)
    {
        return baseCost + (costIncrease * level);
    }

    public int GetValue(int level)
    {
        return baseValue + (valueIncrease * level);
    }
}
