//using System.Collections;
//using TMPro;
//using UnityEngine;
//using UnityEngine.SceneManagement;

//public class LoadingManager : MonoBehaviour
//{
//    [Header("UI")]
//    public TMP_Text loadingText;

//    [Header("Сообщения загрузки")]
//    [TextArea(2, 5)]
//    public string[] messages;

//    [Header("Настройки текста")]
//    public float typingSpeed = 0.05f;
//    public float delayBetweenMessages = 1.2f;
//    public float startDelayBeforeText = 1f;

//    [Header("Звук")]
//    public AudioClip letterSound;
//    public float letterSoundPitch = 1f;
//    private AudioSource audioSource;

//    private int currentMessageIndex = 0;
//    private bool isTyping = false;
//    private Coroutine typingCoroutine;

//    void Start()
//    {
//        audioSource = GetComponent<AudioSource>();
//        if (audioSource == null)
//        {
//            audioSource = gameObject.AddComponent<AudioSource>();
//        }

//        StartCoroutine(ShowMessagesAndLoadScene());
//    }

//    void Update()
//    {
//        if (Input.GetKeyDown(KeyCode.E))
//        {
//            if (isTyping)
//            {
//                // Показываем весь текст сразу
//                if (typingCoroutine != null)
//                    StopCoroutine(typingCoroutine);

//                loadingText.text = messages[currentMessageIndex];
//                isTyping = false;
//            }
//            else
//            {
//                currentMessageIndex++;
//            }
//        }
//    }

//    IEnumerator ShowMessagesAndLoadScene()
//    {
//        yield return new WaitForSeconds(startDelayBeforeText); // Задержка перед началом

//        while (currentMessageIndex < messages.Length)
//        {
//            typingCoroutine = StartCoroutine(TypeSentence(messages[currentMessageIndex]));

//            while (isTyping)
//                yield return null;

//            float timer = 0f;
//            while (!Input.GetKeyDown(KeyCode.E) && timer < delayBetweenMessages)
//            {
//                timer += Time.deltaTime;
//                yield return null;
//            }

//            currentMessageIndex++;
//        }

//        // Загрузка сцены
//        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("GameScene");
//        asyncLoad.allowSceneActivation = false;

//        while (asyncLoad.progress < 0.9f)
//            yield return null;

//        asyncLoad.allowSceneActivation = true;
//    }

//    IEnumerator TypeSentence(string sentence)
//    {
//        isTyping = true;
//        loadingText.text = "";

//        foreach (char letter in sentence)
//        {
//            loadingText.text += letter;

//            if (letterSound != null)
//            {
//                audioSource.pitch = letterSoundPitch;
//                audioSource.PlayOneShot(letterSound);
//            }

//            yield return new WaitForSeconds(typingSpeed);

//            if (!isTyping) yield break; // если прервали печать (по E)
//        }

//        isTyping = false;
//    }
//}









using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingManager : MonoBehaviour
{
    [Header("UI")]
    public TMP_Text loadingText;

    [Header("Сообщения загрузки")]
    [TextArea(2, 5)]
    public string[] messages;

    [Header("Настройки текста")]
    public float typingSpeed = 0.05f;
    public float delayBetweenMessages = 1.2f;
    public float startDelayBeforeText = 1f;

    [Header("Звук")]
    public AudioClip letterSound;
    public float letterSoundPitch = 1f;
    private AudioSource audioSource;

    private int currentMessageIndex = 0;
    private bool isTyping = false;
    private Coroutine typingCoroutine;

    private bool skipText = false;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();

        StartCoroutine(ShowMessagesAndLoadScene());
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (isTyping)
            {
                // Показываем весь текст сразу
                if (typingCoroutine != null)
                    StopCoroutine(typingCoroutine);

                loadingText.text = messages[currentMessageIndex];
                isTyping = false;
            }
            else
            {
                currentMessageIndex++;
                skipText = true; // перейти к следующему сообщению
            }
        }
    }

    IEnumerator ShowMessagesAndLoadScene()
    {
        yield return new WaitForSeconds(startDelayBeforeText); // Задержка перед началом

        while (currentMessageIndex < messages.Length)
        {
            skipText = false;
            typingCoroutine = StartCoroutine(TypeSentence(messages[currentMessageIndex]));

            while (isTyping)
                yield return null;

            float timer = 0f;
            while (!skipText && timer < delayBetweenMessages)
            {
                timer += Time.deltaTime;
                yield return null;
            }

            currentMessageIndex++;
        }

        // После текста — затемнение и переход
        yield return StartCoroutine(FadeAndLoadNextScene("GameScene"));
    }

    IEnumerator TypeSentence(string sentence)
    {
        isTyping = true;
        loadingText.text = "";

        foreach (char letter in sentence)
        {
            loadingText.text += letter;

            if (letterSound != null)
            {
                audioSource.pitch = letterSoundPitch;
                audioSource.PlayOneShot(letterSound);
            }

            yield return new WaitForSeconds(typingSpeed);

            if (!isTyping) yield break;
        }

        isTyping = false;
    }

    IEnumerator FadeAndLoadNextScene(string sceneName)
    {
        if (SceneFader.Instance != null)
        {
            SceneFader.Instance.FadeToScene(sceneName);
            yield break; // сцена загрузится сама после затемнения
        }
        else
        {
            // fallback — если SceneFader не найден
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
            asyncLoad.allowSceneActivation = false;

            while (asyncLoad.progress < 0.9f)
                yield return null;

            asyncLoad.allowSceneActivation = true;
        }
    }
}
