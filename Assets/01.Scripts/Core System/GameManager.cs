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

        // 2. 로컬 저장 데이터 불러오기 (비동기 경고 해결을 위해 await UniTask.Yield 추가)
        SaveLoad.Load();
        await UniTask.Yield();

        if (UI != null) UI.UpdateAssetUI();

        // 3. 수익률 동기화 (로드된 레벨 기반)
        if (Purchase != null)
        {
            Purchase.RefreshTotalCPS();
        }
    }
}
