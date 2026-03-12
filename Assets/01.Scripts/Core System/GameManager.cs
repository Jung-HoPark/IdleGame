using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // 1. 싱글톤 인스턴스 (수정 불가능하게 캡슐화)
    private static GameManager _instance;
    public static GameManager Instance
    {
        get
        {
            if (_instance == null) return null;
            return _instance;
        }
    }

    // 2. Core Systems (직렬화 필드는 private, 외부는 Read-Only 프로퍼티로 노출)
    [Header("Core Systems")]
    [SerializeField] private PlayerAssetManager _asset;
    public PlayerAssetManager Asset => _asset;

    [SerializeField] private ProgressionManager _prog;
    public ProgressionManager Prog => _prog;

    [SerializeField] private DataManager _saveLoad;
    public DataManager SaveLoad => _saveLoad;

    [SerializeField] private ScreenClicker _screenClicker;
    public ScreenClicker ScreenClicker => _screenClicker;

    // 3. Content Systems
    [Header("Content Systems")]
    [SerializeField] private PurchaseManager _purchase;
    public PurchaseManager Purchase => _purchase;

    [SerializeField] private UpgradeManager _upgrade;
    public UpgradeManager Upgrade => _upgrade;

    [SerializeField] private OutSourcing _outSourcing;
    public OutSourcing OutSourcing => _outSourcing;

    [SerializeField] private SelfDevelopment _selfDevelopment;
    public SelfDevelopment SelfDevelopment => _selfDevelopment;

    [SerializeField] private Company _company;
    public Company Company => _company;

    // 4. UI Systems
    [Header("UI Systems")]
    [SerializeField] private UIManager _ui;
    public UIManager UI => _ui;

    [SerializeField] private ObjectPoolManager _pool;
    public ObjectPoolManager Pool => _pool;

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeGame().Forget();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private async UniTaskVoid InitializeGame()
    {
        // 내부 필드(_asset 등)를 사용하여 초기화 진행
        if (_asset != null) _asset.Init();
        if (_upgrade != null) _upgrade.Init();
        if (_ui != null) _ui.Init();

        // 데이터 로드
        if (_saveLoad != null) _saveLoad.Load();
        await UniTask.Yield();

        if (_ui != null) _ui.UpdateAssetUI();

        // 수익률 동기화
        if (_purchase != null)
        {
            _purchase.RefreshTotalCPS();
        }
    }
}
