using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Numerics;

public class SelfDevelopment : MonoBehaviour
{
    [SerializeField]
    private bool isUnlocked = false;

    // 기본값에 최소 배율과 최대 배율을 곱함

    [Header("업그레이드 스탯")]
    public string valueStr = "100"; // 기본값
    private BigInteger value;

    [SerializeField]
    private int interval = 5; // 실행 간격

    [SerializeField]
    private int minPercent = 50; // 50%

    [SerializeField]
    private int maxPercent = 150; // 150%

    float timer;

    public bool IsUnlocked
    {
        get { return isUnlocked; }
        private set { isUnlocked = value; }
    }
    public int MinPercent
    {
        get { return minPercent; }
        set
        {
            minPercent = Mathf.Max(0, value); // 최소값이 최대값보다 커지면 최대값도 같이 올림

            if (minPercent > maxPercent)
            {
                maxPercent = minPercent;
            }
        }
    }

    public int MaxPercent
    {
        get { return maxPercent; }
        set
        {
            maxPercent = Mathf.Max(value, minPercent);
        }
    }

    private void Start()
    {
        Debug.Log("자체 개발 시작");
        isUnlocked = true;

        BigInteger.TryParse(valueStr, out value);
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

    public void Unlock()
    {
        isUnlocked = true;
    }

    public BigInteger CalculateIncome()
    {
        int percent = Random.Range(minPercent, maxPercent + 1);

        BigInteger income = value * percent / 100;

        return income;
    }

    public void AddIncome()
    {
        BigInteger income = CalculateIncome();

        GameManager.Instance.Asset.AddAsset(income);

        Debug.Log($"자체 개발에서 {income} 지급");
    }

    public void IncreaseValue(BigInteger amount)
    {
        value += amount;
    }


}
