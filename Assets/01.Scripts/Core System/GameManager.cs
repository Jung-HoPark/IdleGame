using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Core Systems")]
    public PlayerAssetManager Asset;
    public ProgressionManager Prog;
    public DataManager SaveLoad;

    [Header("Content Systems")]
    public StartupCompanyManager Startup;
    public EmergencyEventManager Event;
    public StockMarketManager Stock;
    public RankingManager Rank;

    //[Header("UI Systems")]
    public UIManager UI;

    private void Awake()
    {
        // 싱글톤 세팅
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // 씬이 바뀌어도 파괴되지 않음
            InitManagers();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void InitManagers()
    {
        // 자산 매니저가 없을 경우 컴포넌트를 찾아줌
        if (Asset == null) Asset = GetComponent<PlayerAssetManager>();
        if (SaveLoad == null) SaveLoad = GetComponent<DataManager>();

        // 데이터 불러오기
        SaveLoad.Load();
    }
}
