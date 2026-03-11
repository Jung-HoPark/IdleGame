using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Numerics;

public class PurchaseManager : MonoBehaviour
{
    [SerializeField] private UpgradeManager teamUpgradeManager;

    // 게임 시작 시 혹은 데이터 로드 후 호출
    public void RefreshTotalCPS()
    {
        BigInteger total = 0;

        // 리스트(upgrades)를 돌면서 수치를 합산
        foreach (var upgrade in teamUpgradeManager.upgrades)
        {
            // 변수 upgradeID 사용
            int level = teamUpgradeManager.GetUpgradeLevel(upgrade.upgradeID);

            // GetReward 함수가 BigInteger를 반환하므로 바로 더함
            total += upgrade.GetReward(level);
        }

        // 실제 자산 매니저의 CPS 업데이트
        GameManager.Instance.Asset.CPS = total;
        Debug.Log($"전체 CPS 동기화 완료: {total}");
    }
    // UI에서 버튼을 누를 때 호출 (BuyUpgrade 함수와 연결)
    public void RequestUpgrade(string id)
    {
        // 함수 BuyUpgrade 호출
        if (teamUpgradeManager.BuyUpgrade(id))
        {
            // 구매 성공 시 수익률 재계산 (UI 갱신 등은 RefreshTotalCPS 내부 혹은 외부에서 처리)
            RefreshTotalCPS();
        }
    }
}
