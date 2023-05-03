using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewardManager : MonoBehaviour {
    public GameObject UpgradePanel;
    public GameObject SkillPanel;

    public GameObject[] upgrades;
    public GameObject[] playerSkills;
    public GameObject[] grabGunSkills;

    public Transform playerSkillLocation;
    public Transform grabGunSkillLocation;

    private GameObject playerSkill;
    private GameObject grabGunSkill;

    private bool disabledUpgrades = false;
    private bool disabledSkills = false;

    // Start is called before the first frame update
    void OnEnable() {
        foreach (GameObject go in GameObject.FindGameObjectsWithTag("Buff")) {
            Destroy(go);
        }
        if (MetaData.HAS_SKILL_REWARD) {
            SkillPanel.SetActive(true);
            playerSkill = Instantiate(playerSkills[Random.Range(0, playerSkills.Length)], playerSkillLocation.position, playerSkillLocation.rotation);
            grabGunSkill = Instantiate(grabGunSkills[Random.Range(0, grabGunSkills.Length)], grabGunSkillLocation.position, grabGunSkillLocation.rotation);
        }
    }

    // Update is called once per frame
    void Update() {
        if (!disabledUpgrades && MetaData.PICKED_UPGRADE) {
            disabledUpgrades = true;
            MetaData.PICKED_UPGRADE = false;
            foreach (GameObject go in upgrades) {
                if (go) {
                    UpgradePickup upgradePickup = go.GetComponent<UpgradePickup>();
                    if (upgradePickup) {
                        upgradePickup.GrayOut();
                    }
                }
            }
        }

        if (!disabledSkills && MetaData.PICKED_SKILL) {
            disabledSkills = true;
            MetaData.PICKED_SKILL = false;

            if (playerSkill) {
                playerSkill.GetComponent<PlayerSkillPickup>().GrayOut();
            }
            if (grabGunSkill) {
                grabGunSkill.GetComponent<GrabGunSkillPickup>().GrayOut();
            }
        }
    }
}
