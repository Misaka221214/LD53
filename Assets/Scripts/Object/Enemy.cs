using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {
    public EnemyType enemyType;
    [SerializeField] private AudioSource peng;
    [SerializeField] private AudioSource pick;

    public GameObject bullet;
    public GameObject target;
    public GameObject center;
    public GameObject destroyEffect;
    public Rigidbody2D rb;
    public SpriteRenderer sr;
    public Collider2D collider2d;
    public GameObject pickupEffect;
    public bool isBurning = false;
    public bool isSwamped = false;
    public bool isPickedUp = false;

    private bool isFrozen = false;
    private float freeMoveCooldown = 0.3f;
    private float freeMoveCounter = 0f;
    private float health = 10f;
    private Vector2 direction = new Vector2(0, 0);
    private float takeDamageCounter = 0f;
    private Animator animator;
    private float pickRange = 5f;
    private float grabGunSkillCounter = 0f;
    private bool isUsingGrabSkill = false;
    private float burningCounter = 0f;
    private float resetColorCounter = 0f;
    private float rangeAttackCounter = 0f;
    private float freezeCounter = 0f;

    void Start() {
        health = SetHealth();
        target = GameObject.Find("Player");
        center = GameObject.Find("Player");
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        collider2d = GetComponent<Collider2D>();

        if (enemyType == EnemyType.BOSS) {
            rb.mass = 5f;
        }
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

        if (!isPickedUp && !isFrozen && rb.bodyType == RigidbodyType2D.Dynamic) {
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

        if (isFrozen && freezeCounter < 0f) {
            isFrozen = false;
            sr.color = Color.white;
        }
    }

    float SetHealth() {
        switch (enemyType) {
            case EnemyType.RANGE:
                return MetaData.RANGE_ENEMY_MAX_HEALTH;
            case EnemyType.BOSS:
                return 999999999f;
            case EnemyType.MELEE:
            default:
                return MetaData.ENEMY_MAX_HEALTH;
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
        if (pick) {
            pick.Play();
        }
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

    public void SetFreeze() {
        isFrozen = true;
        freezeCounter = MetaData.ENEMY_FREEZE_COOLDOWN;
        if (rb) {
            rb.velocity = new(0, 0);
        }
        if (sr) {
            sr.color = Color.black;
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
        rangeAttackCounter -= Time.deltaTime;
        freeMoveCounter -= Time.deltaTime;
        freezeCounter -= Time.deltaTime;
    }

    void Move() {
        if (freeMoveCounter < 0f) {
            switch (enemyType) {
                case EnemyType.RANGE:
                    if (Vector3.Distance(transform.position, target.transform.position) >= MetaData.RANGE_ENEMY_ATTACK_RANGE) {
                        rb.velocity = (isSwamped ? MetaData.SWAMP_MULTIPLIER : 1) * MetaData.ENEMY_SPEED * direction;
                    } else {
                        rb.velocity = new(0, 0);
                        RangeAttack();
                    }
                    break;
                case EnemyType.BOSS:
                    rb.velocity = (isSwamped ? MetaData.SWAMP_MULTIPLIER : 1) * MetaData.BOSS_ENEMY_SPEED * direction;
                    break;
                case EnemyType.MELEE:
                default:
                    rb.velocity = (isSwamped ? MetaData.SWAMP_MULTIPLIER : 1) * MetaData.ENEMY_SPEED * direction;
                    break;
            }
        }
    }

    public void DisableFreeMove() {
        freeMoveCounter = freeMoveCooldown;
    }

    private void RangeAttack() {
        if (rangeAttackCounter < 0f) {
            rangeAttackCounter = MetaData.RANGE_ENEMY_ATTACK_COOLDOWN;
            GameObject flyingBullet = Instantiate(bullet, transform.position + new Vector3(direction.x * 1.2f, direction.y * 1.2f, transform.position.z), transform.rotation);
            flyingBullet.GetComponent<Rigidbody2D>().AddForce(direction * 50f);
        }
    }

    private void OnCollisionStay2D(Collision2D collision) {
        if (takeDamageCounter < 0f) {
            if (collision.gameObject.CompareTag("Parcel")) {
                TakeDamage(MetaData.PARCEL_DAMAGE * MetaData.DAMAGE_MULTIPLIER);
            }
            if (collision.gameObject.CompareTag("Enemy")) {
                Enemy e = collision.gameObject.GetComponent<Enemy>();
                if (e.isPickedUp || e.enemyType == EnemyType.BOSS) {
                    TakeDamage(30f);
                }
            }
            if (collision.gameObject.CompareTag("Bullet")) {
                TakeDamage(6f);
                Destroy(collision.gameObject);
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if(collision.gameObject.CompareTag("Parcel") || collision.gameObject.CompareTag("Enemy") || collision.gameObject.CompareTag("Player")) {
            if (peng) {
                peng.Play();
            }
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
            if (enemyType != EnemyType.BOSS) {
                resetColorCounter = MetaData.RESET_COLOR_COOLDOWN;
                sr.color = Color.red;
            }
        }
    }

    void Die() {
        Instantiate(destroyEffect, transform.position, transform.rotation);
        Destroy(gameObject);
    }

    bool IsWithinPickupRange() {
        if (center != null) {
            return Vector3.Distance(center.transform.position, transform.position) < pickRange;
        }
        return false;
    }
}
