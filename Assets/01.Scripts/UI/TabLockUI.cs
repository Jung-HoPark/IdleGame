using UnityEngine;
using TMPro;

public class TabLockUI : MonoBehaviour
{
    public ContentType contentType;
    
    [Header("UI Elements")]
    public GameObject lockOverlay;   // 잠금 시 활성화될 레이어
    public TextMeshProUGUI conditionText; // 해금 조건 텍스트

    private void OnEnable()
    {
        // 패널이 켜질 때 즉시 상태 확인
        RefreshUI();
    }

    public void RefreshUI()
    {
        // 1. 실행 여부 확인용 로그
        // Debug.Log($"[TabLockUI] RefreshUI() 호출됨 - 오브젝트: {gameObject.name}, 현재 타입: {contentType}");

        // 상점 등 해금 조건이 없는 컨텐츠는 즉시 잠금 해제
        if (contentType == ContentType.None)
        {
            if (lockOverlay != null) lockOverlay.SetActive(false);
            if (conditionText != null) conditionText.text = ""; 
            return;
        }

        // 2. 인스턴스 체크 및 자동 할당 시도
        var gm = GameManager.Instance;
        if (gm == null)
        {
            Debug.LogWarning($"[TabLockUI] GameManager.Instance가 null입니다. (오브젝트: {gameObject.name})");
            return;
        }

        var prog = gm.Prog;
        if (prog == null)
        {
            // 만약 GameManager에 Prog가 연결 안 되어 있다면 씬에서 직접 찾아보기 (최후의 수단)
            prog = FindFirstObjectByType<ProgressionManager>();
            if (prog == null)
            {
                Debug.LogError($"[TabLockUI] 씬에서 ProgressionManager를 찾을 수 없습니다! (오브젝트: {gameObject.name})");
                return;
            }
        }

        bool isUnlocked = prog.IsContentUnlocked(contentType);
        
        // 패널 전체를 덮는 Overlay 활성화 제어
        if (lockOverlay != null)
        {
            lockOverlay.SetActive(!isUnlocked);
        }

        // 잠겨있을 때만 해금 조건 텍스트 표시
        if (!isUnlocked && conditionText != null)
        {
            conditionText.text = GetConditionString();
            // Debug.Log($"[TabLockUI] {gameObject.name} 텍스트 업데이트: {conditionText.text}");
        }
        else if (isUnlocked && conditionText != null)
        {
            conditionText.text = ""; 
        }
    }

    private string GetConditionString()
    {
        switch (contentType)
        {
            case ContentType.Outsource:
                return "키보드 Lv.100 달성 시 해금";
            case ContentType.SelfDevelopment:
                return "자산 1,000만 원 달성 시 해금";
            case ContentType.Company:
                return "자산 1억 원 달성 시 해금";
            case ContentType.Stock:
                return "자산 10억 원 달성 시 해금";
            default:
                return "잠겨있음";
        }
    }
}
