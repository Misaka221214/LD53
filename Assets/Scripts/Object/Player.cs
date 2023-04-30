using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
    private float speed = 5f;

    public Rigidbody2D rb;
    public SpriteRenderer sr;
    public float stamina = 2f;
    public bool isBurning = false;
    public bool isSwamped = false;

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
    private bool isUsingGrabGunSkill = false;
    private bool isInvinsible = false;
    private float resetColorCounter = 0f;
    private float burningCounter = 0f;

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
    }

    void ExitRecover() {
        takeDamageCounter = MetaData.TAKE_DAMAGE_COOLDOWN;
        rb.bodyType = RigidbodyType2D.Dynamic;
        sr.color = Color.white;
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
            TakeStaminaDamage(collision.gameObject.GetComponent<Enemy>().GetDamage());
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
        rb.velocity = new(0, 0);
        rb.bodyType = RigidbodyType2D.Static;
        sr.color = Color.black;
    }

    void UsePlayerSkill() {
        if (playerSkillCounter < 0f) {
            playerSkillCounter = MetaData.SKILL_COOLDOWN;
            switch (MetaData.PLAYER_SKILL) {
                case PlayerSkill.ROLLING:
                    isInvinsible = true;
                    speed *= MetaData.ROLLING_MULTIPLIER;
                    isUsingRollingSkill = true;
                    rollingCounter = MetaData.ROLLING_TIME;
                    break;
                case PlayerSkill.SPRINT:
                    speed *= MetaData.SPRINT_MULTIPLIER;
                    isUsingSprintSkill = true;
                    sprintCounter = MetaData.SPRINT_TIME;
                    break;
                case PlayerSkill.DUPLICATE:
                    break;
                case PlayerSkill.REROLL:
                    break;
                case PlayerSkill.NONE:
                default:
                    break;
            }
        }
    }

    void UseGrabGunSkill() {
        if (grabGunSkillCounter < 0f) {
            grabGunSkillCounter = MetaData.SKILL_COOLDOWN;
            switch (MetaData.GRAB_GUN_SKILL) {
                case GrabGunSkill.SHOOTING:
                    break;
                case GrabGunSkill.ARCHI:
                    break;
                case GrabGunSkill.SHOCK:
                    break;
                case GrabGunSkill.FREEZE:
                    break;
                case GrabGunSkill.FACING:
                    break;
                case GrabGunSkill.GRAB:
                case GrabGunSkill.NONE:
                default:
                    break;
            }
        }

    }
}
