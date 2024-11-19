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
            Debug.LogWarning("û���ҵ��ϴ�ʹ�õļ��㣬��ҽ���Ĭ��λ������");
        }
    }

    public void SaveData(ref GameData _data)
    {
        // 1. �ȱ������ʹ�õļ���ID
        if (lastCheckpoint != null)
        {
            _data.lastCheckpointId = lastCheckpoint.id;
        }
        
        // 2. ������㼤��״̬
        _data.checkpoints.Clear();
        foreach (Checkpoint checkpoint in checkpoints)
        {
            // ֻ�����Ѽ���ļ���
            if (checkpoint.activationStatus)
            {
                _data.checkpoints.Add(checkpoint.id, true);
            }
        }
        
        // 3. ȷ��lastCheckpointId���ᱻ���⸲��
        if (lastCheckpoint != null)
        {
            // �ٴ�ȷ�����ʹ�õļ���IDû�б��ı�
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
