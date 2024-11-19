using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;

public class SaveManager : MonoBehaviour
{
    public static SaveManager instance;

    [SerializeField] private string fileName;
    [SerializeField] private bool encryptData;

    private GameData gameData;
    private List<ISaveManager> saveManagers;
    private FileDataHandler dataHandler;


    [ContextMenu("DeleteSaveData")]
    public void DeleteSaveData()
    {
        dataHandler = new FileDataHandler(Application.persistentDataPath, fileName, encryptData);
        dataHandler.Delete();
    }




    private void Awake()
    {
        if (instance != null)
            Destroy(instance.gameObject);
        else
            instance = this;
    }

    private void Start()
    {
        dataHandler = new FileDataHandler(Application.persistentDataPath, fileName, encryptData);
        saveManagers = FindAllSaveManagers();

        LoadGame();
    }



    public void NewGame()
    {
        gameData = new GameData();
    }

    public void LoadGame()
    {
        gameData = dataHandler.Load();

        if (this.gameData == null)
        {
            Debug.Log("没有存档");
            NewGame();
        }


        foreach (ISaveManager saveManager in saveManagers)
        {
            saveManager.LoadData(gameData);
        }

    }

    public void SaveGame()
    {
        
        // 保存前记录当前ID
        string currentId = gameData.lastCheckpointId;
        
        // 调用所有SaveManager的SaveData
        foreach (ISaveManager saveManager in saveManagers)
        {
            saveManager.SaveData(ref gameData);
        }
        
        // 检查ID是否被意外改变
        if (currentId != gameData.lastCheckpointId)
        {
            Debug.LogWarning($"警告：ID在保存过程中被改变 - 原ID: {currentId}, 新ID: {gameData.lastCheckpointId}");
        }
        
        dataHandler.Save(gameData);
    }

    private void OnApplicationQuit()
    {
        SaveGame();
    }

    //private List<ISaveManager> FindAllSaveManagers()
    //{

    //    IEnumerable<ISaveManager> saveManagers = FindObjectsOfType<MonoBehaviour>().OfType<ISaveManager>();

    //    return new List<ISaveManager>(saveManagers);

    //}

    private List<ISaveManager> FindAllSaveManagers()
    {
        // 使用Resources.FindObjectsOfTypeAll获取所有游戏对象，包括未激活的
        // 应急解决方法，消耗大量资源
        GameObject[] allObjects = Resources.FindObjectsOfTypeAll<GameObject>();

        List<ISaveManager> saveManagers = new List<ISaveManager>();

        foreach (GameObject obj in allObjects)
        {
            MonoBehaviour[] monoBehaviours = obj.GetComponents<MonoBehaviour>();

            foreach (MonoBehaviour behaviour in monoBehaviours)
            {
                if (behaviour is ISaveManager)
                {
                    saveManagers.Add(behaviour as ISaveManager);
                }
            }
        }

        return saveManagers;
    }


    public bool HasSaveData()
    {
        if (dataHandler.Load() != null)
            return true;
        return false;
    }

    // 添加公共方法来获取最后检查点ID
    public string GetLastCheckpointId()
    {
        return gameData.lastCheckpointId;
    }
    
    // 添加公共方法来设置最后检查点ID
    public void SetLastCheckpointId(string id)
    {
        gameData.lastCheckpointId = id;
    }

}
