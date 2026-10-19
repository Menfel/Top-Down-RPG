using UnityEngine;

public class TeleportManager : MonoBehaviour
{
    public static TeleportManager Instance;

    public bool JustTeleported { get; private set; }

    private void Awake()
    {
        // Singleton
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void MarkTeleport()
    {
        JustTeleported = true;
    }

    private void Update()
    {
        if (JustTeleported)
        {
            // Через 1 секунду после телепорта сбрасываем флаг
            Invoke(nameof(ResetTeleport), 1f);
        }
    }

    private void ResetTeleport()
    {
        JustTeleported = false;
    }


}
