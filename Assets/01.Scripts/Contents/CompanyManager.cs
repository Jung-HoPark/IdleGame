using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompanyManager : MonoBehaviour
{
    public long companyIncome;

    public float interval = 1f;
    public float timer = 0f;
    void Start()
    {
        
    }

    void Update()
    {
        if (!ProgressionManager.Instance.IsUnlocked(ContentType.Company1)) return;

        timer += Time.deltaTime;

        if (timer >= interval)
        {
            AddAsset();
            timer = 0f;
        }
    }

    public void AddAsset()
    {
        PlayerAssetManager.Instance.AddAsset(companyIncome);
    }


}
