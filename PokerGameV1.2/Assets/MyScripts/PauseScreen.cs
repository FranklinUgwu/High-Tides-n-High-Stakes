using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseScreen : MonoBehaviour
{

    public bool currentStatus = true;

    public GameObject pauseScreenUI;
    public GameObject settingsCanvas;
    public GameObject hudScreenUI;

    public void GoToScene(string sceneName) {
      SceneManager.LoadScene(sceneName);
    }

    public void GoToSettings() {
      settingsCanvas.SetActive(true);
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetButtonDown("Cancel")) {
          if (currentStatus) {
            Resume();
          } else {
            Pause();
          }
        }
    }

    public void Resume() {
      pauseScreenUI.SetActive(false);
      hudScreenUI.SetActive(true);
      currentStatus = false;
    }

    void Pause() {
      pauseScreenUI.SetActive(true);
      hudScreenUI.SetActive(false);
      currentStatus = true;
    }
}
