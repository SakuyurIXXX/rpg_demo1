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
            Debug.Log("û�д浵");
            NewGame();
        }


        foreach (ISaveManager saveManager in saveManagers)
        {
            saveManager.LoadData(gameData);
        }

    }

    public void SaveGame()
    {
        
        // ����ǰ��¼��ǰID
        string currentId = gameData.lastCheckpointId;
        
        // ��������SaveManager��SaveData
        foreach (ISaveManager saveManager in saveManagers)
        {
            saveManager.SaveData(ref gameData);
        }
        
        // ���ID�Ƿ�����ı�
        if (currentId != gameData.lastCheckpointId)
        {
            Debug.LogWarning($"���棺ID�ڱ�������б��ı� - ԭID: {currentId}, ��ID: {gameData.lastCheckpointId}");
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
        // ʹ��Resources.FindObjectsOfTypeAll��ȡ������Ϸ���󣬰���δ�����
        // Ӧ��������������Ĵ�����Դ
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

    // ��ӹ�����������ȡ������ID
    public string GetLastCheckpointId()
    {
        return gameData.lastCheckpointId;
    }
    
    // ��ӹ�������������������ID
    public void SetLastCheckpointId(string id)
    {
        gameData.lastCheckpointId = id;
    }

}
