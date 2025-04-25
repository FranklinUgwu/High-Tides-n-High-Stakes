using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    public GameObject settingsCanvas;

    public void GoToScene(string sceneName) {
      SceneManager.LoadScene(sceneName);
    }

    public void GoToSettings() {
      settingsCanvas.SetActive(true);
    }

    public void QuitApp() {
      Application.Quit();
      Debug.Log("Application has quit.");
    }
}
