using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Numerics;

public class UpgradeSlot : MonoBehaviour
{
    [Header("UI 연결")]
    public Image iconImage;
    public TextMeshProUGUI descText;
    public TextMeshProUGUI currentText;
    public TextMeshProUGUI nextText;
    public TextMeshProUGUI levelText;
    public TextMeshProUGUI priceText;
    public Button purchaseButton;

    private UpgradeDataSO myData;
    private int currentLevel = 0; // 나중엔 매니저에서 가져올 변수

    public void Setup(UpgradeDataSO data)
    {
        myData = data;

        purchaseButton.onClick.RemoveAllListeners();
        purchaseButton.onClick.AddListener(OnClickPurchase);

        UpdateUI(); 

    }

    public void UpdateUI()
    {
        if (myData.icon != null && iconImage != null) iconImage.sprite = myData.icon;
        descText.text = myData.upgradeDesc;

        // GameManager에서 현재 내 레벨이 몇인지 물어봐서 가져옴!
        currentLevel = GameManager.Instance.Upgrade.GetUpgradeLevel(myData.upgradeID);

        BigInteger currentVal = myData.GetReward(currentLevel);
        BigInteger nextVal = myData.GetReward(currentLevel + 1);
        BigInteger currentPrice = myData.GetCost(currentLevel);

        switch (myData.type)
        {
            case UpgradeType.ClickPower:
            case UpgradeType.AutoIncome:
                currentText.text = "+" + currentVal.ToString();
                nextText.text = "+" + nextVal.ToString();
                break;
            case UpgradeType.GlobalMultiplier:
                currentText.text = currentVal.ToString() + "%";
                nextText.text = nextVal.ToString() + "%";
                break;
            case UpgradeType.SalaryReduction:
                currentText.text = "-" + currentVal.ToString();
                nextText.text = "-" + nextVal.ToString();
                break;
        }

        levelText.text = "Lv." + currentLevel.ToString();
        priceText.text = currentPrice.ToString();
    }
    private void OnClickPurchase()
    {
        bool success = GameManager.Instance.Upgrade.BuyUpgrade(myData.upgradeID);

        if (success)
        {
            UpdateUI();
        }
    }
}