using UnityEngine;

public class PauseController : MonoBehaviour
{
    public static bool IsGamePaused { get; private set; } = false;

    private static PauseController instance;

    private void Awake()
    {
        // Singleton, чтобы не было дубликатов
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject); // сохраняется между сценами
    }

    public static void SetPause(bool pause)
    {
        IsGamePaused = pause;
        Time.timeScale = pause ? 0f : 1f;
    }
}
