using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using Unity.VisualScripting;
using UnityEngine;

public class OutSourcing : MonoBehaviour
{
    [SerializeField]
    private bool isUnlocked = false; // 해금 여부

    [Header("업그레이드 스탯")]
    public string valueStr = "100"; // 초기값
    private BigInteger value;

    [SerializeField]
    private int interval = 5; // 실행 간격
    private int valuePercent = 100; // 기본값에 곱해지는 배율

    [SerializeField]
    private int critChance = 30;

    [SerializeField]
    private int critDamage = 200;


    float timer;
    
    public bool IsUnlocked
    {
        get { return isUnlocked; }
        private set { isUnlocked = value; }
    }
    public int ValuePercent
    {
        get { return valuePercent; }
        set { valuePercent = value;}
    }

    public int CritChance
    {
        get
        { return critChance; }
        set
        {
            critChance = Mathf.Clamp(value, 0, 100); // 최대 100까지
        }
    }
    public int CritDamage
    {
        get
        { return critDamage; }
        set
        {
            critDamage = Mathf.Max(value, 100); // 최소 100 이상
        }
    }
    
    private void Start()
    {
        Debug.Log("외주 시작");

        if (!BigInteger.TryParse(valueStr, out value))
        {
            value = 0;
            Debug.LogWarning("valueStr 파싱 실패");
        }
    }

    private void Update()
    {
        if (!IsUnlocked) return;

        timer += Time.deltaTime;

        if (timer >= interval)
        {
            timer -= interval;
            AddIncome();
        }
    }

    public BigInteger CalculateIncome()
    {
        BigInteger income = value * ValuePercent / 100;

        if (Random.Range(0, 100) < CritChance)
        {
            Debug.Log("외주 치명타 발생");
            income = income * CritDamage / 100;
        }

        return income;
    }

    public void AddIncome()
    {
        BigInteger income = CalculateIncome();
        GameManager.Instance.Asset.AddAsset(income);
        Debug.Log($"외주에서 {income} 지급");
    }

    public void IncreaseValue(BigInteger amount)
    {
        value += amount;
    }
    public void Unlock()
    {
        IsUnlocked = true;
    }
}