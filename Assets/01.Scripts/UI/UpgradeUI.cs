using System.Collections.Generic;
using UnityEngine;

public class UpgradeUI : MonoBehaviour
{
    [Header("UI ¿¬°á")]
    public Transform contentTransform; 
    public GameObject slotPrefab;

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

        List<UpgradeDataSO> managerUpgrades = GameManager.Instance.Upgrade.upgrades;

        foreach (var data in managerUpgrades)
        {
            GameObject newSlot = Instantiate(slotPrefab, contentTransform);

            var slotScript = newSlot.GetComponent<UpgradeSlot>();

            if (slotScript != null)
            {
                slotScript.Setup(data);
            }
        }
    }
}