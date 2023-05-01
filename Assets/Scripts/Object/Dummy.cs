using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dummy : MonoBehaviour {
    private float destroyCounter = 0f;
    private List<Enemy> enemies = new List<Enemy>();
    private GameObject player;

    private void Start() {
        destroyCounter = MetaData.DUMMY_COOLDOWN;
        player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void FixedUpdate() {
        destroyCounter -= Time.deltaTime;
        if (destroyCounter <= 0f) {
            foreach (Enemy e in enemies) {
                if (e) {
                    e.target = player;
                }
            }
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        Enemy enemy = collision.gameObject.GetComponent<Enemy>();
        if (enemy) {
            enemies.Add(enemy);
            enemy.target = gameObject;
        }
    }
}
