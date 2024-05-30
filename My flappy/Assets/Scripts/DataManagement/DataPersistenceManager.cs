using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DataPersistenceManager : MonoBehaviour
{
    private static DataPersistenceManager _instance;
    public static DataPersistenceManager Instance
    {
        get { return _instance; }
        private set { _instance = value; }
    }

    [Header("File Storage Config")]
    [SerializeField] private string fileName;
    [SerializeField] private bool encrypt;
    private FileDataHandler dataHandler;
    private GameData gameData;
    private List<InterfaceDataPersistence> dataPersistenceObjects = new List<InterfaceDataPersistence>();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // Destroy this instance if it's a duplicate
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        // Initialize data handler
        dataHandler = new FileDataHandler(Application.persistentDataPath, fileName, encrypt);
    }

    // Save data whenever the game quits
    private void OnApplicationQuit()
    {
        SaveGame();
    }
    private void OnDestroy()
    {
        SaveGame();
    }
    private void Start()
    {
        // Load game data
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

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        dataPersistenceObjects = FindAllInterfaceDataPersistenceObjects();
        Debug.Log($"Scene {scene.name} loaded, updated data persistence objects list count: {dataPersistenceObjects.Count}");
        LoadGame();
        foreach (var obj in dataPersistenceObjects)
        {
            Debug.Log($"Data persistence object: {obj}");
        }
    }

    public void SaveGame()
    {
        Debug.Log("Saving game data");

        if (dataPersistenceObjects != null && dataPersistenceObjects.Count > 0)
        {
            Debug.Log($"Before saving, high score = {gameData.highScore}");

            foreach (InterfaceDataPersistence dataPersistenceObj in dataPersistenceObjects)
            {
                if (dataPersistenceObj != null)
                {
                    dataPersistenceObj.SaveData(ref gameData);
                }
                else
                {
                    Debug.LogWarning("Encountered a null object in dataPersistenceObjects list.");
                }
            }

            Debug.Log($"After saving, high score = {gameData.highScore}");
        }
        else
        {
            Debug.LogError("dataPersistenceObjects list is null or empty!!!");
        }

        // Save the changed data to persistent storage
        //dataHandler.Save(gameData);
        Debug.Log($"Saved high score = {gameData.highScore}");
    }

    public void LoadGame()
    {
        Debug.Log("Loading game data");
        //gameData = dataHandler.Load();
        
        if (gameData == null)
        {
            Debug.Log("No game data found: new game initiated");
            NewGame();
        }

        if (dataPersistenceObjects != null && dataPersistenceObjects.Count > 0)
        {
            foreach (InterfaceDataPersistence dataPersistenceObj in dataPersistenceObjects)
            {
                if (dataPersistenceObj != null)
                {
                    dataPersistenceObj.LoadData(gameData);
                }
            }
        }

        Debug.Log($"Loaded high score = {gameData.highScore}");
    }

    public void NewGame()
    {
        gameData = new GameData();
    }

    // Find all objects in the scene that implement InterfaceDataPersistence
    public List<InterfaceDataPersistence> FindAllInterfaceDataPersistenceObjects()
    {
        // Reinitialize the list to ensure it's fresh each time
        List<InterfaceDataPersistence> results = new List<InterfaceDataPersistence>();

        // Find all objects in the active scene
        GameObject[] rootObjects = SceneManager.GetActiveScene().GetRootGameObjects();
        foreach (GameObject obj in rootObjects)
        {
            results.AddRange(obj.GetComponentsInChildren<InterfaceDataPersistence>(true));
        }

        // Include objects in DontDestroyOnLoad
        results.AddRange(GetDontDestroyOnLoadObjects());

        // Debug.Log($"Formed list of data persistence objects, count: {results.Count}");
        // foreach (var obj in results)
        // {
        //     Debug.Log($"Data persistence object: {obj}");
        // }

        return results;
    }

    // Helper method to get objects under DontDestroyOnLoad
    private List<InterfaceDataPersistence> GetDontDestroyOnLoadObjects()
    {
        List<InterfaceDataPersistence> results = new List<InterfaceDataPersistence>();
        GameObject temp = null;

        try
        {
            temp = new GameObject();
            DontDestroyOnLoad(temp);
            Scene dontDestroyOnLoadScene = temp.scene;

            GameObject[] rootObjects = dontDestroyOnLoadScene.GetRootGameObjects();
            foreach (GameObject obj in rootObjects)
            {
                if (obj != temp)
                {
                    results.AddRange(obj.GetComponentsInChildren<InterfaceDataPersistence>(true));
                }
            }
        }
        finally
        {
            if (temp != null)
            {
                Destroy(temp);
            }
        }

        return results;
    }
}
