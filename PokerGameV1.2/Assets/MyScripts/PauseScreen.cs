using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseScreen : MonoBehaviour
{

    public bool currentStatus = true;

    public GameObject pauseScreenUI;

    public void GoToScene(string sceneName) {
      SceneManager.LoadScene(sceneName);
    }

    public void ResumeGame() {
      currentStatus = false;
      gameObject.SetActive(currentStatus);
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

    void Resume() {
      pauseScreenUI.SetActive(false);
      currentStatus = false;
    }

    void Pause() {
      pauseScreenUI.SetActive(true);
      currentStatus = true;
    }
}
