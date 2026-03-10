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

        if (myData.icon != null && iconImage != null)
        {
            iconImage.sprite = myData.icon;
        }

        descText.text = myData.upgradeDesc;

        // SO의 수학 공식 함수에 현재 레벨을 던져서 BigInteger 값 받아오기!
        BigInteger currentVal = myData.GetReward(currentLevel);
        BigInteger nextVal = myData.GetReward(currentLevel + 1);
        BigInteger currentPrice = myData.GetCost(currentLevel);

        // 타입에 맞춰서 기호(+, %, -) 붙여서 출력하기
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

        // 레벨과 가격 표시 (BigInteger는 ToString()으로 출력)
        levelText.text = "Lv." + currentLevel.ToString();
        priceText.text = currentPrice.ToString();
    }
}