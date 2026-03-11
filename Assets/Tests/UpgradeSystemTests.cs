using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using System.Numerics;

public class UpgradeSystemTests
{
    private UpgradeManager _upgradeManager;
    private UpgradeDataSO _testUpgradeSO;
    private PlayerAssetManager _assetManager;

    [SetUp]
    public void SetUp()
    {
        // 1. GameManager 생성 및 싱글톤 수동 설정
        var gameManagerGO = new GameObject("GameManager");
        var gameManager = gameManagerGO.AddComponent<GameManager>();
        
        // Reflection을 사용해 싱글톤 인스턴스 강제 설정 (테스트 환경용)
        var instanceField = typeof(GameManager).GetField("<Instance>k__BackingField", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic);
        instanceField?.SetValue(null, gameManager);

        // 2. 필수 매니저 생성 및 연결
        var assetGO = new GameObject("AssetManager");
        _assetManager = assetGO.AddComponent<PlayerAssetManager>();
        gameManager.Asset = _assetManager; // 중요: GameManager가 Asset을 알게 함

        var upgradeGO = new GameObject("UpgradeManager");
        _upgradeManager = upgradeGO.AddComponent<UpgradeManager>();
        _upgradeManager.upgrades = new List<UpgradeDataSO>();
        gameManager.Upgrade = _upgradeManager; // 중요: GameManager가 Upgrade를 알게 함

        // 3. 테스트용 ScriptableObject 생성
        _testUpgradeSO = ScriptableObject.CreateInstance<UpgradeDataSO>();
        _testUpgradeSO.upgradeID = "TEST_UPGRADE_01";
        _testUpgradeSO.upgradeName = "공격력 업그레이드";
        _testUpgradeSO.maxLevel = 5;
        _testUpgradeSO.type = UpgradeType.ClickPower;
        _testUpgradeSO.baseCostStr = "100";
        _testUpgradeSO.baseRewardStr = "10";

        _upgradeManager.upgrades.Add(_testUpgradeSO);
        
        // 초기화 호출
        _upgradeManager.Init();
    }

    [TearDown]
    public void TearDown()
    {
        // 테스트 간 간섭 방지를 위해 인스턴스 파괴
        if (GameManager.Instance != null) Object.DestroyImmediate(GameManager.Instance.gameObject);
        if (_assetManager != null) Object.DestroyImmediate(_assetManager.gameObject);
        if (_upgradeManager != null) Object.DestroyImmediate(_upgradeManager.gameObject);
    }

    [Test]
    public void Upgrade_Success_WhenAssetsAreEnough()
    {
        // Arrange
        _assetManager.TotalAsset = 1000; // 충분한 돈
        string id = "TEST_UPGRADE_01";

        // Act: 업그레이드 실행
        bool result = _upgradeManager.BuyUpgrade(id);

        // Assert: 결과 검증
        Assert.IsTrue(result, "돈이 충분할 때는 업그레이드가 성공해야 합니다.");
        Assert.AreEqual(1, _upgradeManager.GetUpgradeLevel(id), "업그레이드 후 레벨이 1이 되어야 합니다.");
        Assert.AreEqual((BigInteger)900, _assetManager.TotalAsset, "자산이 차감되어야 합니다.");
    }

    [Test]
    public void Upgrade_Fail_WhenAssetsAreInsufficient()
    {
        // Arrange
        _assetManager.TotalAsset = 0; // 돈이 없음
        string id = "TEST_UPGRADE_01";

        // Act 
        bool result = _upgradeManager.BuyUpgrade(id);

        // Assert
        Assert.IsFalse(result, "돈이 부족할 때는 업그레이드가 실패해야 합니다.");
        Assert.AreEqual(0, _upgradeManager.GetUpgradeLevel(id), "업그레이드 실패 시 레벨은 그대로 0이어야 합니다.");
    }

    [Test]
    public void Upgrade_CannotExceedMaxLevel()
    {
        // Arrange
        _assetManager.TotalAsset = BigInteger.Parse("1000000000000000"); 
        string id = "TEST_UPGRADE_01";
        int maxLvl = _testUpgradeSO.maxLevel;

        // Act: 최대 레벨까지 업그레이드 시도
        for (int i = 0; i < maxLvl + 2; i++) 
        {
            _upgradeManager.BuyUpgrade(id);
        }

        // Assert
        Assert.AreEqual(maxLvl, _upgradeManager.GetUpgradeLevel(id), "최대 레벨을 초과하여 업그레이드될 수 없습니다.");
    }
}

