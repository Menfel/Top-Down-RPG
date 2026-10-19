using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneFader : MonoBehaviour
{
    public static SceneFader Instance;

    [SerializeField] private Canvas fadeCanvas;
    [SerializeField] private Image fadeImage;
    [SerializeField] private float fadeSpeed = 1f;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    public void FadeToScene(string sceneName)
    {
        StartCoroutine(FadeAndLoadScene(sceneName));
    }

    private IEnumerator FadeAndLoadScene(string sceneName)
    {
        // Включаем канвас
        fadeCanvas.gameObject.SetActive(true);

        // Включаем блокировку кликов
        fadeImage.raycastTarget = true;

        // Затемнение
        Color color = fadeImage.color;
        color.a = 0f;
        fadeImage.color = color;

        while (color.a < 1f)
        {
            color.a += fadeSpeed * Time.deltaTime;
            fadeImage.color = color;
            yield return null;
        }

        // Загружаем сцену
        SceneManager.LoadScene(sceneName);

        // Подписываемся на событие окончания загрузки
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Отключаем затемнение
        StartCoroutine(FadeFromBlack());

        // Отписываемся от события
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private IEnumerator FadeFromBlack()
    {
        Color color = fadeImage.color;
        color.a = 1f;
        fadeImage.color = color;

        while (color.a > 0f)
        {
            color.a -= fadeSpeed * Time.deltaTime;
            fadeImage.color = color;
            yield return null;
        }

        color.a = 0f;
        fadeImage.color = color;

        // Отключаем блокировку кликов
        fadeImage.raycastTarget = false;

        // Скрываем FadeCanvas полностью
        fadeCanvas.gameObject.SetActive(false);
    }
}
