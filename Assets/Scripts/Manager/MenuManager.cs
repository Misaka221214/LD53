using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour {
    [SerializeField] private AudioSource click;

    public void GoToGameScene() {
        if (click) {
            click.Play();
        }
        SceneManager.LoadScene("Tutorial", LoadSceneMode.Single);
    }

    public void QuitGame() {
        Application.Quit();
    }
}
