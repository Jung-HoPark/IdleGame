using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    // !!만들어지면 헤더 지우기!!
    //[Header("Core Systems")]
    //public PlayerAssetManager Asset;
    //public ProgressionManager Prog;
    //public DataManager SaveLoad;

    //[Header("Content Systems")]
    //public StartupCompanyManager Startup;
    //public EmergencyEventManager Event;
    //public StockMarketManager Stock;
    //public RankingManager Rank;

    //[Header("UI Systems")]
    //public UIManager UI;

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
        // 여기서 각 매니저들의 초기화 순서 제어
        Debug.Log("모든 시스템 초기화 완료!");
    }
}
