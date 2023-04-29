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
    public SpriteRenderer sr;
    public Collider2D collider2D;

    private float health = 10f;
    private Vector2 direction = new Vector2(0, 0);
    private float takeDamageCounter = 0f;
    private Animator animator;

    void Start() {
        health = maxHealth;
        sr = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        collider2D = GetComponent<Collider2D>();
    }
    // Update is called once per frame
    void Update() {
        direction = (target.transform.position - transform.position).normalized;
    }

    void FixedUpdate() {
        if (rb.bodyType == RigidbodyType2D.Dynamic) {
            Move();
        }
        takeDamageCounter -= Time.deltaTime;

        if (sr.color == Color.red && takeDamageCounter < 0f) {
            sr.color = Color.white;
        }
    }

    void Move() {
        rb.velocity = direction * speed;
    }

    private void OnCollisionStay2D(Collision2D collision) {
        if (collision.gameObject.CompareTag("Parcel") && takeDamageCounter < 0f) {
            TakeDamage(collision.gameObject.GetComponent<Parcel>().GetDamage());
        }
    }

    public float GetDamage() {
        return damage;
    }

    private void TakeDamage(float damage) {
        takeDamageCounter = takeDamageCoolDown;
        health -= damage;

        if (health <= 0f) {
            rb.bodyType = RigidbodyType2D.Static;
            collider2D.enabled = false;
            sr.color = Color.white;
            animator.SetBool("isDead", true);
        } else {
            sr.color = Color.red;
        }
    }
}
