using UnityEngine;
using System.IO;
using Unity.Cinemachine;
using NUnit.Framework;
using System.Collections.Generic; 
using System.Linq;                

public class SaveController : MonoBehaviour
{
    private string saveLocation;
    private InventoryController inventoryController;
    private Chest[] chests;

    void Start()
    {
        InitializeComponents();
        LoadGame();
    }

    //private void InitializeComponents()
    //{
    //    saveLocation = Path.Combine(Application.persistentDataPath, "saveData.json");
    //    Debug.Log("Файл сохранения: " + saveLocation);
    //    inventoryController = FindFirstObjectByType<InventoryController>();
    //    //chests = FindObjectsOfType<Chest>();
    //    chests = Object.FindObjectsByType<Chest>(FindObjectsSortMode.None);
    //}

    private void InitializeComponents()
    {
        string sceneName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;  // Получаем имя текущей сцены
        saveLocation = Path.Combine(Application.persistentDataPath, $"saveData_{sceneName}.json");  // Создаем уникальный файл для каждой сцены
        Debug.Log("Файл сохранения: " + saveLocation);

        inventoryController = FindFirstObjectByType<InventoryController>();
        chests = Object.FindObjectsByType<Chest>(FindObjectsSortMode.None);
    }


    //public void SaveGame()
    //{
    //    GameObject player = GameObject.FindGameObjectWithTag("Player");
    //    GameObject companion = GameObject.FindGameObjectWithTag("Companion");

    //    SaveData saveData = new SaveData
    //    {
    //        playerPosition = player != null ? player.transform.position : Vector3.zero,
    //        companionPosition = companion != null ? companion.transform.position : Vector3.zero,
    //        mapBoundary = FindFirstObjectByType<CinemachineConfiner2D>().BoundingShape2D.gameObject.name,
    //        inventorySaveData = inventoryController.GetInventoryItems(),
    //        chestSaveDatas = GetChestsState()
    //    };

    //    File.WriteAllText(saveLocation, JsonUtility.ToJson(saveData));
    //}

    public void SaveGame()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        GameObject companion = GameObject.FindGameObjectWithTag("Companion");

        SaveData saveData = new SaveData
        {
            inventorySaveData = inventoryController.GetInventoryItems(),  // Общий инвентарь

            sceneData = new List<SceneSaveData> {
            new SceneSaveData
            {
                sceneName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name,  // Имя текущей сцены
                playerPosition = player != null ? player.transform.position : Vector3.zero,
                companionPosition = companion != null ? companion.transform.position : Vector3.zero,
                mapBoundary = FindFirstObjectByType<CinemachineConfiner2D>().BoundingShape2D.gameObject.name,
                chestSaveDatas = GetChestsState()  // Состояние сундуков
            }
        }
        };

        File.WriteAllText(saveLocation, JsonUtility.ToJson(saveData));
    }


    private List<ChestSaveData> GetChestsState()
    {
        List<ChestSaveData> chestStates = new List<ChestSaveData>();

        foreach(Chest chest in chests)
        {
            //ChestSaveData chestSaveData = new ChestSaveData
            //{
            //    chestID = chest.chestID, 
            //    isOpened = chest.isOpened
            //};
            ChestSaveData chestSaveData = new ChestSaveData
            {
                chestID = chest.ChestID,    
                isOpened = chest.IsOpened   
            };
            chestStates.Add(chestSaveData);
        }
        return chestStates;
    }


    //public void LoadGame()
    //{
    //    if (File.Exists(saveLocation))
    //    {
    //        SaveData saveData = JsonUtility.FromJson<SaveData>(File.ReadAllText(saveLocation));

    //        GameObject player = GameObject.FindGameObjectWithTag("Player");
    //        if (player != null)
    //            player.transform.position = saveData.playerPosition;

    //        GameObject companion = GameObject.FindGameObjectWithTag("Companion");
    //        if (companion != null)
    //            companion.transform.position = saveData.companionPosition;

    //        var confiner = FindFirstObjectByType<CinemachineConfiner2D>();
    //        var boundaryObject = GameObject.Find(saveData.mapBoundary);
    //        if (boundaryObject != null)
    //        {
    //            confiner.BoundingShape2D = boundaryObject.GetComponent<PolygonCollider2D>();
    //        }
    //        inventoryController.SetInventoryItems(saveData.inventorySaveData);

    //        LoadChestStates(saveData.chestSaveDatas);
    //    }
    //    else
    //    {
    //        SaveGame();

    //    }
    //}

    public void LoadGame()
    {
        if (File.Exists(saveLocation))
        {
            SaveData saveData = JsonUtility.FromJson<SaveData>(File.ReadAllText(saveLocation));

            // Загружаем общий инвентарь
            inventoryController.SetInventoryItems(saveData.inventorySaveData);

            // Загружаем данные для текущей сцены
            string currentSceneName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
            SceneSaveData currentSceneData = saveData.sceneData.FirstOrDefault(scene => scene.sceneName == currentSceneName);

            if (currentSceneData != null)
            {
                GameObject player = GameObject.FindGameObjectWithTag("Player");
                if (player != null)
                    player.transform.position = currentSceneData.playerPosition;

                GameObject companion = GameObject.FindGameObjectWithTag("Companion");
                if (companion != null)
                    companion.transform.position = currentSceneData.companionPosition;

                var confiner = FindFirstObjectByType<CinemachineConfiner2D>();
                var boundaryObject = GameObject.Find(currentSceneData.mapBoundary);
                if (boundaryObject != null)
                {
                    confiner.BoundingShape2D = boundaryObject.GetComponent<PolygonCollider2D>();
                }

                LoadChestStates(currentSceneData.chestSaveDatas);  // Загружаем сундуки для этой сцены
            }
        }
        else
        {
            SaveGame();
        }
    }
        
        



    private void LoadChestStates(List<ChestSaveData> chestStates)
    {
        foreach(Chest chest in chests)
        {
            ChestSaveData chestSaveData = chestStates.FirstOrDefault(c => c.chestID == chest.ChestID);

            if(chestSaveData != null)
            {
                chest.SetOpened(chestSaveData.isOpened);
            }
        }
    }
}

