using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class UpgradeSystemTests
{
    private UpgradeManager _upgradeManager;
    private UpgradeSO _testUpgradeSO;

    [SetUp]
    public void SetUp()
    {
        // Arrange: 테스트를 위한 환경 세팅
        var go = new GameObject();
        _upgradeManager = go.AddComponent<UpgradeManager>();
        _upgradeManager.upgrades = new List<UpgradeSO>();

        // 테스트용 ScriptableObject를 가상으로 생성합니다.
        _testUpgradeSO = ScriptableObject.CreateInstance<UpgradeSO>();
        _testUpgradeSO.ID = "TEST_UPGRADE_01";
        _testUpgradeSO.Name = "공격력 업그레이드";
        _testUpgradeSO.maxLevel = 5;
        _testUpgradeSO.statType = StatType.ClickIncome;

        _upgradeManager.upgrades.Add(_testUpgradeSO);
        
        // Start()를 강제로 호출하여 사전을 구성하게 합니다.
        // 실제 게임에서는 유니티가 해주지만, 테스트 코드에서는 수동으로 호출 가능합니다.
        var method = typeof(UpgradeManager).GetMethod("Start", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        method?.Invoke(_upgradeManager, null);
    }

    [Test]
    public void Upgrade_Success_WhenAssetsAreEnough()
    {
        // Arrange
        _upgradeManager.asset = 1000; // 충분한 돈
        string id = "TEST_UPGRADE_01";

        // Act: 업그레이드 실행
        bool result = _upgradeManager.Upgrade(id);

        // Assert: 결과 검증
        Assert.IsTrue(result, "돈이 충분할 때는 업그레이드가 성공해야 합니다.");
        Assert.AreEqual(1, _upgradeManager.GetUpgradeLevel(id), "업그레이드 후 레벨이 1이 되어야 합니다.");
    }

    [Test]
    public void Upgrade_Fail_WhenAssetsAreInsufficient()
    {
        // Arrange
        _upgradeManager.asset = 0; // 돈이 없음
        string id = "TEST_UPGRADE_01";

        // Act 
        bool result = _upgradeManager.Upgrade(id);

        // Assert
        Assert.IsFalse(result, "돈이 부족할 때는 업그레이드가 실패해야 합니다.");
        Assert.AreEqual(0, _upgradeManager.GetUpgradeLevel(id), "업그레이드 실패 시 레벨은 그대로 0이어야 합니다.");
    }

    [Test]
    public void Upgrade_CannotExceedMaxLevel()
    {
        // Arrange
        _upgradeManager.asset = 999999; 
        string id = "TEST_UPGRADE_01";
        int maxLvl = _testUpgradeSO.maxLevel;

        // Act: 최대 레벨까지 업그레이드 시도
        for (int i = 0; i < maxLvl + 2; i++) 
        {
            _upgradeManager.Upgrade(id);
        }

        // Assert
        Assert.AreEqual(maxLvl, _upgradeManager.GetUpgradeLevel(id), "최대 레벨을 초과하여 업그레이드될 수 없습니다.");
    }
}
