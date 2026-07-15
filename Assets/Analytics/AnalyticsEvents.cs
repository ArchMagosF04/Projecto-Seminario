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

    public class LevelFailedEvent : Event
    {
        public LevelFailedEvent() : base("level_failed")
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

        public float FailureTimeSeconds
        {
            set => SetParameter("failure_time_seconds", value);
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
    }

    public class WeaponSelectedEvent : Event
    {
        public WeaponSelectedEvent() : base("weapon_selected")
        {
        }

        public string WeaponId
        {
            set => SetParameter("weapon_id", value);
        }

        public int WeaponIndex
        {
            set => SetParameter("weapon_index", value);
        }

        public string PreviousWeaponId
        {
            set => SetParameter("previous_weapon_id", value);
        }

        public bool ChangedWeapon
        {
            set => SetParameter("changed_weapon", value);
        }
    }

    public class WeaponSelectorOpenedEvent : Event
    {
        public WeaponSelectorOpenedEvent() : base("weapon_selector_opened")
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
    }

    public class LevelAbandonedEvent : Event
    {
        public LevelAbandonedEvent() : base("level_abandoned")
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

        public float AbandonmentTimeSeconds
        {
            set => SetParameter("abandonment_time_seconds", value);
        }

        public string AbandonReason
        {
            set => SetParameter("abandon_reason", value);
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

    public class ComboResultEvent : Event
    {
        public ComboResultEvent() : base("combo_result")
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

        public string ComboOutcome
        {
            set => SetParameter("combo_outcome", value);
        }

        public int ComboLength
        {
            set => SetParameter("combo_length", value);
        }

        public int ComboThreshold
        {
            set => SetParameter("combo_threshold", value);
        }

        public string BreakReason
        {
            set => SetParameter("break_reason", value);
        }
    }
}