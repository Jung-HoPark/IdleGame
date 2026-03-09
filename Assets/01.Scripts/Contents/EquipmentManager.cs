using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Numerics;
public class EquipmentManager : MonoBehaviour
{
    public Equipment equipmentA;
    public Equipment equipmentB;
    public Equipment equipmentC;
    public Equipment equipmentD;

    public bool Purchase(Equipment target)
    {
        if (target.isPurchased) return false;
        if (!target.canPurchase) return false;
        if (!PlayerAssetManager.Instance.DeductAsset(target.itemPrice)) return false;

        target.isPurchased = true;
        ProgressionManager.Instance.UnlockContent(target.unlockContent);
        return true;

    }



}
