using UnityEngine;

public class PageManager : MonoBehaviour
{
    [SerializeField] private GameObject settingsPage;
    [SerializeField] private GameObject playerInfoPage;

    public void OpenPlayerInfo()
    {
        settingsPage.SetActive(false);
        playerInfoPage.SetActive(true);
    }

    public void BackToSettings()
    {
        playerInfoPage.SetActive(false);
        settingsPage.SetActive(true);
    }
}
