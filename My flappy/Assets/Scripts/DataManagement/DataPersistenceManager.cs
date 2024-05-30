// using System.Collections.Generic;
// using System.Linq;
// using UnityEngine;
// using UnityEngine.SceneManagement;
// // Manages data persistence in the game
// public class DataPersistenceManager : MonoBehaviour
// {


//     //Public fields for data storage
//     [Header("File Storage Config")]
//     [SerializeField] private string fileName;
//     [SerializeField] private bool encrypt;
//     private FileDataHandler dataHandler;
//     private GameData gameData;
//     private List<InterfaceDataPersistence> dataPersistenceObjects;


//     // Public property to access the singleton instance
//     public static DataPersistenceManager Instance
//     {
//         get ;
//         private set ;
//     }

//     // Awake is called when the script instance is being loaded
//     private void Awake()
//     {
//         // If an instance already exists and it's not this, destroy this instance
//         if (Instance != null && Instance != this)
//         {
//             Debug.LogError("Found more than one DataPersistenceManager");
//             Destroy(gameObject);
//         }
//         else
//         {
//             Instance = this;
//             // Ensure this instance persists across scenes
//             DontDestroyOnLoad(gameObject);
//         }
//     }


//     // Load game data when the game starts
//     private void Start()
//     {
//         this.dataPersistenceObjects = FindAllInterfaceDataPersistenceObjects();
//         LoadGame();
//     }

//     // Save data whenever the game quits
//     private void OnApplicationQuit()
//     {
//      SaveGame();   
//     }
//     private void OnDestroy()
//     {
//         SaveGame(); 
//     }
//     // Create a new game with default data
//     public void NewGame()
//     {
//         this.gameData = new GameData();
//     }

//     // Save game data to persistent storage
//     public void SaveGame()
//     { 
//         // Pass the data to other objects to update it
//         Debug.Log("Saving game data");
//         for(int i = 0; i <this.dataPersistenceObjects.Count; i++){
//             InterfaceDataPersistence dataPersistenceObj = this.dataPersistenceObjects[i];
//             dataPersistenceObj.SaveData(ref gameData);
//             Debug.Log("Saved highscore ="+ gameData.highScore);
//         }
//         // foreach(InterfaceDataPersistence dataPersistenceObj in this.dataPersistenceObjects){
//         //     dataPersistenceObj.SaveData(ref gameData);
//         //     Debug.Log("Saved highscore ="+ gameData.highScore);
//         // } 


//     }

//     // Load game data from persistent storage
//     public void LoadGame()
//     {
        
//         // If no file is found, start a new game
//         if (this.gameData == null)
//         {
//             Debug.Log("No game data found: new game initiated");
//             NewGame();
//         }
//         // Push the loaded data to wherever needed
//         foreach(InterfaceDataPersistence dataPersistenceObj in this.dataPersistenceObjects){
//             dataPersistenceObj.LoadData(gameData);
//         }
//         Debug.Log("Loaded highscore ="+ gameData.highScore);
//     }

//     // Find all objects in the scene that implement InterfaceDataPersistence
//     // public List<InterfaceDataPersistence> FindAllInterfaceDataPersistenceObjects()
//     // {
//     //     IEnumerable<InterfaceDataPersistence> idpObjects = FindObjectsOfType<MonoBehaviour>()
//     //         .OfType<InterfaceDataPersistence>();
        
//     //     List<InterfaceDataPersistence> list = new List<InterfaceDataPersistence> (idpObjects);
//     //     Debug.Log("forming the list of datapersistenceOnjects list = " +  list.ToString());
//     //     return list;
//     // }

// }
using System.Collections.Generic;
using System.Linq;
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
    private List<InterfaceDataPersistence> dataPersistenceObjects;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Debug.Log("Found more than one DataPersistenceManager");
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    private void Start()
    {
        dataHandler = new FileDataHandler(Application.persistentDataPath, fileName, encrypt);
        dataPersistenceObjects = FindAllInterfaceDataPersistenceObjects();
        LoadGame();
    }

    private void OnApplicationQuit()
    {
        SaveGame();
    }

    private void OnDestroy()
    {
        SaveGame();
    }

    public void NewGame()
    {
        gameData = new GameData();
    }

    public void SaveGame()
    {
        Debug.Log("Saving game data");
        if (dataPersistenceObjects != null)
        {
            foreach (InterfaceDataPersistence dataPersistenceObj in dataPersistenceObjects)
            {
                dataPersistenceObj.SaveData(ref gameData);
            }
            Debug.Log("Saved highscore = " + gameData.highScore);
        }
        else
        {
            Debug.Log("dataPersistenceObjects list is null???");
        }

        // TODO - Save the changed data
    }

    public void LoadGame()
    {
        Debug.Log("Loading game data");
        dataHandler.Load();
        // TODO - Load data

        if (gameData == null)
        {
            Debug.Log("No game data found: new game initiated");
            NewGame();
        }

        foreach (InterfaceDataPersistence dataPersistenceObj in dataPersistenceObjects)
        {
            dataPersistenceObj.LoadData(gameData);
        }
        Debug.Log("Loaded highscore = " + gameData.highScore);
    }

    // Improved method to find all objects implementing InterfaceDataPersistence
    public List<InterfaceDataPersistence> FindAllInterfaceDataPersistenceObjects()
    {
        // List to hold all found objects
        List<InterfaceDataPersistence> results = new List<InterfaceDataPersistence>();

        // Find all objects in the active scene
        GameObject[] rootObjects = UnityEngine.SceneManagement.SceneManager.GetActiveScene().GetRootGameObjects();
        foreach (GameObject obj in rootObjects)
        {
            results.AddRange(obj.GetComponentsInChildren<InterfaceDataPersistence>(true));
        }

        // Include objects in DontDestroyOnLoad
        results.AddRange(GetDontDestroyOnLoadObjects());

        Debug.Log("Formed list of data persistence objects, count: " + results.Count);
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
