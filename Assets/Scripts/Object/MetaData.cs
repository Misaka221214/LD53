public static class MetaData {
    // Skills
    public static PlayerSkill PLAYER_SKILL = PlayerSkill.ROLLING;
    public static GrabGunSkill GRAB_GUN_SKILL = GrabGunSkill.SHOCK;

    // Upgrades
    public static float BASE_SPEED = 5f;
    public static float MAX_STAMINA = 10f;
    public static float DAMAGE_UPGRADE = 0f;
    public static float SKILL_COOLDOWN = 5f;

    // Static Data
    public static float BURN_DAMAGE = 1f;
    public static float SWAMP_MULTIPLIER = 0.5f;
    public static float DRAG_RADIUS = 5f;
    public static float ROLLING_MULTIPLIER = 8f;
    public static float ROLLING_TIME = 0.2f;
    public static float SPRINT_SPEED = 10f;
    public static float SPRINT_MASS = 50f;
    public static float SPRINT_TIME = 3f;
    public static float PLAYER_MASS = 5f;

    // Enemy Data
    public static float ENEMY_MAX_HEALTH = 11f;
    public static float ENEMY_DAMAGE = 12f;
    public static float ENEMY_DRAG_SPEED = 20f;
    public static float ENEMY_SPEED = 3f;
    public static float ENEMY_FREEZE_COOLDOWN = 1f;

    public static float RANGE_ENEMY_MAX_HEALTH = 8f;
    public static float RANGE_ENEMY_ATTACK_RANGE = 5f;
    public static float RANGE_ENEMY_ATTACK_COOLDOWN = 1.5f;
    public static float BULLET_SPEED = 7f;
    public static float BULLET_DAMAGE = 12f;
    public static float BULLET_DESTROY_TIME = 1f;

    public static float BOSS_ENEMY_SPEED = 5.7f;

    // Parcel Data
    public static float PARCEL_DAMAGE = 6f;
    public static float PARCEL_SPEED = 20f;
    public static float PARCEL_PICK_RANGE = 5f;
    public static float PARCEL_MASS = 10f;

    // Cooldown
    public static float RESET_COLOR_COOLDOWN = 0.1f;
    public static float BURNING_COOLDOWN = 0.5f;
    public static float TAKE_DAMAGE_COOLDOWN = 0.5f;
    public static float RECOVER_COOLDOWN = 1f;
    public static float FREEZE_COOLDOWN = 1f;
    public static float DUMMY_COOLDOWN = 6f;

    // Global Flag
    public static bool PICKED_UPGRADE = false;
    public static bool PICKED_SKILL = false;

    // Level Data
    public static int LEVEL_1_MAX_PARCEL = 3;
    public static int LEVEL_2_MAX_PARCEL = 3;
    public static int LEVEL_3_MAX_PARCEL = 4;
    public static float LEVEL_1_MAX_TIME = 120f;
    public static float LEVEL_2_MAX_TIME = 120f;
    public static float LEVEL_3_MAX_TIME = 120f;

    // Score
    public static bool HAS_SKILL_REWARD = false;

    // Scene
    public static string LEVEL_TO_BE_LOADED = "Level1";
}

public enum PlayerSkill {
    NONE,
    ROLLING,
    SPRINT,
    DUPLICATE,
    REROLL
}

public enum GrabGunSkill {
    NONE,
    GRAB,
    ARCHI,
    SHOCK,
    FREEZE,
    FACING
}

public enum TrapType {
    NONE,
    FIRE,
    SWAMP,
    HOLE
}

public enum UpgradeType {
    SPEED,
    DAMAGE,
    COOLDOWN
}

public enum EnemyType {
    MELEE,
    RANGE,
    BOSS
}