using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Numerics;
using Cysharp.Threading.Tasks;
using System.Threading;

public class PlayerAssetManager : MonoBehaviour
{
    [Header("Current Assets")]
    public BigInteger TotalAsset = 0;   // 전체 자산
    public BigInteger Dia = 0;

    [Header("Income Stats")]
    public BigInteger CPS = 0;          // 초당 수익
    public BigInteger GoldPerClick = 1; // 클릭당 수익
    public BigInteger GoldPerMin => CPS * 60;

    public int globalIncomePercent = 100; // 기본값에 곱해지는 배율

    [Header("Settings")]
    public bool isProfitFrozen = false; // 이벤트로 인한 수익 정지 상태 체크

    private CancellationTokenSource _cts;

    public int GlobalIncomePercent
    {
        get { return globalIncomePercent; }
        set
        {
            globalIncomePercent = value;
        }
    }
    public void Init()
    {
        _cts = new CancellationTokenSource();
        ProfitLoopTask(_cts.Token).Forget();

        // (UI 업데이트는 어차피 GameManager에서 나중에 해줄 거니 여기서 뺍니다)
    }
    private void OnDestroy()
    {
        // 오브젝트 파괴 시 루프 안전하게 종료
        _cts?.Cancel();
        _cts?.Dispose();
    }
    // 1초마다 자산을 추가하는 비동기 루프
    private async UniTaskVoid ProfitLoopTask(CancellationToken token)
    {
        while (!token.IsCancellationRequested)
        {
            await UniTask.Delay(1000, delayTiming: PlayerLoopTiming.Update, cancellationToken: token);

            if (!isProfitFrozen && CPS > 0)
            {
                AddAsset(CPS);
            }
        }
    }
    // 자산 추가 (클릭, 외주 수익, 대박 이벤트 등)
    public void AddAsset(BigInteger amount)
    {
        if (amount <= 0) return;

        BigInteger finalAmount = amount * globalIncomePercent / 100;
        TotalAsset += finalAmount;

        // UI 업데이트 알림 (UIManager와 연동될 부분)
        if (GameManager.Instance.UI != null)
        {
            GameManager.Instance.UI.UpdateAssetUI();
        }
    }
    // 자산 차감 (아이템 구매, 외주 해금, 쪽박 이벤트 등)
    // 차감 성공 여부를 bool로 반환해서 구매 로직에서 활용함
    public bool DeductAsset(BigInteger amount)
    {
        if (TotalAsset < amount)
        {
            Debug.Log("자산이 부족합니다!");
            return false;
        }

        TotalAsset -= amount;

        if (GameManager.Instance.UI != null)
        {
            GameManager.Instance.UI.UpdateAssetUI();
        }
        return true;
    }
}
