using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour {
    public int deliveredParcel = 0;
    public int level;
    public GameObject[] patterns;

    private float timer = 99f;

    // Start is called before the first frame update
    void Start() {
        SetTimer();
        SetNextLevel();

        int draw = Random.Range(0, patterns.Length);
        patterns[draw].SetActive(true);
        foreach (Transform child in patterns[draw].transform) {
            child.gameObject.SetActive(true);
        }
    }

    // Update is called once per frame
    void FixedUpdate() {
        timer -= Time.deltaTime;
        if (DeliveredAllParcel()) {
            MetaData.HAS_SKILL_REWARD = timer > 0f;
            SceneManager.LoadScene("CollectReward", LoadSceneMode.Single);
        }
    }

    private void OnTriggerStay2D(Collider2D collision) {
        GameObject go = collision.gameObject;
        Parcel parcel = go.transform.GetComponent<Parcel>();

        if (parcel != null && !parcel.isPickedUp) {
            deliveredParcel++;
            Destroy(go);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        GameObject go = collision.gameObject;
        Parcel parcel = go.transform.GetComponent<Parcel>();

        if (parcel != null && !parcel.isPickedUp) {
            deliveredParcel++;
            Destroy(go);
        }
    }


    private void SetTimer() {
        switch (level) {
            case 1:
                timer = MetaData.LEVEL_1_MAX_TIME;
                break;
            case 2:
                timer = MetaData.LEVEL_2_MAX_TIME;
                break;
            case 3:
                timer = MetaData.LEVEL_3_MAX_TIME;
                break;
            default:
                break;
        }
    }

    private void SetNextLevel() {
        switch (level) {
            case 1:
                MetaData.LEVEL_TO_BE_LOADED = "Level2";
                break;
            case 2:
                MetaData.LEVEL_TO_BE_LOADED = "Level3";
                break;
            case 3:
                MetaData.LEVEL_TO_BE_LOADED = "Score";
                break;
            default:
                break;
        }
    }

    private bool DeliveredAllParcel() {
        //switch (level) {
        //    case 1:
        //        return deliveredParcel >= MetaData.LEVEL_1_MAX_PARCEL;
        //    case 2:
        //        return deliveredParcel >= MetaData.LEVEL_2_MAX_PARCEL;
        //    case 3:
        //        return deliveredParcel >= MetaData.LEVEL_3_MAX_PARCEL;
        //    default:
        //        return false;
        //}
        return FindObjectsOfType(typeof(Parcel)).Length <= 0;
    }
}
