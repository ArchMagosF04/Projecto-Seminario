using Unity.Services.Analytics;

public class LevelStartEvent : Event
{
    public LevelStartEvent() : base("level_start")
    {
    }

    public int LevelId
    {
        set => SetParameter("level_id", value);
    }

    public string LevelName
    {
        set => SetParameter("level_name", value);
    }

    public string WeaponId
    {
        set => SetParameter("weapon_id", value);
    }

    public int AttemptNumber
    {
        set => SetParameter("attempt_number", value);
    }

    public bool IsTutorial
    {
        set => SetParameter("is_tutorial", value);
    }
}

public class LevelCompleteEvent : Event
{
    public LevelCompleteEvent() : base("level_complete")
    {
    }

    public int LevelId
    {
        set => SetParameter("level_id", value);
    }

    public string LevelName
    {
        set => SetParameter("level_name", value);
    }

    public string WeaponId
    {
        set => SetParameter("weapon_id", value);
    }

    public int AttemptNumber
    {
        set => SetParameter("attempt_number", value);
    }

    public float CompletionTimeSeconds
    {
        set => SetParameter("completion_time_seconds", value);
    }

    public int TotalAttacks
    {
        set => SetParameter("total_attacks", value);
    }

    public int GoodHits
    {
        set => SetParameter("good_hits", value);
    }

    public int PerfectHits
    {
        set => SetParameter("perfect_hits", value);
    }

    public int MissedHits
    {
        set => SetParameter("missed_hits", value);
    }

    public int MaxCombo
    {
        set => SetParameter("max_combo", value);
    }

    public int DamageReceived
    {
        set => SetParameter("damage_received", value);
    }

    public int HealthRemaining
    {
        set => SetParameter("health_remaining", value);
    }
}