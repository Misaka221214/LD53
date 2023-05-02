using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
    private float speed = 5f;
    [SerializeField] AudioSource goodThing;
    [SerializeField] AudioSource rolling;
    [SerializeField] AudioSource damaged;

    public Rigidbody2D rb;
    public SpriteRenderer sr;
    public float stamina = 2f;
    public bool isBurning = false;
    public bool isSwamped = false;
    public GameObject shock;
    public GameObject dummy;
    public GameObject freeze;
    public GameObject fire;
    public GameObject reroll;
    public Animator animator;

    private Vector2 direction = new Vector2(0, 0);
    private float takeDamageCounter = 0f;
    private float recoverCounter = 0f;
    private float playerSkillCounter = 0f;
    private float grabGunSkillCounter = 0f;
    private float sprintCounter = 0f;
    private float rollingCounter = 0f;
    private bool isRecovering = false;
    private bool isUsingRollingSkill = false;
    private bool isUsingSprintSkill = false;
    private bool isInvinsible = false;
    private float resetColorCounter = 0f;
    private float burningCounter = 0f;
    private float freezeCounter = 0f;
    private float rerollCounter = 0f;

    // Start is called before the first frame update
    void Start() {
        stamina = MetaData.MAX_STAMINA;
        speed = MetaData.BASE_SPEED;
    }

    // Update is called once per frame
    void Update() {
        if (recoverCounter < 0f) {
            if (rb.bodyType == RigidbodyType2D.Static) {
                ExitRecover();
            }
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

            if (Input.GetKeyDown(KeyCode.Space)) {
                UsePlayerSkill();
            }
        }

        if (Input.GetMouseButtonDown(1)) {
            UseGrabGunSkill();
        }
    }

    void FixedUpdate() {
        CountDown();

        if (!isRecovering) {
            Move();
        }

        if (stamina <= 0f) {
            Recover();
        }

        if (isUsingRollingSkill && rollingCounter < 0f) {
            ExitRoll();
        }

        if (isUsingSprintSkill && sprintCounter < 0f) {
            ExitSprint();
        }

        if (sr.color == Color.red && resetColorCounter < 0f) {
            ResetColorToWhite();
        }

        if (isBurning && burningCounter < 0f) {
            Burn();
        }

        if (freeze.activeSelf && freezeCounter < 0f) {
            freeze.SetActive(false);
        }

        if (reroll.activeSelf && rerollCounter < 0f) {
            reroll.SetActive(false);
        }
    }

    public void PlayGoodThing() {
        goodThing.Play();
    }

    void Burn() {
        burningCounter = MetaData.BURNING_COOLDOWN;
        TakeStaminaDamage(MetaData.BURN_DAMAGE);
    }

    void ResetColorToWhite() {
        sr.color = Color.white;
        resetColorCounter = MetaData.RESET_COLOR_COOLDOWN;
    }

    void ExitRoll() {
        isInvinsible = false;
        speed = MetaData.BASE_SPEED;
        playerSkillCounter = MetaData.SKILL_COOLDOWN;
        isUsingRollingSkill = false;
    }

    void ExitSprint() {
        speed = MetaData.BASE_SPEED;
        rb.mass = MetaData.PLAYER_MASS;
        playerSkillCounter = MetaData.SKILL_COOLDOWN;
        isUsingSprintSkill = false;
    }

    void CountDown() {
        takeDamageCounter -= Time.deltaTime;
        recoverCounter -= Time.deltaTime;
        playerSkillCounter -= Time.deltaTime;
        grabGunSkillCounter -= Time.deltaTime;
        sprintCounter -= Time.deltaTime;
        rollingCounter -= Time.deltaTime;
        resetColorCounter -= Time.deltaTime;
        burningCounter -= Time.deltaTime;
        freezeCounter -= Time.deltaTime;
        rerollCounter -= Time.deltaTime;
    }

    void ExitRecover() {
        direction = new(0, 0);
        takeDamageCounter = MetaData.TAKE_DAMAGE_COOLDOWN;
        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.velocity = new(0, 0);
        //sr.color = Color.white;
        animator.Play("Player_idle");
        isRecovering = false;
    }

    void Move() {
        rb.velocity = (isSwamped ? MetaData.SWAMP_MULTIPLIER : 1) * speed * direction;
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
            if (damaged) {
                damaged.Play();
            }
            TakeStaminaDamage(collision.gameObject.GetComponent<Enemy>().GetDamage());
        }
        if (collision.gameObject.CompareTag("Bullet") && !isRecovering && takeDamageCounter < 0f) {
            if (damaged) {
                damaged.Play();
            }
            TakeStaminaDamage(MetaData.BULLET_DAMAGE);
            Destroy(collision.gameObject);
        }
    }

    public void TakeStaminaDamage(float damage) {
        if (!isInvinsible) {
            takeDamageCounter = MetaData.TAKE_DAMAGE_COOLDOWN;
            resetColorCounter = MetaData.RESET_COLOR_COOLDOWN;
            stamina -= damage;
            sr.color = Color.red;
        }
    }

    private void Recover() {
        recoverCounter = MetaData.RECOVER_COOLDOWN;
        isRecovering = true;
        stamina = MetaData.MAX_STAMINA;
        StayStill();
    }

    private void StayStill() {
        rb.bodyType = RigidbodyType2D.Static;
        //sr.color = Color.black;
        animator.Play("Player_recover");
    }

    void UsePlayerSkill() {
        if (playerSkillCounter < 0f) {
            playerSkillCounter = MetaData.SKILL_COOLDOWN;
            switch (MetaData.PLAYER_SKILL) {
                case PlayerSkill.ROLLING:
                    if (rolling) {
                        rolling.Play();
                    }
                    isInvinsible = true;
                    speed *= MetaData.ROLLING_MULTIPLIER;
                    isUsingRollingSkill = true;
                    rollingCounter = MetaData.ROLLING_TIME;
                    break;
                case PlayerSkill.SPRINT:
                    if (rolling) {
                        rolling.Play();
                    }
                    speed = MetaData.SPRINT_SPEED;
                    rb.mass = MetaData.SPRINT_MASS;
                    isUsingSprintSkill = true;
                    sprintCounter = MetaData.SPRINT_TIME;
                    break;
                case PlayerSkill.DUPLICATE:
                    Instantiate(dummy, RandomPosition(), transform.rotation);
                    break;
                case PlayerSkill.REROLL:
                    rerollCounter = 0.5f;
                    reroll.SetActive(true);
                    break;
                default:
                    break;
            }
        }
    }

    private Vector3 RandomPosition() {
        return new Vector3(transform.position.x + Random.Range(-0.1f, 0.1f), transform.position.y + Random.Range(-0.1f, 0.1f), transform.position.z);
    }

    void UseGrabGunSkill() {
        if (grabGunSkillCounter < 0f) {
            grabGunSkillCounter = MetaData.SKILL_COOLDOWN;
            switch (MetaData.GRAB_GUN_SKILL) {
                case GrabGunSkill.ARCHI:
                    Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    Instantiate(fire, new Vector3(mousePos.x, mousePos.y, transform.position.z), transform.rotation);
                    break;
                case GrabGunSkill.SHOCK:
                    Instantiate(shock, transform.position, transform.rotation);
                    break;
                case GrabGunSkill.FREEZE:
                    freezeCounter = MetaData.FREEZE_COOLDOWN;
                    freeze.SetActive(true);
                    break;
                default:
                    break;
            }
        }

    }

    public void UpdateUpgrades(UpgradeType upgradeType) {
        switch (upgradeType) {
            case UpgradeType.SPEED:
                speed = MetaData.BASE_SPEED;
                break;
            case UpgradeType.COOLDOWN:
            case UpgradeType.DAMAGE:
            default:
                break;
        }
    }
}
