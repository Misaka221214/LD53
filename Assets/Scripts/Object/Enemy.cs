using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {
    private float speed = 4f;

    public GameObject target;
    public GameObject center;
    public Rigidbody2D rb;
    public SpriteRenderer sr;
    public Collider2D collider2d;
    public GameObject pickupEffect;
    public bool isBurning = false;
    public bool isSwamped = false;

    private float health = 10f;
    private Vector2 direction = new Vector2(0, 0);
    private float takeDamageCounter = 0f;
    private Animator animator;
    private bool isPickedUp = false;
    private float pickRange = 5f;
    private float grabGunSkillCounter = 0f;
    private bool isUsingGrabSkill = false;
    private float burningCounter = 0f;
    private float resetColorCounter = 0f;

    void Start() {
        health = MetaData.ENEMY_MAX_HEALTH;
        target = GameObject.Find("Player");
        center = GameObject.Find("Player");
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        collider2d = GetComponent<Collider2D>();
    }
    // Update is called once per frame
    void Update() {
        UpdateFacingDirection();

        if (MetaData.GRAB_GUN_SKILL == GrabGunSkill.GRAB && grabGunSkillCounter < 0f && Input.GetMouseButtonDown(1)) {
            TryPickupEnemy();
        }
    }

    void FixedUpdate() {
        CountDown();

        if (isPickedUp) {
            FollowCursor();
        }

        if (!isPickedUp && rb.bodyType == RigidbodyType2D.Dynamic) {
            Move();
        }

        if (isUsingGrabSkill && grabGunSkillCounter < 0f) {
            DropEnemy();
        }

        if (isBurning && burningCounter < 0f) {
            Burn();
        }

        if (sr.color == Color.red && resetColorCounter < 0f) {
            ResetColorResetColorToWhite();
        }
    }

    void TryPickupEnemy() {
        // Pick up enemy
        if (IsWithinPickupRange()) {
            isUsingGrabSkill = true;
            grabGunSkillCounter = MetaData.SKILL_COOLDOWN;
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);

            RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero);
            if (hit.collider != null && hit.collider.gameObject == gameObject) {
                PickupEnemy();
            }
        }
    }

    void PickupEnemy() {
        isPickedUp = true;
        rb.bodyType = RigidbodyType2D.Dynamic;
        pickupEffect.SetActive(true);
    }

    void UpdateFacingDirection() {
        direction = (target.transform.position - transform.position).normalized;
    }

    void FollowCursor() {
        // Follow mouse position
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = Camera.main.nearClipPlane;
        rb.velocity = (Camera.main.ScreenToWorldPoint(mousePos) - transform.position).normalized * MetaData.ENEMY_DRAG_SPEED;

        if (center != null) {
            RotateEnemy();
            LimitDragRange();
        }
    }

    void RotateEnemy() {
        Vector3 vectorToTarget = center.transform.position - transform.position;
        float angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg;
        Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation = Quaternion.Slerp(transform.rotation, q, Time.deltaTime * MetaData.ENEMY_DRAG_SPEED);
    }

    void LimitDragRange() {
        // Set drag limitation
        Vector3 centerPosition = center.transform.localPosition; //center of *black circle*
        Vector3 newLocation = transform.position; //*BlackCenter* + all that Math
        float distance = Vector3.Distance(newLocation, centerPosition); //distance from ~green object~ to *black circle*

        if (distance > MetaData.DRAG_RADIUS) //If the distance is less than the radius, it is already within the circle.
        {
            Vector3 fromOriginToObject = newLocation - centerPosition; //~GreenPosition~ - *BlackCenter*
            fromOriginToObject *= MetaData.DRAG_RADIUS / distance; //Multiply by radius //Divide by Distance
            newLocation = centerPosition + fromOriginToObject; //*BlackCenter* + all that Math
            transform.position = newLocation;
        }
    }

    void DropEnemy() {
        isPickedUp = false;
        pickupEffect.SetActive(false);
        grabGunSkillCounter = MetaData.SKILL_COOLDOWN;
        transform.rotation = new Quaternion(0, 0, 0, transform.rotation.w);
        isUsingGrabSkill = false;
    }

    void Burn() {
        burningCounter = MetaData.BURNING_COOLDOWN;
        TakeDamage(MetaData.BURN_DAMAGE);
    }

    void ResetColorResetColorToWhite() {
        resetColorCounter = MetaData.RESET_COLOR_COOLDOWN;
        sr.color = Color.white;
    }

    void CountDown() {
        takeDamageCounter -= Time.deltaTime;
        grabGunSkillCounter -= Time.deltaTime;
        burningCounter -= Time.deltaTime;
        resetColorCounter -= Time.deltaTime;
    }

    void Move() {
        rb.velocity = (isSwamped ? MetaData.SWAMP_MULTIPLIER : 1) * speed * direction;
    }

    private void OnCollisionStay2D(Collision2D collision) {
        if (collision.gameObject.CompareTag("Parcel") && takeDamageCounter < 0f) {
            TakeDamage(collision.gameObject.GetComponent<Parcel>().GetDamage());
        }
    }

    public float GetDamage() {
        return MetaData.ENEMY_DAMAGE;
    }

    public void TakeDamage(float damage) {
        takeDamageCounter = MetaData.TAKE_DAMAGE_COOLDOWN;
        health -= damage;

        if (health <= 0f) {
            Die();
        } else {
            resetColorCounter = MetaData.RESET_COLOR_COOLDOWN;
            sr.color = Color.red;
        }
    }

    void Die() {
        rb.bodyType = RigidbodyType2D.Static;
        collider2d.enabled = false;
        sr.color = Color.white;
        animator.SetBool("isDead", true);
    }

    bool IsWithinPickupRange() {
        if (center != null) {
            return Vector3.Distance(center.transform.position, transform.position) < pickRange;
        }
        return false;
    }
}
