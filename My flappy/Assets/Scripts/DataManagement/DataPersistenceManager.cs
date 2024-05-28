using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Manages data persistence in the game
public class DataPersistenceManager : MonoBehaviour
{
    // Private field to store the singleton instance
    private static DataPersistenceManager _instance;

    //Public fields for data storage
    [Header("File Storage Config")]
    [SerializeField] private string fileName;
    [SerializeField] private bool encrypt;
    private FileDataHandler dataHandler;

    // Public property to access the singleton instance
    public static DataPersistenceManager Instance
    {
        get { return _instance; }
        private set { _instance = value; }
    }

    private GameData gameData;
    private List<InterfaceDataPersistence> dataPersistenceObjects;

    // Awake is called when the script instance is being loaded
    private void Awake()
    {
        // If an instance already exists and it's not this, destroy this instance
        if (Instance != null && Instance != this)
        {
            Debug.Log("Found more than one DataPersistenceManager");
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            // Ensure this instance persists across scenes
            DontDestroyOnLoad(gameObject);
        }
    }

    // Load game data when the game starts
    private void Start()
    {
        this.dataHandler = new FileDataHandler(Application.persistentDataPath, fileName, encrypt);
        this.dataPersistenceObjects = FindAllInterfaceDataPersistenceObjects();
        LoadGame();
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
    // Create a new game with default data
    public void NewGame()
    {
        this.gameData = new GameData();
    }

    // Save game data to persistent storage
    public void SaveGame()
    { 
        // Pass the data to other objects to update it
        Debug.Log("Saving game data");
        if(dataPersistenceObjects != null){
            foreach(InterfaceDataPersistence dataPersistenceObj in this.dataPersistenceObjects){
            dataPersistenceObj.LoadData(gameData);
            }
            Debug.Log("Saved highscore ="+ gameData.highScore);
        } else{
            Debug.Log("datapesistenseobjects list is null???");
        }
        
        // TODO - Save the changed data
    }

    // Load game data from persistent storage
    public void LoadGame()
    {
        // Load data from the save file using the data handler
        Debug.Log("Loading game data"); 
        dataHandler.Load();
        // TODO - Load data

        // If no file is found, start a new game
        if (this.gameData == null)
        {
            Debug.Log("No game data found: new game initiated");
            NewGame();
        }

        // Push the loaded data to wherever needed
        foreach(InterfaceDataPersistence dataPersistenceObj in this.dataPersistenceObjects){
            dataPersistenceObj.LoadData(gameData);
        }
        Debug.Log("Loaded highscore ="+ gameData.highScore);
    }

    // Find all objects in the scene that implement InterfaceDataPersistence
    public List<InterfaceDataPersistence> FindAllInterfaceDataPersistenceObjects()
    {
        return FindObjectsOfType<MonoBehaviour>()
            .OfType<InterfaceDataPersistence>()
            .ToList();
    }
}
