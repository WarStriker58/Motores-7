using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    public AudioMixer audioMixer;
    public Scrollbar masterVolumeScrollbar;
    public Scrollbar musicVolumeScrollbar;
    public Scrollbar sfxVolumeScrollbar;

    public void SetMasterVolume()
    {
        float volume = Mathf.Log10(masterVolumeScrollbar.value) * 20;
        audioMixer.SetFloat("MasterVolume", volume);
    }

    public void SetMusicVolume()
    {
        float volume = Mathf.Log10(musicVolumeScrollbar.value) * 20;
        audioMixer.SetFloat("MusicVolume", volume);
    }

    public void SetSFXVolume()
    {
        float volume = Mathf.Log10(sfxVolumeScrollbar.value) * 20;
        audioMixer.SetFloat("SFXVolume", volume);
    }
}