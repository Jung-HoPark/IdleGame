using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TabController : MonoBehaviour
{
    public GameObject[] panels;

    public void SwitchTab(int tabIndex)
    {
        if (tabIndex < 0 || tabIndex >= panels.Length) return;
        
       HideAllPanels(); 

        panels[tabIndex].SetActive(true);
    }

    public void HideAllPanels()
    {
        for (int i = 0; i < panels.Length; i++)
        {
            panels[i].SetActive(false);
        }
    }
}
