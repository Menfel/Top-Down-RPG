using UnityEngine;

public class MenuController : MonoBehaviour
{
    public static MenuController Instance;

    public GameObject menuCanvas;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        menuCanvas.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!menuCanvas.activeSelf && PauseController.IsGamePaused)
            {
                return;
            }

            menuCanvas.SetActive(!menuCanvas.activeSelf);
            PauseController.SetPause(menuCanvas.activeSelf);
        }
    }

    public bool IsMenuOpen()
    {
        return menuCanvas != null && menuCanvas.activeSelf;
    }
}
