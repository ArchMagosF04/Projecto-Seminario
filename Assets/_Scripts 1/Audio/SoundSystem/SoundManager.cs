using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    private IObjectPool<SoundEmmiter> soundEmmiterPool; //Pool of sound emmiters.

    readonly List<SoundEmmiter> activeSoundEmmiters = new List<SoundEmmiter>();
    public readonly Queue<SoundEmmiter> FrequentSoundEmmiters = new Queue<SoundEmmiter>();


    [SerializeField] private SoundEmmiter soundEmmiterPrefab;

    [Header("Pool Settings")]
    [SerializeField] private bool collectionCheck = true;
    [SerializeField] private int defaultCapacity = 20;
    [SerializeField] private int maxPoolSize = 100; 
    [SerializeField] private int maxSoundInstances = 30; //The maximum number of instances for a single sound type. 

    private void Awake()
    {
        transform.parent = null;

        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        InitializePool();
    }

    public SoundBuilder CreateSound() => new SoundBuilder(this);

    public bool CanPlaySound(SoundData data)
    {
        if (!data.FrequentSound) return true;

        if (FrequentSoundEmmiters.Count >= maxSoundInstances && FrequentSoundEmmiters.TryDequeue(out var soundEmmiter))
        {
            try
            {
                soundEmmiter.Stop();
                return true;
            }
            catch
            {
                Debug.Log("SoundEmmiter is already released");
            }
            return false;
        }

        return true;
    }

    public SoundEmmiter Get()
    {
        return soundEmmiterPool.Get();
    }

    public void ReturnToPool(SoundEmmiter soundEmmiter)
    {
        soundEmmiterPool.Release(soundEmmiter);
    }

    private SoundEmmiter CreateSoundEmmiter()
    {
        var soundEmmiter = Instantiate(soundEmmiterPrefab);
        soundEmmiter.gameObject.SetActive(false);
        return soundEmmiter;
    }

    private void OnTakeFromPool(SoundEmmiter emmiter)
    {
        emmiter.gameObject.SetActive(true);
        activeSoundEmmiters.Add(emmiter);
    }

    private void OnReturnedToPool(SoundEmmiter emmiter)
    {
        emmiter.gameObject.SetActive(false);
        activeSoundEmmiters.Remove(emmiter);
    }

    private void OnDestroyPoolObject(SoundEmmiter emmiter)
    {
        Destroy(emmiter.gameObject);
    }

    private void InitializePool()
    {
        soundEmmiterPool = new ObjectPool<SoundEmmiter>(
            CreateSoundEmmiter,
            OnTakeFromPool,
            OnReturnedToPool,
            OnDestroyPoolObject,
            collectionCheck,
            defaultCapacity,
            maxPoolSize);
    }
}
