using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class OutSourcing : MonoBehaviour
{
    public bool isUnlocked = false; // 해금 여부

    [Header("업그레이드 스탯")]
    public int value = 100; // 기본값
    public int interval = 5; // 실행 간격
    [Tooltip("치명타 확률 (0~1, 예: 0.85f = 85%)")]
    public float critChance = 0.3f; 
    [Tooltip("치명타 배율 (2f = 200%)")]
    public float critDamage = 2f; 


    float timer;

    private void Start()
    {
        Debug.Log("외주 시작");
        isUnlocked = true;
    }

    private void Update()
    {
        if (!isUnlocked) return;

        timer += Time.deltaTime;

        if(timer >= interval)
        {
            timer -= interval;
            AddIncome();
        }    
    }

    public int CalculateIncome()
    {
        int income = value;
        
        if(Random.value < critChance)
        {
            Debug.Log("외주 치명타 발생");
            income = (int)(income * critDamage);
        }

        return income;
    }

    public void AddIncome()
    {
        int income = CalculateIncome();
        // GameManager.Instance.Asset.AddAsset(CalculateIncome());
        UpgradeManager.Instance.asset += income;
        Debug.Log($"외주에서 {income} 지급");
    }

}
