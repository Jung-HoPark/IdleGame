using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class UpgradeEffectData
{
    [Header("해당 스탯")]
    public StatType statType;

    [Header("계산 방식")]
    public ModifierType modifierType;

    [Header("초기값")]
    public double baseValue;

    [Header("레벨당 증가량")]
    public double valueIncrease;

    public double GetValueAtLevel(int level)
    {
        return baseValue + (valueIncrease * level);
    }
}
