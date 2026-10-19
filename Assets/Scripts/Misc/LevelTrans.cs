using UnityEngine;
using UnityEngine.SceneManagement;


public class LevelTrans : MonoBehaviour
{
    [Header("UI Panels")]
    public GameObject mainPanelUI;     
    public GameObject settingsPanelUI; 

      public void OnPlayButtonPressed()
    {
        if (SceneFader.Instance != null)
        {
            SceneFader.Instance.FadeToScene("LoadingScene");
        }
        else
        {
            Debug.LogError("SceneFader.Instance не найден!");
            SceneManager.LoadScene("LoadingScene");
        }
    }


    public void ExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
    public void OpenSettings()
    {
        mainPanelUI.SetActive(false);
        settingsPanelUI.SetActive(true);
    }

    public void CloseSettings()
    {
        settingsPanelUI.SetActive(false);
        mainPanelUI.SetActive(true);
    }

public void OnBackButtonPressed()
{
    PlayerPrefs.Save();

    SceneManager.LoadScene("MainPanel");
    if (SceneManager.GetSceneByName("SettingScene").isLoaded)
    {
        SceneManager.UnloadSceneAsync("SettingScene");
    }
}

}
