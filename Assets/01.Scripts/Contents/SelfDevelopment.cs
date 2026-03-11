using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDevelopment : MonoBehaviour
{
    public bool isUnlocked = false; // 해금 여부

    // 기본값에 최소 배율과 최대 배율을 곱함

    [Header("업그레이드 스탯")]
    public int value = 100; // 기본값
    public int interval = 5; // 실행 간격

    public int minPercent = 50; // 50%
    public int maxPercent = 150; // 150%

    float timer;

    private void Start()
    {
        Debug.Log("자체 개발 시작");
        isUnlocked = true;
    }

    private void Update()
    {
        if (!isUnlocked) return;

        timer += Time.deltaTime;

        if (timer >= interval)
        {
            timer -= interval;
            AddIncome();
        }
    }

    public int CalculateIncome()
    {
        int income = 0;
        int percent = Random.Range(minPercent, maxPercent+1);

        income = value * percent / 100;

        return income;
    }

    public void AddIncome()
    {
        int income = CalculateIncome();

        // GameManager.Instance.Asset.AddAsset(CalculateIncome());

        UpgradeManager.Instance.asset += income;
        Debug.Log($"자체 개발에서 {income} 지급");
    }




}
