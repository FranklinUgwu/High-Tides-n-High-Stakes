using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class VolumeControl : MonoBehaviour
{

    public GameObject volumeController;
    public Slider musicSlider;
    public Slider soundEffectSlider;
    public AudioSource musicSource;
    public AudioSource soundEffectSource;

    // Start is called before the first frame update
    void Start()
    {
      volumeController.SetActive(false);
      musicSlider.value = PlayerPrefs.GetFloat("MusicVolume", 1f);
      soundEffectSlider.value = PlayerPrefs.GetFloat("SoundEffectVolume", 0.25f);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void changeMusicVolume() {
      if (musicSource != null) {
        musicSource.volume = musicSlider.value;
      }
      PlayerPrefs.SetFloat("MusicVolume", musicSlider.value);
      PlayerPrefs.Save();
    }

    public void changeSoundEffectVolume() {
      if (soundEffectSource != null) {
        soundEffectSource.volume = soundEffectSlider.value;
      }
      PlayerPrefs.SetFloat("SoundEffectVolume", soundEffectSlider.value);
      PlayerPrefs.Save();
    }

    public void disablePage() {
      volumeController.SetActive(false);
    }
}
