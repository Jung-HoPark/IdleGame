using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Numerics;

public class PurchaseManager : MonoBehaviour
{
    public List<PurchaseItem> myPurchase = new List<PurchaseItem>();

    // 게임 시작 시 혹은 데이터 로드 후 호출
    public void RefreshTotalCPS()
    {
        BigInteger total = 0;
        foreach (var item in myPurchase)
        {
            total += item.GetCurrentCPS();
        }

        // 실제 자산 매니저의 CPS 업데이트
        GameManager.Instance.Asset.CPS = total;
        Debug.Log($"전체 CPS 업데이트 완료: {total}");
    }
    // 레벨업 버튼 클릭 시 호출할 함수
    public void UpgradeItem(int id)
    {
        var item = myPurchase.Find(x => x.dataID == id);
        if (item == null) return;

        // 비용 계산
        BigInteger cost = CalculateCost(item);

        // 돈이 충분하면 차감 후 레벨업
        if (GameManager.Instance.Asset.DeductAsset(cost))
        {
            item.currentLevel++;
            RefreshTotalCPS(); // 수익 재계산
        }
    }
    private BigInteger CalculateCost(PurchaseItem item)
    {
        var data = item.GetData();
        // 비용 = 기초비용 * (성장계수 ^ 현재레벨)
        double multiplier = System.Math.Pow(data.IncreaseRate, item.currentLevel);
        return BigInteger.Parse(data.BaseCost) * (BigInteger)multiplier;
    }
}
