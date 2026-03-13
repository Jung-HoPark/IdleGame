using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgressionManager : MonoBehaviour
{
    // 각 기능의 해금 상태 (세이브 데이터 연동 필요)
    [Header("Unlock States")]
    public bool isOutsourceUnlocked = false;
    public bool isSelfDevelopmentUnlocked = false;
    public bool isCompanyUnlocked = false;
    public bool isStockUnlocked = false;

    private float _checkTimer = 0f;
    private const float CHECK_INTERVAL = 1.0f; // 1초 주기 체크

    private void Update()
    {
        // 성능을 고려한 주기적 폴링 체크 , 나중에 Action으로 변경 예정
        _checkTimer += Time.deltaTime;
        if (_checkTimer >= CHECK_INTERVAL)
        {
            _checkTimer = 0f;
            CheckUnlockConditions();
        }
    }

    /// <summary>
    /// 조건별 해금 처리 및 UI 갱신
    /// </summary>
    public void CheckUnlockConditions()
    {
        // 1. 외주: 키보드(PLAYER_CLICK_01) 100레벨 달성 시
        if (!isOutsourceUnlocked)
        {
            int keyboardLevel = GameManager.Instance.Upgrade.GetUpgradeLevel("PLAYER_CLICK_01");
            if (keyboardLevel >= 100)
            {
                isOutsourceUnlocked = true;
                Debug.Log("<color=yellow>[Unlock]</color> 외주 컨텐츠 활성화");
                GameManager.Instance.UI.TabController.RefreshAllTabs();
            }
        }

        // 2. 자체개발: 자산 1000만 원 이상
        if (!isSelfDevelopmentUnlocked)
        {
            if (GameManager.Instance.Asset.TotalAsset >= 10000000)
            {
                isSelfDevelopmentUnlocked = true;
                Debug.Log("<color=yellow>[Unlock]</color> 자체 앱 개발 컨텐츠 활성화");
                GameManager.Instance.UI.TabController.RefreshAllTabs();
            }
        }

        // 3. 창업: 자산 1억 원 이상
        if (!isCompanyUnlocked)
        {
            if (GameManager.Instance.Asset.TotalAsset >= 100000000)
            {
                isCompanyUnlocked = true;
                Debug.Log("<color=yellow>[Unlock]</color> 창업 컨텐츠 활성화");
                GameManager.Instance.UI.TabController.RefreshAllTabs();
            }
        }

        // 4. 주식: 자산 10억 원 이상
        if (!isStockUnlocked)
        {
            if (GameManager.Instance.Asset.TotalAsset >= 1000000000)
            {
                isStockUnlocked = true;
                Debug.Log("<color=yellow>[Unlock]</color> 주식 컨텐츠 활성화");
                GameManager.Instance.UI.TabController.RefreshAllTabs();
            }
        }
    }

    /// <summary>
    /// 컨텐츠 타입에 따른 해금 상태 반환
    /// </summary>
    public bool IsContentUnlocked(ContentType type)
    {
        switch (type)
        {
            case ContentType.Outsource: return isOutsourceUnlocked;
            case ContentType.SelfDevelopment: return isSelfDevelopmentUnlocked;
            case ContentType.Company: return isCompanyUnlocked;
            case ContentType.Stock: return isStockUnlocked;
            default: return true; 
        }
    }
}
