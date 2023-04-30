using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewardManager : MonoBehaviour {
    public GameObject UpgradePanel;
    public GameObject SkillPanel;

    // Start is called before the first frame update
    void Start() {
        UpgradePanel.SetActive(true);
        SkillPanel.SetActive(MetaData.HAS_SKILL_REWARD);
    }

    // Update is called once per frame
    void Update() {

    }
}
