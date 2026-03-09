using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgressionManager : MonoBehaviour
{
    public static ProgressionManager Instance;

    public bool isCompany1Unlocked;
    public bool isOutsourceUnlocked;
    public bool isCompany2Unlocked;
    public bool isInHouseUnlocked;
    public bool isSelfDevelopmentUnlocked;


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

    public void UnlockContent(ContentType contents)
    {
        switch (contents)
        {
            case ContentType.Company1:
                isCompany1Unlocked = true;
                break;
            case ContentType.Outsource:
                isOutsourceUnlocked = true;
                break;
            case ContentType.Company2:
                isCompany2Unlocked = true;
                break;
            case ContentType.SelfDevelopment:
                isInHouseUnlocked = true;
                break;
        }
    }
    public bool IsUnlocked(ContentType content)
    {
        switch (content)
        {
            case ContentType.Company1: return isCompany1Unlocked;
            case ContentType.Outsource: return isOutsourceUnlocked;
            case ContentType.Company2: return isCompany2Unlocked;
            case ContentType.SelfDevelopment: return isSelfDevelopmentUnlocked;
        }
        return false;
    }

}
