using System;
using System.Collections.Generic;
using System.IO;
using System.Numerics;
using UnityEngine;

// 1. 저장할 데이터 구조 정의
[Serializable]
public class SaveData
{
    public string totalMoney;      // BigInteger는 string으로 저장
    public string lastExitTime;
    public List<UpgradeLevelData> upgradeLevels = new List<UpgradeLevelData>();
}

[Serializable]
public class UpgradeLevelData
{
    public string id;
    public int level;
}

public class DataManager : MonoBehaviour
{
    [SerializeField] private UpgradeManager teamUpgradeManager;
    private string savePath;

    private void Awake()
    {
        // 저장 경로 설정 (PC, 모바일 모두 대응)
        savePath = Path.Combine(Application.persistentDataPath, "savefile.json");
    }

    private void OnApplicationQuit()
    {
        Save();
    }

    // 데이터 저장 로직
    public void Save()
    {
        if (GameManager.Instance.Asset == null) return;

        SaveData data = new SaveData();

        // 데이터 채우기
        data.totalMoney = GameManager.Instance.Asset.TotalAsset.ToString();
        data.lastExitTime = DateTime.Now.ToBinary().ToString();

        foreach (var upgrade in teamUpgradeManager.upgrades)
        {
            data.upgradeLevels.Add(new UpgradeLevelData
            {
                id = upgrade.upgradeID,
                level = teamUpgradeManager.GetUpgradeLevel(upgrade.upgradeID)
            });
        }

        // JSON으로 변환 후 파일 쓰기
        string json = JsonUtility.ToJson(data, true); // true는 보기 좋게 정렬
        File.WriteAllText(savePath, json);
        Debug.Log($"데이터 저장 완료: {savePath}");
    }
    // 데이터 불러오기 로직
    public void Load()
    {
        if (!File.Exists(savePath)) return;

        string json = File.ReadAllText(savePath);
        SaveData data = JsonUtility.FromJson<SaveData>(json);

        // 1. 자산 복구
        GameManager.Instance.Asset.TotalAsset = BigInteger.Parse(data.totalMoney);

        // 2. 레벨 복구 및 강제 주입
        foreach (var levelData in data.upgradeLevels)
        {
            if (teamUpgradeManager.upgradeLevels.ContainsKey(levelData.id))
            {
                teamUpgradeManager.upgradeLevels[levelData.id] = levelData.level;
            }
        }

        CalculateOfflineReward(data.lastExitTime);
    }
    // 오프라인 수익 계산
    public void CalculateOfflineReward(string lastTimeBinary)
    {
        if (string.IsNullOrEmpty(lastTimeBinary)) return;

        // 1. 시간 차이 계산
        long temp = long.Parse(lastTimeBinary);
        DateTime lastTime = DateTime.FromBinary(temp);
        TimeSpan diff = DateTime.Now - lastTime;

        // 2. 부재 시간(초) 가져오기
        double totalSeconds = diff.TotalSeconds;
        
        // 3. 오프라인 최대 시간 설정 (20시간)
        double maxOfflineSeconds = 20 * 3600;
        double appliedSeconds = Math.Min(totalSeconds, maxOfflineSeconds);

        BigInteger offlineProfit = GameManager.Instance.Asset.CPS * (int)appliedSeconds;
        offlineProfit = (offlineProfit * (int)(Constants.OFFLINE_REWARD_RATE * 100)) / 100;

        if (offlineProfit > 0)
        {
            GameManager.Instance.Asset.AddAsset(offlineProfit);
            string timeText = (totalSeconds > maxOfflineSeconds) ? "20시간(최대)" : $"{(int)(totalSeconds / 60)}분";
            Debug.Log($"[오프라인 보상] {timeText} 동안 {offlineProfit}원 획득");
        }
    }
}
