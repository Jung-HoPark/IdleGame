using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgressionManager : MonoBehaviour
{
    public static ProgressionManager Instance;

    public bool isCompanyUnlocked;
    public bool isOutsourceUnlocked;
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

    

}
