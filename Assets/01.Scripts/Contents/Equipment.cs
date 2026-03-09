using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Numerics;
using Unity.Loading;

[System.Serializable]
public class Equipment
{
    public string itemName;
    public BigInteger itemPrice;
    public bool canPurchase; // 전 아이템을 구매해서 구매 가능한 상태인지
    public bool isPurchased;
    public ContentType unlockContent;

    public Equipment(string name, BigInteger price, ContentType unlock)
    {
        itemName = name;
        itemPrice = price;
        unlockContent = unlock;
    }
}
