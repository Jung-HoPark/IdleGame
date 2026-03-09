using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/// <summary>
/// 자원 스크립트에서 정보를 가져와 연동 후 UI 갱신합니다.
/// </summary>
public class UIManager : MonoBehaviour
{
    public TabController TabController;

    public TextMeshProUGUI currentGoldText;
    public TextMeshProUGUI currentGoldPerSecondText;
    public TextMeshProUGUI currentGoldPerClickText;
    public TextMeshProUGUI currentDiaText;

    string testRes; // 임시자원

    //테스트용 나중에 게임매니저에서 불러와서 초기화해야 합니다.
    private void Awake()
    {
        Init();
    }

    public void Init()
    {
        UpdateUI();
        TabController.HideAllPanels();
    }

    public void UpdateUI()
    {
        UpdateCurrentGoldUI(testRes);
        UpdateCurrentDiaUI(testRes);
        UpdateGoldPerSecUI(testRes);
        UpdateGoldPerClickUI(testRes);
    }

    public void UpdateCurrentGoldUI(string goldText)
    {
        currentGoldText.text = goldText;
    }
    public void UpdateGoldPerSecUI(string goldText)
    {
        currentGoldPerSecondText.text = goldText;
    }
    public void UpdateGoldPerClickUI(string goldText)
    {
        currentGoldPerClickText.text = goldText;
    }
    public void UpdateCurrentDiaUI(string diaText)
    {
        currentDiaText.text = diaText;
    }


}
