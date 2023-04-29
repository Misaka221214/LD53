using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {
    private float damage = 2f;

    private float maxHealth = 10f;
    private float speed = 4f;
    private float takeDamageCoolDown = 0.5f;

    public GameObject target;
    public Rigidbody2D rb;

    private float health = 10f;
    private Vector2 direction = new Vector2(0, 0);
    private float takeDamageCounter = 0f;

    void Start() {
        health = maxHealth;
    }
    // Update is called once per frame
    void Update() {
        direction = (target.transform.position - transform.position).normalized;
    }

    void FixedUpdate() {
        Move();
        takeDamageCounter -= Time.deltaTime;

        if(health <= 0f) {
            Destroy(gameObject);
        }
    }

    void Move() {
        rb.velocity = direction * speed;
    }

    private void OnCollisionStay2D(Collision2D collision) {
        if (collision.gameObject.CompareTag("Parcel") && takeDamageCounter < 0f) {
            takeDamageCounter = takeDamageCoolDown;
            health -= collision.gameObject.GetComponent<Parcel>().GetDamage();
        }
    }

    public float GetDamage() {
        return damage;
    }
}
