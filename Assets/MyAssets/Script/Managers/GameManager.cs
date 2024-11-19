using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour, ISaveManager
{
    public static GameManager instance;
    public Checkpoint[] checkpoints;
    private Checkpoint lastCheckpoint;

    private void Awake()
    {
        if (instance != null)
            Destroy(instance.gameObject);
        else
            instance = this;
            
        checkpoints = FindObjectsOfType<Checkpoint>();
    }

    private void Start()
    {
        if (SceneManager.GetActiveScene().name == "Cave")
            AudioManager.instance.PlayBGM(0);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
            RestartScene();
    }

    public void RestartScene()
    {
        SaveManager.instance.SaveGame();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void LoadData(GameData _data)
    {
        foreach (KeyValuePair<string, bool> pair in _data.checkpoints)
        {
            foreach (Checkpoint checkpoint in checkpoints)
            {
                if (checkpoint.id == pair.Key && pair.Value == true)
                {
                    checkpoint.ActivateCheckpoint();
                }
            }
        }

        if (!string.IsNullOrEmpty(_data.lastCheckpointId))
        {
            foreach (Checkpoint checkpoint in checkpoints)
            {
                if (checkpoint.id == _data.lastCheckpointId)
                {
                    lastCheckpoint = checkpoint;
                    break;
                }
            }
        }

        Invoke("PlacePlayerAtLastCheckpoint", 0.1f);
    }

    private void PlacePlayerAtLastCheckpoint()
    {
        if (lastCheckpoint != null)
        {
            PlayerManager.instance.player.transform.position = lastCheckpoint.transform.position;
        }
        else
        {
            Debug.LogWarning("没有找到上次使用的检查点，玩家将在默认位置重生");
        }
    }

    public void SaveData(ref GameData _data)
    {
        // 1. 先保存最后使用的检查点ID
        if (lastCheckpoint != null)
        {
            _data.lastCheckpointId = lastCheckpoint.id;
        }
        
        // 2. 保存检查点激活状态
        _data.checkpoints.Clear();
        foreach (Checkpoint checkpoint in checkpoints)
        {
            // 只保存已激活的检查点
            if (checkpoint.activationStatus)
            {
                _data.checkpoints.Add(checkpoint.id, true);
            }
        }
        
        // 3. 确保lastCheckpointId不会被意外覆盖
        if (lastCheckpoint != null)
        {
            // 再次确认最后使用的检查点ID没有被改变
            if (_data.lastCheckpointId != lastCheckpoint.id)
            {
                _data.lastCheckpointId = lastCheckpoint.id;
            }
        }
    }

    public void SetLastCheckpoint(Checkpoint checkpoint)
    {
        if (checkpoint != null)
        {
            lastCheckpoint = checkpoint;
        }
    }

    public Vector3 GetLastCheckpointPosition()
    {
        if (lastCheckpoint != null)
            return lastCheckpoint.transform.position;
            
        return Vector3.zero;
    }

    public void PauseGame(bool _pause)
    {
        Time.timeScale = _pause ? 0 : 1;
    }
}
