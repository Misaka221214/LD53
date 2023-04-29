using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
    private float maxStamina = 2f;

    private float takeDamageCoolDown = 1f;
    private float recoverCoolDown = 1f;
    private float speed = 5f;

    public Rigidbody2D rb;
    public SpriteRenderer sr;
    public float stamina = 2f;

    private Vector2 direction = new Vector2(0, 0);
    private float takeDamageCounter = 0f;
    private float recoverCounter = 0f;
    private bool isRecovering = false;

    // Start is called before the first frame update
    void Start() {
        stamina = maxStamina;
    }

    // Update is called once per frame
    void Update() {
        if (recoverCounter < 0f) {
            if (rb.bodyType == RigidbodyType2D.Static) {
                takeDamageCounter = takeDamageCoolDown;
                rb.bodyType = RigidbodyType2D.Dynamic;
                sr.color = Color.white;
            }
            isRecovering = false;
            if (Input.GetKey(KeyCode.W)) {
                direction = new Vector2(direction.x, 1).normalized;
            }

            if (Input.GetKey(KeyCode.A)) {
                direction = new Vector2(-1, direction.y).normalized;
                Flip(true);
            }

            if (Input.GetKey(KeyCode.S)) {
                direction = new Vector2(direction.x, -1).normalized;
            }

            if (Input.GetKey(KeyCode.D)) {
                direction = new Vector2(1, direction.y).normalized;
                Flip(false);
            }
            if (Input.GetKeyUp(KeyCode.W)) {
                direction = new Vector2(direction.x, 0).normalized;
            }

            if (Input.GetKeyUp(KeyCode.A)) {
                direction = new Vector2(0, direction.y).normalized;
            }

            if (Input.GetKeyUp(KeyCode.S)) {
                direction = new Vector2(direction.x, 0).normalized;
            }

            if (Input.GetKeyUp(KeyCode.D)) {
                direction = new Vector2(0, direction.y).normalized;
            }
        }
    }

    void FixedUpdate() {
        if (!isRecovering) {
            Move();
        }
        takeDamageCounter -= Time.deltaTime;
        recoverCounter -= Time.deltaTime;

        if (stamina <= 0f) {
            Recover();
        }
    }

    void Move() {
        rb.velocity = direction * speed;
    }

    void Flip(bool left) {
        Quaternion rotation = transform.rotation;
        if (left) {
            transform.rotation = new Quaternion(rotation.x, 0, rotation.z, rotation.w);
        } else {
            transform.rotation = new Quaternion(rotation.x, 180, rotation.z, rotation.w);
        }
    }

    private void OnCollisionStay2D(Collision2D collision) {
        if (collision.gameObject.CompareTag("Enemy") && !isRecovering && takeDamageCounter < 0f) {
            takeDamageCounter = takeDamageCoolDown;
            stamina -= collision.gameObject.GetComponent<Enemy>().GetDamage();
        }
    }

    private void Recover() {
        recoverCounter = recoverCoolDown;
        isRecovering = true;
        stamina = maxStamina;
        StayStill();
    }

    private void StayStill() {
        rb.velocity = new(0, 0);
        rb.bodyType = RigidbodyType2D.Static;
        sr.color = Color.black;
    }
}
