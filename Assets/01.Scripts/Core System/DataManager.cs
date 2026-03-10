using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.Networking;
using static System.Net.WebRequestMethods;

public class DataManager : MonoBehaviour
{
    private const string sheetURL = "https://docs.google.com/spreadsheets/d/e/2PACX-1vT6ZQ-rLJD74Yd2ObGJvmSZOITQxdVI5jlvzmGxlq5Sg1oCfkCUzpY7BcqcY9AgnDJqHiBsLLLMctH5/pub?gid=151104950&single=true&output=csv";

    public List<CostData> CostList = new List<CostData>();

    // 버튼 연결용 혹은 Start에서 호출용
    public async UniTask LoadDataFromGoogleSheet()
    {
        Debug.Log("구글 시트에서 데이터를 가져오는 중...");
        using (UnityWebRequest www = UnityWebRequest.Get(sheetURL))
        {
            // 비동기로 데이터 요청
            await www.SendWebRequest().ToUniTask();

            if (www.result == UnityWebRequest.Result.Success)
            {
                ParseCSV(www.downloadHandler.text);
            }
            else
            {
                Debug.LogError("데이터 로드 실패: " + www.error);
            }
        }
    }
    private void ParseCSV(string csvText)
    {
        CostList.Clear();

        // 줄바꿈으로 나누기
        string[] lines = csvText.Split(new[] { "\r\n", "\r", "\n" }, System.StringSplitOptions.None);
        for (int i = 1; i < lines.Length; i++) // 첫 줄(헤더) 제외
        {
            if (string.IsNullOrWhiteSpace(lines[i])) continue;

            string[] row = lines[i].Split(',');
            CostData data = new CostData();
            data.ID = int.Parse(row[0]);
            data.Name = row[1];
            data.BaseCost = row[2];
            data.IncreaseRate = float.Parse(row[3]);
            data.Desc = row[4];

            CostList.Add(data);
        }
        Debug.Log($"총 {CostList.Count}개의 데이터를 실시간 반영");
    }

    private void OnApplicationQuit()
    {
        Save();
    }

    // 데이터 저장 로직
    public void Save()
    {
        if (GameManager.Instance.Asset == null) return;

        // BigInteger는 바로 저장할 수 없으므로 문자열(string)로 변환해 저장
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
