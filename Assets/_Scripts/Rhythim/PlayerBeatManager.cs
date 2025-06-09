using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBeatManager : MonoBehaviour
{
    public static PlayerBeatManager Instance;

    private SpriteRenderer sprite;

    [SerializeField] private Color correctColor;
    [SerializeField] private Color wrongColor;
    private Color defaultColor;

    private bool performedAction;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        sprite = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        defaultColor = sprite.color;
    }

    private void Update()
    {
        if (performedAction && !BeatManager.Instance.OneBeat.BeatGrace)
        {
            sprite.color = defaultColor;
        }
    }

    public void OnBeatAction()
    {
        bool condition = BeatManager.Instance.OneBeat.BeatGrace;

        if (condition)
        {
            sprite.color = correctColor;
        }
        else if(!condition)
        {
            sprite.color = wrongColor;
        }

        performedAction = true;
    }
}
