using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ScreenClicker : MonoBehaviour, IPointerDownHandler
{
    public void OnPointerDown(PointerEventData eventData)
    {
        GameManager.Instance.Asset.AddAsset(GameManager.Instance.Asset.GoldPerClick);

        FloatingCoin newCoin = GameManager.Instance.Pool.GetFloatingCoin();

        RectTransform coinRect = newCoin.GetComponent<RectTransform>();
        Vector2 localPos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            newCoin.transform.parent as RectTransform,
            eventData.position,
            eventData.pressEventCamera,
            out localPos);

        coinRect.anchoredPosition = localPos;

        newCoin.AnimateUpward().Forget();
    }
}
