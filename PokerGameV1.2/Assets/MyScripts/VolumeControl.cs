using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class VolumeControl : MonoBehaviour
{

    public GameObject volumeController;
    public Slider slider;
    public AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
      volumeController.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter() {
      volumeController.SetActive(true);
    }

    void OnTriggerExit() {
      volumeController.SetActive(false);
    }

    public void changeVolume() {
      //Slider slider = volumeController.GetCompontent<Slider>();
      audioSource.volume = slider.value;
    }
}
