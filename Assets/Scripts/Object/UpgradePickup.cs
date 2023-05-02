using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradePickup : MonoBehaviour {
    public UpgradeType upgradeType;

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.collider.gameObject.CompareTag("Player")) {
            collision.collider.gameObject.GetComponent<Player>().PlayGoodThing();
            switch (upgradeType) {
                case UpgradeType.SPEED:
                    MetaData.BASE_SPEED *= 1.4f;
                    break;
                case UpgradeType.DAMAGE:
                    MetaData.DAMAGE_MULTIPLIER += 3;
                    break;
                case UpgradeType.COOLDOWN:
                    MetaData.SKILL_COOLDOWN -= 1;
                    break;
                default:
                    break;
            }
            MetaData.PICKED_UPGRADE = true;
            GameObject.Find("Player").GetComponent<Player>().UpdateUpgrades(upgradeType);
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
