using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SoundEffectManager : MonoBehaviour
{
    private static SoundEffectManager Instance;

    private static SoundEffectLibrary soundEffectLibrary;
    private static AudioSource audioSource;
    private static AudioSource voiceAudioSource;
    private static AudioSource randomPitchAudioSource;
    [SerializeField] private Slider sfxSlider;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            AudioSource[] audioSources = GetComponents<AudioSource>();
            audioSource = audioSources[0];
            randomPitchAudioSource = audioSources[1];
            voiceAudioSource = audioSources[2];
            soundEffectLibrary = GetComponent<SoundEffectLibrary>();
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }


    public static void Play(string soundName, bool randomPitch = false)
    {
        AudioClip audioClip = soundEffectLibrary.GEtRandomClip(soundName);
        if (audioClip != null)
        {
            if (randomPitch)
            {
                randomPitchAudioSource.pitch = Random.Range(1f, 1.5f);
                randomPitchAudioSource.PlayOneShot(audioClip);
            }
            else
            {
                audioSource.PlayOneShot(audioClip);
            }
        }
    }

    //public static void PlayVoice(AudioClip audioClip, float pitch = 1f)
    //{
    //    voiceAudioSource.pitch = pitch;
    //    voiceAudioSource.PlayOneShot(audioClip);
    //}

    public static void PlayVoice(AudioClip audioClip, float pitch = 1f)
    {
        if (audioClip == null)
        {
            Debug.LogWarning("AudioClip is null!");
            return;
        }

        voiceAudioSource.pitch = pitch;
        voiceAudioSource.PlayOneShot(audioClip);
    }



    void Start()
    {
        sfxSlider.onValueChanged.AddListener(delegate { OnValueChanged(); });
    }

    public static void SetVolume(float volume)
    {
        audioSource.volume = volume;
        randomPitchAudioSource.volume = volume;
        voiceAudioSource.volume = volume;
    }

    public static void OnValueChanged()
    {
        //SetVolume(Instance.sfxSlider.value);
        SetVolume(Instance.sfxSlider.value);
    }
}

