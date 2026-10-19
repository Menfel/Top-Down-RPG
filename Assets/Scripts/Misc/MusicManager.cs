//using UnityEngine;
//using UnityEngine.SceneManagement;
//using UnityEngine.UI;

//public class MusicManager : MonoBehaviour
//{
//    public static MusicManager Instance;

//    [Header("Музыка по сценам")]
//    public AudioClip mainMenuMusic;
//    public AudioClip gameSceneMusic;
//    public AudioClip game2SceneMusic;

//    [Header("Настройки")]
//    public Slider musicSlider;
//    public float gameplayVolume = 0.3f; // громкость в игровых сценах
//    public float menuVolume = 0.8f;     // громкость в меню

//    private AudioSource musicSource;
//    private string currentScene;

//    private void Awake()
//    {
//        if (Instance == null)
//        {
//            Instance = this;
//            DontDestroyOnLoad(gameObject);

//            musicSource = GetComponent<AudioSource>();
//            musicSource.loop = true;
//            musicSource.playOnAwake = false;

//            SceneManager.sceneLoaded += OnSceneLoaded;
//        }
//        else
//        {
//            Destroy(gameObject);
//        }
//    }

//    private void Start()
//    {
//        if (musicSlider != null)
//        {
//            musicSlider.onValueChanged.AddListener(SetMusicVolume);
//            SetMusicVolume(musicSlider.value);
//        }

//        currentScene = SceneManager.GetActiveScene().name;
//        PlayMusicForScene(currentScene);
//    }

//    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
//    {
//        currentScene = scene.name;
//        PlayMusicForScene(currentScene);
//    }

//    private void PlayMusicForScene(string sceneName)
//    {
//        AudioClip clipToPlay = null;
//        float volume = 1f;

//        if (sceneName == "MainPanel" || sceneName == "SettingScene")
//        {
//            clipToPlay = mainMenuMusic;
//            volume = menuVolume;
//        }
//        else if (sceneName == "GameScene")
//        {
//            clipToPlay = gameSceneMusic;
//            volume = gameplayVolume;
//        }
//        else if (sceneName == "Game2Scene")
//        {
//            clipToPlay = game2SceneMusic;
//            volume = gameplayVolume;
//        }

//        if (clipToPlay == null) return;

//        // Проверка, уже ли играет нужная музыка
//        if (musicSource.clip == clipToPlay && musicSource.isPlaying)
//        {
//            // Только обновим громкость
//            musicSource.volume = volume * (musicSlider != null ? musicSlider.value : 1f);
//            return;
//        }

//        // Воспроизводим новый трек
//        musicSource.clip = clipToPlay;
//        musicSource.volume = volume * (musicSlider != null ? musicSlider.value : 1f);
//        musicSource.Play();
//    }


//    public void SetMusicVolume(float sliderValue)
//    {
//        float baseVolume = (currentScene == "MainPanel" || currentScene == "SettingScene") ? menuVolume : gameplayVolume;
//        musicSource.volume = baseVolume * sliderValue;
//    }

//    public class VolumeSliderHandler : MonoBehaviour
//    {
//        public Slider volumeSlider;

//        private void Start()
//        {
//            volumeSlider.onValueChanged.AddListener(OnVolumeChanged);
//        }

//        private void OnVolumeChanged(float value)
//        {
//            SoundEffectManager.SetVolume(value);
//            if (MusicManager.Instance != null)
//            {
//                MusicManager.Instance.SetMusicVolume(value);
//            }
//        }
//    }

//}

using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance;

    [Header("Музыка по сценам")]
    public AudioClip mainMenuMusic;
    public AudioClip gameSceneMusic;
    public AudioClip game2SceneMusic;

    [Header("Настройки")]
    public float gameplayVolume = 0.8f; // Базовая громкость для игровых сцен
    public float menuVolume = 0.8f;     // Базовая громкость для меню

    private AudioSource musicSource;
    private string currentScene;
    private float volumeMultiplier = 1f;


    public AudioMixer audioMixer; // Привяжи через инспектор

    public void SetMusicVolume(float volume) // volume от 0.0001 до 1
    {
        float dB = Mathf.Log10(volume) * 20;
        audioMixer.SetFloat("MusicVolume", dB);
    }

    private void Awake()
    {
        Debug.Log("[MusicManager] Awake вызван");

        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            musicSource = GetComponent<AudioSource>();
            if (musicSource == null)
            {
                Debug.LogError("[MusicManager] AudioSource не найден на объекте!");
            }

            musicSource.loop = true;
            musicSource.playOnAwake = false;

            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Debug.Log("[MusicManager] Второй экземпляр найден и уничтожен");
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        currentScene = SceneManager.GetActiveScene().name;
        Debug.Log($"[MusicManager] Start -> Текущая сцена: {currentScene}");
        PlayMusicForScene(currentScene);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        currentScene = scene.name;
        Debug.Log($"[MusicManager] Сцена загружена: {currentScene}");
        PlayMusicForScene(currentScene);
    }


    private void PlayMusicForScene(string sceneName)
    {
        Debug.Log("[MusicManager] Выбор музыки для сцены: " + sceneName);

        AudioClip clipToPlay = null;
        float baseVolume = 1f;

        if (sceneName == "MainPanel" || sceneName == "SettingScene")
        {
            clipToPlay = mainMenuMusic;
            baseVolume = menuVolume;
        }
        else if (sceneName == "GameScene")
        {
            clipToPlay = gameSceneMusic;
            baseVolume = gameplayVolume;
        }
        else if (sceneName == "Game2Scene")
        {
            clipToPlay = game2SceneMusic;
            baseVolume = gameplayVolume;
        }

        if (clipToPlay == null)
        {
            Debug.LogWarning("[MusicManager] Нет музыки для сцены: " + sceneName);
            return;
        }

        if (musicSource == null)
        {
            Debug.LogError("[MusicManager] AudioSource не найден!");
            return;
        }

        if (musicSource.clip == clipToPlay && musicSource.isPlaying)
        {
            musicSource.volume = baseVolume * volumeMultiplier;
            Debug.Log("[MusicManager] Музыка уже играет, только обновили громкость.");
            return;
        }

        musicSource.clip = clipToPlay;
        musicSource.volume = baseVolume * volumeMultiplier;
        musicSource.Play();

        Debug.Log("[MusicManager] Проигрываем новый трек для " + sceneName + ": " + clipToPlay.name);
    }


    public void SetVolumeMultiplier(float multiplier)
    {
        volumeMultiplier = multiplier;
        Debug.Log($"[MusicManager] Установлен множитель громкости: {volumeMultiplier}");
        PlayMusicForScene(currentScene); // Обновим громкость
    }
}
