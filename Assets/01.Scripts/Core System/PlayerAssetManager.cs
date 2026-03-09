using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Numerics;

public class PlayerAssetManager : MonoBehaviour
{
    [Header("Current Assets")]
    public BigInteger TotalAsset = 0;   // 전체 자산
    public int Dia = 0;

    [Header("Income Stats")]
    public BigInteger CPS = 0;          // 초당 수익
    public BigInteger GoldPerClick = 1; // 클릭당 수익
    public BigInteger GoldPerMin => CPS * 60;

    [Header("Settings")]
    public bool isProfitFrozen = false; // 이벤트로 인한 수익 정지 상태 체크

    private float timer = 0f;

    private void Update()
    {
        // 1초마다 초당 수익(CPS)을 더해줌
        if (!isProfitFrozen)
        {
            timer += Time.deltaTime;
            if (timer >= 1f)
            {
                AddAsset(CPS);
                timer = 0f;
            }
        }
    }
    // 자산 추가 (클릭, 외주 수익, 대박 이벤트 등)
    public void AddAsset(BigInteger amount)
    {
        if (amount <= 0) return;

        TotalAsset += amount;

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
