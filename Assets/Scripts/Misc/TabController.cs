using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;


public class TabController : MonoBehaviour
{
    public Image[] escapeImages; 
    public GameObject[] pages;

    void Start()
    {
        ActiveEscape(0);
    }
    public void ActiveEscape(int escapeNo)
    {
        for (int i = 0; i < pages.Length; i++)
        {
            pages[i].SetActive(false);
            escapeImages[i].color= Color.grey;
        }
        pages[escapeNo].SetActive(true);
        escapeImages[escapeNo].color = Color.white;
    }
}
