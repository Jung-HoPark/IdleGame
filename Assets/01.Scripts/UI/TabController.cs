using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TabController : MonoBehaviour
{
    public GameObject[] panels;
    public TabLockUI[] tabLockUIs; // 각 탭에 대응하는 잠금 UI (인스펙터에서 순서대로 할당)

    public void Update()
    {
        // ESC 키를 누르면 모든 패널 닫기
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            HideAllPanels();
        }
    }

    public void SwitchTab(int tabIndex)
    {
        if (tabIndex < 0 || tabIndex >= panels.Length) return;

        // 이미 켜져 있는 탭을 다시 누르면 닫기 (토글 기능)
        if (panels[tabIndex].activeSelf)
        {
            HideAllPanels();
            return;
        }

        // 해당 인덱스에 잠금 UI가 있다면 해금 여부 확인 (로그 출력용)
        if (tabIndex < tabLockUIs.Length && tabLockUIs[tabIndex] != null)
        {
            ContentType type = tabLockUIs[tabIndex].contentType;
            if (type != ContentType.None && GameManager.Instance != null && GameManager.Instance.Prog != null)
            {
                if (!GameManager.Instance.Prog.IsContentUnlocked(type))
                {
                    Debug.Log($"<color=yellow>[안내]</color> {type} 컨텐츠는 현재 잠겨있습니다. (패널 내부에서 조건 표시)");
                    // 이제 여기서 return하지 않습니다. 패널을 열어서 내부 TabLockUI가 작동하게 합니다.
                }
            }
        }
        
    UnlockPass:
       HideAllPanels(); 

        panels[tabIndex].SetActive(true);
    }

    public void HideAllPanels()
    {
        for (int i = 0; i < panels.Length; i++)
        {
            panels[i].SetActive(false);
        }
    }

    public void RefreshAllTabs()
    {
        foreach (var lockUI in tabLockUIs)
        {
            if (lockUI != null) lockUI.RefreshUI();
        }
    }
}
