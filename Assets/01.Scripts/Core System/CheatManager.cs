using UnityEngine;
using System.Numerics;
using UnityEngine.UI;

public class CheatManager : MonoBehaviour
{
    [Header("UI 연결")]
    public GameObject cheatPanel;

    [Header("외부 매니저 연결")]
    public RoomEnvironment roomEnv;

    private bool isPanelOpen = false;

    private int currentCheatMapLevel = 0;

    private void Update()
    {
        if (!cheatPanel.activeSelf) return;

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ToggleCheatPanel();
        }
    }

    // 치트 패널 열기/닫기
    public void ToggleCheatPanel()
    {
        isPanelOpen = !isPanelOpen;
        cheatPanel.SetActive(isPanelOpen);

        //cheatPanel.SetActive(!cheatPanel.activeSelf);
    }

    // 금액 증가 치트
    public void Cheat_AddMoney()
    {
        BigInteger cheatAmount = BigInteger.Parse("1000000000000000000");
        GameManager.Instance.Asset.AddAsset(cheatAmount);
        BigInteger currentTotal = GameManager.Instance.Asset.TotalAsset;

        Debug.Log(BigIntegerFormatter.Format(cheatAmount) + "원 입금완료\n" +
            " 총 금액 :" + BigIntegerFormatter.Format(currentTotal));

    }

    // 맵 변경 치트
    public void Cheat_ChangeMap()
    {
        if (roomEnv == null) return;

        currentCheatMapLevel++;
        if (currentCheatMapLevel >= roomEnv.roomLevelSprites.Length)
        {
            currentCheatMapLevel = 0;
        }

        roomEnv.ChangeRoomLevel(currentCheatMapLevel);
        Debug.Log($" 맵 강제 변경 {currentCheatMapLevel}");
    }

    // 저장 정보 초기화 치트
    public void Cheat_ResetSave()
    {
        // 저장 방지 (DataManager의 OnApplicationQuit 차단)
        if (GameManager.Instance.SaveLoad != null)
        {
            GameManager.Instance.SaveLoad.isFirstReset = true;
        }

        // 하드디스크(PlayerPrefs) 삭제
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();

        // 메모리 데이터 즉시 초기화
        var asset = GameManager.Instance.Asset;
        if (asset != null)
        {
            asset.TotalAsset = 0;
            asset.Dia = 0;
            asset.CPS = 0;
            asset.GoldPerClick = 1; // 0이 아닌 1로 초기화!
        }

        Debug.Log("초기화 완료.");

        // 플랫폼별 재시작 처리
#if UNITY_EDITOR
        UnityEngine.SceneManagement.SceneManager.LoadScene(
            UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
#else
    Application.Quit(); 
#endif
    }

    // 튜토리얼 스킵 치트
    public void Cheat_SkipTutorial()
    {
        //PlayerPrefs.DeleteKey("isTutorialCompleted");
        Debug.Log("튜토리얼 스킵");
    }

    // 해금 치트 (모든 콘텐츠 강제 오픈) 
    public void Cheat_UnlockAll()
    {
        // TODO : 해금 관련 코드 추가
        Debug.Log(" 모든 콘텐츠 강제 해금");
    }
}