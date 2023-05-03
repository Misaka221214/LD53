using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextLevelStepper : MonoBehaviour {
    [SerializeField] AudioSource hole;

    private void OnTriggerStay2D(Collider2D collision) {
        if (collision.gameObject.CompareTag("Player")) {
            if (MetaData.LEVEL_TO_BE_LOADED != "") {
                hole.Play();
                SceneManager.LoadScene(MetaData.LEVEL_TO_BE_LOADED);
            }
        }
    }
}
