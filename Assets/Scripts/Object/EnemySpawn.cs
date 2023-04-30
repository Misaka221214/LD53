using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawn : MonoBehaviour {
    public GameObject enemy;
    public float respawnCooldown = 10f;
    public int spawnPerWave = 3;

    private float timer = 0f;
    private bool isStarted = false;

    // Update is called once per frame
    void FixedUpdate() {
        timer -= Time.deltaTime;

        if (timer <= 0f && MetaData.SPAWN_ENEMY) {
            timer = respawnCooldown;

            if (isStarted) {
                for (int i = 0; i < spawnPerWave; i++) {
                    Instantiate(enemy, RandomPosition(), transform.rotation);
                }
            } else {
                isStarted = true;
            }
        }
    }

    Vector3 RandomPosition() {
        return new Vector3(transform.position.x + Random.Range(-0.1f, 0.1f), transform.position.y + Random.Range(-0.1f, 0.1f), transform.position.z);
    }
}
