using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PurchaseItem
{
    public int dataID;       // 구글 시트의 ID와 매칭
    public int currentLevel; // 현재 업그레이드 레벨

    // 시트에서 가져온 원본 데이터 참조
    public CostData GetData()
    {
        return GameManager.Instance.SaveLoad.CostList.Find(x => x.ID == dataID);
    }

    // 현재 레벨에서의 초당 수익(CPS) 계산
    public System.Numerics.BigInteger GetCurrentCPS()
    {
        var data = GetData();
        if (data == null || currentLevel == 0) return 0;

        // 현재: 기초수익 * 레벨 (더 복잡한 수식도 가능)
        return System.Numerics.BigInteger.Parse(data.BaseCost) * currentLevel;
    }
}
