using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour {
    public void GoToGameScene() {
        SceneManager.LoadScene("Level1", LoadSceneMode.Single);
    }

    public void QuitGame() {
        Application.Quit();
    }
}
