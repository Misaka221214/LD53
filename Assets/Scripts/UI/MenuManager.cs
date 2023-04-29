using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour {
    public void GoToGameScene() {
        SceneManager.LoadScene("Game", LoadSceneMode.Single);
    }

    public void QuitGame() {
        Application.Quit();
    }
}
