using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawn : MonoBehaviour {
    public GameObject meleeEnemy;
    public GameObject rangeEnemy;
    public float respawnCooldown = 10f;
    public int spawnPerWave = 3;

    private float timer = 0f;

    // Update is called once per frame
    void FixedUpdate() {
        timer -= Time.deltaTime;

        if (timer <= 0f) {
            timer = respawnCooldown;

            for (int i = 0; i < spawnPerWave; i++) {
                if(Random.Range(0, 10) < 6) {
                    Instantiate(meleeEnemy, RandomPosition(), transform.rotation);
                } else {
                    Instantiate(rangeEnemy, RandomPosition(), transform.rotation);
                }
            }
        }
    }

    Vector3 RandomPosition() {
        return new Vector3(transform.position.x + Random.Range(-0.1f, 0.1f), transform.position.y + Random.Range(-0.1f, 0.1f), transform.position.z);
    }
}
