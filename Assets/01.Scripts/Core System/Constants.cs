using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Numerics;

public static class Constants
{
    public const string SAVE_KEY_MONEY = "UserTotalMoney";
    public const string SAVE_KEY_LEVEL = "UserCurrentLevel";

    // 인게임 설정값
    public const float OFFLINE_REWARD_RATE = 0.5f; // 오프라인 수익 50% 적용
    public const int TICK_RATE = 1; // 초당 수익 계산 주기 (1초)

    // Enum: 아이템 등급이나 직업 유형 등
    public enum ItemGrade { Normal, Rare, Epic, Legendary }
    public enum JobType { Outsource, InHouse }
}
