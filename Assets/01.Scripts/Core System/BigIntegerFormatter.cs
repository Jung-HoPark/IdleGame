using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Numerics;

public static class BigIntegerFormatter
{
    public static string Format(BigInteger money)
    {
        if (money < 10000) return money.ToString();

        // 한글 단위 버전 (만, 억, 조, 경...)
        string[] units = { "", "만", "억", "조", "경", "해" };
        int unitIndex = 0;
        BigInteger tempMoney = money;
        string result = "";

        // 현재 로직: 가장 큰 단위 2개 정도만 보여주는 방식
        return money.ToString("N0");
    }
}
