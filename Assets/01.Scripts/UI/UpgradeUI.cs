using System.Collections.Generic;
using UnityEngine;

public class UpgradeUI : MonoBehaviour
{
    [Header("UI 연결")]
    public Transform contentTransform; 
    public GameObject slotPrefab;     

    [Header("업그레이드 데이터 리스트")]
    public List<UpgradeDataSO> upgradeDatas; 

    private void Start()
    {
        GenerateSlots();
    }

    private void GenerateSlots()
    {
        foreach (Transform child in contentTransform)
        {
            Destroy(child.gameObject);
        }

        foreach (UpgradeDataSO data in upgradeDatas)
        {
            GameObject newSlot = Instantiate(slotPrefab, contentTransform);

            UpgradeSlot slotScript = newSlot.GetComponent<UpgradeSlot>();

            if (slotScript != null)
            {
                slotScript.Setup(data);
            }
        }
    }
}