using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Numerics;
using System.Text;

public static class BigIntegerFormatter
{
    private static readonly string[] KorUnits = { "", "만", "억", "조", "경", "해", "자", "양", "구", "간" };

    public static string Format(BigInteger money)
    {
        // 1만 미만은 천단위 콤마만 찍어서 반환 (예시: 9,500)
        if (money < 10000)
        {
            return money.ToString("N0");
        }

        string moneyStr = money.ToString();
        int length = moneyStr.Length;

        // 4자리마다 단위가 바뀜 (만,억,조)
        // unitIndex: 1이면 만, 2면 억
        int unitIndex = (length - 1) / 4;

        // 배열 범위를 벗어나지 않도록 방어하게
        if (unitIndex >= KorUnits.Length)
            unitIndex = KorUnits.Length - 1;

        // 현재 단위에서 보여줄 정수 부분의 자릿수 (1~4자리)
        int mainDigitsCount = (length - 1) % 4 + 1;

        // 정수 부분 추출 (예시: 123억에서 123)
        string mainPart = moneyStr.Substring(0, mainDigitsCount);

        // 소수점 부분 추출 (정밀도를 위해 뒤의 2자리 사용, 예시: 123.45억)
        // 만약 숫자가 딱 떨어져서 소수점이 없으면 생략하기 위해 조건부 처리
        string subPart = moneyStr.Substring(mainDigitsCount, 2);

        // 소수점 아래가 "00"이면 정수만 보여주고, 아니면 소수점까지 보여줌
        if (subPart == "00")
        {
            return $"{mainPart}{KorUnits[unitIndex]}";
        }
        else
        {
            // "12.34만" 같은 형식
            return $"{mainPart}.{subPart}{KorUnits[unitIndex]}";
        }
    }
}
    