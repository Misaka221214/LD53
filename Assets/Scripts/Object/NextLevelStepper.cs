using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextLevelStepper : MonoBehaviour {
    private void OnTriggerStay2D(Collider2D collision) {
        if (collision.gameObject.CompareTag("Player")) {
            if (MetaData.LEVEL_TO_BE_LOADED != "") {
                SceneManager.LoadScene(MetaData.LEVEL_TO_BE_LOADED, LoadSceneMode.Single);
            }
        }
    }
}
