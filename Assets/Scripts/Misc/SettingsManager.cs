using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SettingsManager : MonoBehaviour
{
    public static SettingsManager Instance;

    [Header("UI Elements")]
    public Toggle fullscreenToggle;

    //private void Awake()
    //{
    //    // Singleton
    //    if (Instance == null)
    //    {
    //        Instance = this;
    //        DontDestroyOnLoad(gameObject); // Сохраняем между сценами
    //    }
    //    else
    //    {
    //        Destroy(gameObject);
    //        return;
    //    }

    //    LoadSettings();
    //}

    void Start()
    {
        // Только при запуске — окно 1280x720, 16:9
        if (!Screen.fullScreen)
        {
            Screen.SetResolution(1280, 720, FullScreenMode.Windowed);
        }

        // Применяем текущие настройки
        LoadSettings();
    }

    public void SetFullscreen(bool isFullscreen)
    {
        Debug.Log("[SettingsManager] SetFullscreen: " + isFullscreen);

        if (isFullscreen)
        {
            Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
            Screen.fullScreen = true;
        }
        else
        {
            // Автоопределение подходящего 16:9 окна
            Resolution bestResolution = GetMax16by9Resolution(Screen.currentResolution.width, Screen.currentResolution.height);
            Screen.SetResolution(bestResolution.width, bestResolution.height, FullScreenMode.Windowed);
        }

        PlayerPrefs.SetInt("Fullscreen", isFullscreen ? 1 : 0);
    }

    
    public void LoadSettings()
    {
        bool isFullscreen = PlayerPrefs.GetInt("Fullscreen", 0) == 1; // 0 — окно по умолчанию

        if (isFullscreen)
        {
            Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
            Screen.fullScreen = true;
        }
        else
        {
            Screen.SetResolution(1280, 720, FullScreenMode.Windowed);
        }

        if (fullscreenToggle != null)
            fullscreenToggle.isOn = isFullscreen;
    }

    private Resolution GetMax16by9Resolution(int screenWidth, int screenHeight)
    {
        // Массив популярных 16:9 разрешений
        Resolution[] options = new Resolution[]
        {
        new Resolution { width = 1920, height = 1080 },
        new Resolution { width = 1600, height = 900 },
        new Resolution { width = 1366, height = 768 },
        new Resolution { width = 1280, height = 720 },
        new Resolution { width = 1024, height = 576 },
        };

        foreach (Resolution res in options)
        {
            if (res.width <= screenWidth && res.height <= screenHeight)
                return res;
        }

        // Если ничего не подошло — fallback
        return new Resolution { width = 1280, height = 720 };
    }

    public struct Resolution
    {
        public int width;
        public int height;
        public int refreshRate;
    }

}
