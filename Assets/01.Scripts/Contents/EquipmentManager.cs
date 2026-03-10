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
        // 1. 이미 구매했는지, 구매 가능한 상태인지 먼저 체크
        if (target.isPurchased) return false;
        if (!target.canPurchase) return false;


        // 2. 자산 차감 시도 (성공하면 true, 부족하면 false 반환됨)
        // 여기서 PlayerAssetManager 내부의 DeductAsset 로직이 실행
        if (!GameManager.Instance.Asset.DeductAsset(target.itemPrice))
        {
            // 자산이 부족할 경우 여기서 false를 반환하고 종료
            return false;
        }

        // 3. 차감에 성공했을 경우에만 아래 로직 실행
        target.isPurchased = true;

        // 진행도 업데이트 및 콘텐츠 해금
        if (GameManager.Instance.Prog != null)
        {
            GameManager.Instance.Prog.UnlockContent(target.unlockContent);
        }

        Debug.Log($"{target} 구매 성공!");
        return true;

    }



}
