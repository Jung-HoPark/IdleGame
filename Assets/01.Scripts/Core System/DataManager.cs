using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.Networking;
using static System.Net.WebRequestMethods;

public class DataManager : MonoBehaviour
{
    [SerializeField] private UpgradeManager teamUpgradeManager;

    private void OnApplicationQuit()
    {
        Save();
    }

    // 데이터 저장 로직
    public void Save()
    {
        if (GameManager.Instance.Asset == null) return;

        // 1. 자산 저장 (BigInteger -> String)
        PlayerPrefs.SetString(Constants.SAVE_KEY_MONEY, GameManager.Instance.Asset.TotalAsset.ToString());

        // 2. 변경된 upgradeID를 키로 사용하여 레벨 저장
        foreach (var upgrade in teamUpgradeManager.upgrades)
        {
            int currentLevel = teamUpgradeManager.GetUpgradeLevel(upgrade.upgradeID);
            PlayerPrefs.SetInt($"Level_{upgrade.upgradeID}", currentLevel);
        }

        PlayerPrefs.SetString("LastExitTime", System.DateTime.Now.ToBinary().ToString());
        PlayerPrefs.Save();
    }
    // 데이터 불러오기 로직
    public void Load()
    {
        // 1. 자산 불러오기
        if (PlayerPrefs.HasKey(Constants.SAVE_KEY_MONEY))
        {
            string savedMoney = PlayerPrefs.GetString(Constants.SAVE_KEY_MONEY);
            GameManager.Instance.Asset.TotalAsset = BigInteger.Parse(savedMoney);
        }

        // 2. 레벨 데이터 불러오기 및 팀원 매니저에 강제 주입
        foreach (var upgrade in teamUpgradeManager.upgrades)
        {
            int savedLevel = PlayerPrefs.GetInt($"Level_{upgrade.upgradeID}", 0);

            // 팀원분의 딕셔너리에 직접 접근하여 레벨 설정
            // (UpgradeManager 내 upgradeLevels 딕셔너리가 public이어야 합니다)
            /* if (teamUpgradeManager.upgradeLevels.ContainsKey(upgrade.upgradeID))
            {
                teamUpgradeManager.upgradeLevels[upgrade.upgradeID] = savedLevel;
            }
            */
        }

        CalculateOfflineReward();
    }
    // 오프라인 수익 계산
    public void CalculateOfflineReward()
    {
        if (!PlayerPrefs.HasKey("LastExitTime")) return;

        long temp = long.Parse(PlayerPrefs.GetString("LastExitTime"));
        System.DateTime lastTime = System.DateTime.FromBinary(temp);
        System.TimeSpan diff = System.DateTime.Now - lastTime;

        // 부재 시간(초) 계산
        double seconds = diff.TotalSeconds;

        // (초당 수익 * 부재 시간 * 보정 계수) 만큼 보상 지급
        BigInteger offlineProfit = GameManager.Instance.Asset.CPS * (int)seconds;
        offlineProfit = (offlineProfit * (int)(Constants.OFFLINE_REWARD_RATE * 100)) / 100;

        if (offlineProfit > 0)
        {
            GameManager.Instance.Asset.AddAsset(offlineProfit);
            Debug.Log($"잠든 동안 {offlineProfit}원 획득!");
        }
    }
}
