using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class DataPersistanceManager : MonoBehaviour
{
    [Header("Debugging")]
    [SerializeField] private bool disableDataPersistance = false;
    [SerializeField] private bool initializeDataIfNull = false;
    [SerializeField] private bool overrideSelectedProfileId = false;
    [SerializeField] private string testSelectedProfileId = "test";

    [Header("File Storage Config")]
    [SerializeField] private string fileName;
    [SerializeField] private bool useEncryption = false;

    public static DataPersistanceManager Instance { get; private set; }

    private GameData gameData;

    private List<IDataPersistance> dataPersistanceObjects;

    private FileDataHandler dataHandler;

    private string selectedProfileId = "test";

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
            Debug.LogWarning("More than one Data Persistance Manager was found", this);
            return;
        }

        transform.parent = null;

        DontDestroyOnLoad(gameObject);

        if (disableDataPersistance)
        {
            Debug.LogWarning("Data Persistance is currently disabled!", this);
        }

        //Application.persistentDataPath points to the default directory of: %userprofile%\AppData\LocalLow\<companyname>\<productname>
        dataHandler = new FileDataHandler(Application.persistentDataPath, fileName, useEncryption);

        InitializeSelectedProfileId();

        dataPersistanceObjects = FindAllDataPersistanceObjects();
        LoadGame();
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        dataPersistanceObjects = FindAllDataPersistanceObjects();
        LoadGame();
    }

    public void ChangeSelectedProfileId(string newProfileId)
    {
        selectedProfileId = newProfileId;
        LoadGame();
    }

    public void DeleteProfileData(string profileId)
    {
        dataHandler.Delete(profileId);

        InitializeSelectedProfileId();

        LoadGame();
    }

    private void InitializeSelectedProfileId()
    {
        selectedProfileId = dataHandler.GetMostRecentlyUpdatedProfileId();
        if (overrideSelectedProfileId)
        {
            selectedProfileId = testSelectedProfileId;
            Debug.LogWarning("Overrode selected profile id with test id: " + testSelectedProfileId, this);
        }
    }

    public void NewGame()
    {
        this.gameData = new GameData();
    }

    public void LoadGame()
    {
        if (disableDataPersistance)
        {
            return;
        }

        //Load any saved data
        gameData = dataHandler.Load(selectedProfileId);

        if (gameData == null && initializeDataIfNull)
        {
            Debug.Log("No data was found. Initializing data to defaults", this);
            NewGame();
        }

        //if no data can be loaded, initialize to a new game
        if (gameData == null)
        {
            Debug.Log("No data was found. A New Game needs to be started before data can be loaded.", this);
            return;
        }

        //Push the loaded data to all other scripts that need it.
        foreach(IDataPersistance obj in dataPersistanceObjects)
        {
            obj.LoadData(gameData);
        }
    }

    public void SaveGame()
    {
        if (disableDataPersistance)
        {
            return;
        }

        //if we don't have any data to save log a warning
        if (gameData == null)
        {
            Debug.LogWarning("No data was found. A New Game needs to be started before data can be saved.");
            return;
        }

        //pass the data to other scripts so they can update it.
        foreach (IDataPersistance obj in dataPersistanceObjects)
        {
            obj.SaveData(gameData);
        }

        //timestamp the data so we know when it was last saved.
        gameData.lastUpdated = System.DateTime.Now.ToBinary();

        //save the data to a file using the data handler.
        dataHandler.Save(gameData, selectedProfileId);
    }

    private void OnApplicationQuit()
    {
        SaveGame();
    }

    private List<IDataPersistance> FindAllDataPersistanceObjects()
    {
        IEnumerable<IDataPersistance> dataPersistances = FindObjectsByType<MonoBehaviour>(FindObjectsInactive.Include, FindObjectsSortMode.None).OfType<IDataPersistance>();

        return new List<IDataPersistance>(dataPersistances);
    }

    public bool HasGameData()
    {
        return gameData != null;
    }

    public Dictionary<string, GameData> GetAllProfilesGameData()
    {
        return dataHandler.LoadAllProfiles();
    }

    public int GetSelectedWeapon()
    {
        return gameData.weaponSelected;
    }

    public void ChangeSelectedWeapon(int weaponIndex)
    {
        gameData.weaponSelected = weaponIndex;
    }
}
