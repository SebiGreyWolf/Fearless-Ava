using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;

public class DataPersistanceManager : MonoBehaviour
{
    [Header("Debugging")]
    [SerializeField] private bool disableDataPersistance = false;

    [SerializeField] private string fileName;

    private GameData gameData;
    private List<IDataPersistance> dataPersistanceObjects;

    private FileDataHandler dataHandler;

    private string selectedProfileId = "test";
    public static DataPersistanceManager Instance {  get; private set; }

    private void Awake()
    {
        if(Instance != null)
        {
            Debug.Log("More than one Instance of DataPersistanceManager. Newest one destroyed.");
            Destroy(this.gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(this.gameObject);

        if (disableDataPersistance)
        {
            Debug.Log("Data persistence is currently disabled");
        }

        this.dataHandler = new FileDataHandler(Application.persistentDataPath, fileName);
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.sceneUnloaded += OnSceneUnLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        SceneManager.sceneUnloaded -= OnSceneUnLoaded;
    }

    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        this.dataPersistanceObjects = FindAllDataPersistanceObjects();
        LoadGame();
    }

    public void OnSceneUnLoaded(Scene scene)
    {
        SaveGame();
    }

    public void ChangeSelectedProfileId(string newProfileId)
    {
        this.selectedProfileId = newProfileId;

        LoadGame();
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


        this.gameData = dataHandler.Load(selectedProfileId);

        if(this.gameData == null)
        {
            Debug.Log("No Data was found");
            return;
        }

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

        if (this.gameData == null)
        {
            Debug.Log("No Save Yet");
            return;
        }

        foreach (IDataPersistance obj in dataPersistanceObjects)
        {
            obj.SaveData(ref gameData);
        }

        dataHandler.Save(gameData, selectedProfileId);
    }

    private void OnApplicationQuit()
    {
        SaveGame();
    }

    public void DeleteGame()
    {
        dataHandler.Delete(selectedProfileId);
    }

    private List<IDataPersistance> FindAllDataPersistanceObjects()
    {
        IEnumerable<IDataPersistance> dataPersistenceObjects = FindObjectsOfType<MonoBehaviour>().OfType<IDataPersistance>();

        return new List<IDataPersistance>(dataPersistenceObjects);
    }

    public bool hasGameData()
    {
        return gameData != null;
    }

    public Dictionary<string, GameData> GetAllProfilesGameData()
    {
        return dataHandler.LoadAllProfiles();
    }

    public GameData GetGameData()
    {
        return gameData;
    }
}
