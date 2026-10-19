using UnityEngine;
using UnityEngine.UI;

public class VolumeSliderHandler : MonoBehaviour
{
    public Slider volumeSlider;

    private void Start()
    {
        volumeSlider.onValueChanged.AddListener(OnSliderChanged);
        OnSliderChanged(volumeSlider.value); // Установим начальное значение
    }

    private void OnSliderChanged(float value)
    {
        SoundEffectManager.SetVolume(value);

        if (MusicManager.Instance != null)
        {
            MusicManager.Instance.SetVolumeMultiplier(value);
        }
    }
}
