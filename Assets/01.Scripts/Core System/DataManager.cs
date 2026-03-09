using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Numerics;

public class DataManager : MonoBehaviour
{
    private void OnApplicationQuit()
    {
        Save();
    }

    // 데이터 저장 로직
    public void Save()
    {
        if (GameManager.Instance.Asset == null) return;

        // BigInteger는 바로 저장할 수 없으므로 문자열(string)로 변환해 저장해
        string moneyStr = GameManager.Instance.Asset.TotalAsset.ToString();
        PlayerPrefs.SetString(Constants.SAVE_KEY_MONEY, moneyStr);

        // 마지막 종료 시간 저장 (오프라인 수익 계산용)
        PlayerPrefs.SetString("LastExitTime", System.DateTime.Now.ToBinary().ToString());

        PlayerPrefs.Save(); // 디스크에 즉시 반영
        Debug.Log("데이터 저장 완료: " + moneyStr);
    }
    // 데이터 불러오기 로직
    public void Load()
    {
        // 자산 불러오기
        if (PlayerPrefs.HasKey(Constants.SAVE_KEY_MONEY))
        {
            string savedMoney = PlayerPrefs.GetString(Constants.SAVE_KEY_MONEY);
            GameManager.Instance.Asset.TotalAsset = BigInteger.Parse(savedMoney);
        }
        else
        {
            // 저장된 데이터가 없으면 초기값 설정
            GameManager.Instance.Asset.TotalAsset = 0;
        }

        CalculateOfflineReward();
        Debug.Log("데이터 로드 완료!");
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
