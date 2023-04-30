public static class MetaData {
    // Skills
    public static PlayerSkill PLAYER_SKILL = PlayerSkill.ROLLING;
    public static GrabGunSkill GRAB_GUN_SKILL = GrabGunSkill.GRAB;

    // Upgrades
    public static float BASE_SPEED = 5f;
    public static float MAX_STAMINA = 10f;
    public static float DAMAGE_MULTIPLIER = 1f;
    public static float SKILL_COOLDOWN = 5f;

    // Static Data
    public static float BURN_DAMAGE = 1f;
    public static float SWAMP_MULTIPLIER = 0.5f;
    public static float DRAG_RADIUS = 5f;
    public static float ROLLING_MULTIPLIER = 8f;
    public static float ROLLING_TIME = 0.2f;
    public static float SPRINT_MULTIPLIER = 1.5f;
    public static float SPRINT_TIME = 3f;

    // Enemy Data
    public static float ENEMY_MAX_HEALTH = 10f;
    public static float ENEMY_DAMAGE = 10f;
    public static float ENEMY_DRAG_SPEED = 20f;

    // Parcel Data
    public static float PARCEL_DAMAGE = 2f;
    public static float PARCEL_SPEED = 20f;
    public static float PARCEL_PICK_RANGE = 5f;
    public static float PARCEL_MASS = 10f;

    // Cooldown
    public static float RESET_COLOR_COOLDOWN = 0.1f;
    public static float BURNING_COOLDOWN = 0.5f;
    public static float TAKE_DAMAGE_COOLDOWN = 0.5f;
    public static float RECOVER_COOLDOWN = 1f;

    // Global Flag
    public static bool SPAWN_ENEMY = true;

    // Level Data
    public static int LEVEL_1_MAX_PARCEL = 3;
    public static int LEVEL_2_MAX_PARCEL = 5;
    public static int LEVEL_3_MAX_PARCEL = 5;
    public static float LEVEL_1_MAX_TIME = 9999f;
    public static float LEVEL_2_MAX_TIME = 5f;
    public static float LEVEL_3_MAX_TIME = 5f;

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
    SHOOTING,
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