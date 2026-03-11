using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Core Systems")]
    public PlayerAssetManager Asset;
    public ProgressionManager Prog;
    public DataManager SaveLoad;
    public ScreenClicker screenClicker;

    [Header("Content Systems")]
    public PurchaseManager Purchase;
    public UpgradeManager Upgrade;
    // public StartupCompanyManager Startup;
    // public EmergencyEventManager Event;
    // public StockMarketManager Stock;
    // public RankingManager Rank;

    [Header("UI Systems")]
    public UIManager UI;
    public ObjectPoolManager Pool;

    private void Awake()
    {
        // 싱글톤 세팅
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // 씬이 바뀌어도 파괴되지 않음
            InitializeGame().Forget();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private async UniTaskVoid InitializeGame()
    {
        // 1. 기초 UI 및 오브젝트 풀 초기화
        if(Asset != null) Asset.Init();
        if(Upgrade != null) Upgrade.Init();
        if (Pool != null) { /* Pool 초기화 로직이 있다면 여기서 실행 */ }

        if (UI != null) UI.Init();

        // 2. 구글 시트 데이터 로드 (기준 데이터)
        await SaveLoad.LoadDataFromGoogleSheet();

        // 3. 로컬 저장 데이터 불러오기 (오프라인 수익 계산 포함)
        SaveLoad.Load();

        if (UI != null) UI.UpdateAssetUI();

        // 4. 콘텐츠 매니저 초기화 (시트 데이터가 들어온 후 실행)
        if (Purchase != null)
        {
            // 여기서 시트 데이터를 기반으로 아이템 리스트를 구성하거나
            // 현재 저장된 레벨을 바탕으로 CPS를 계산
            Purchase.RefreshTotalCPS();
        }
    }
}
