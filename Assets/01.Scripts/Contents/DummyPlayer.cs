using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

public class DummyPlayer : MonoBehaviour
{
    public static DummyPlayer Instance;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }


    public BigInteger TotalAsset = 0;
    public BigInteger MoneyPerClick = 1;

    public void AddAsset(BigInteger amount)
    {
        TotalAsset += amount;
    }

    public bool DeductAsset(BigInteger amount)
    {
        if (TotalAsset < amount)
        {
            Debug.Log("자산이 부족합니다!");
            return false;
        }

        TotalAsset -= amount;
        return true;
    }
}
