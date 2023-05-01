using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabGunSkillPickup : MonoBehaviour {
    public GrabGunSkill skillType;

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.collider.gameObject.CompareTag("Player")) {
            MetaData.GRAB_GUN_SKILL = skillType;
            MetaData.PICKED_SKILL = true;
            Destroy(gameObject);
        }
    }

    public void GrayOut() {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        Collider2D collider2d = GetComponent<Collider2D>();

        sr.color = Color.black;
        collider2d.enabled = false;
    }
}
