using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//[System.Serializable]
//public class SaveData
//{
//    public string sceneName;
//    public Vector3 playerPosition;
//    public Vector3 companionPosition;
//    public string mapBoundary;

//    public List<InventorySaveData> inventorySaveData;
//    public List<ChestSaveData> chestSaveDatas;
//}

//[System.Serializable]
//public class ChestSaveData
//{
//    public string chestID;
//    public bool isOpened;
//}

[System.Serializable]
public class SaveData
{
    public List<SceneSaveData> sceneData;  
    public List<InventorySaveData> inventorySaveData; 
}

[System.Serializable]
public class SceneSaveData
{
    public string sceneName; 
    public Vector3 playerPosition;
    public Vector3 companionPosition;
    public string mapBoundary;
    public List<ChestSaveData> chestSaveDatas;
}

[System.Serializable]
public class ChestSaveData
{
    public string chestID;
    public bool isOpened;
}
