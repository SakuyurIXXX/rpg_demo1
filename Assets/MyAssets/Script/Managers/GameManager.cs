using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour, ISaveManager
{
    public static GameManager instance;
    public Checkpoint[] checkpoints;
    [SerializeField] private string closestCheckpointId;
    private void Awake()
    {
        if (instance != null)
            Destroy(instance.gameObject);
        else
            instance = this;
        checkpoints = FindObjectsOfType<Checkpoint>(); // 执行顺序问题，在load之前要把存档点分配给checkpoints,不然这个数组会在没东西的时候被遍历，就没结果了，要不就手动分配对象
    }

    private void Start()
    {

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
        closestCheckpointId = _data.closestCheckpointId;

        Invoke("PlacePlayerAtClosestCheckpoint", .1f);

    }

    private void PlacePlayerAtClosestCheckpoint()
    {
        foreach (Checkpoint checkpoint in checkpoints)
        {
            if (closestCheckpointId == checkpoint.id)
                PlayerManager.instance.player.transform.position = checkpoint.transform.position;
        }
    }

    public void SaveData(ref GameData _data)
    {
        _data.closestCheckpointId = FindClosestCheckpoint().id;
        _data.checkpoints.Clear();

        foreach (Checkpoint checkpoint in checkpoints)
        {
            _data.checkpoints.Add(checkpoint.id, checkpoint.activationStatus);
        }
    }

    private Checkpoint FindClosestCheckpoint()
    {
        float closestDistance = Mathf.Infinity;
        Checkpoint closestCheckpoint = null;

        foreach (var checkpoint in checkpoints)
        {
            float distanceToCheckpoint = Vector2.Distance(PlayerManager.instance.player.transform.position, checkpoint.transform.position);

            if (distanceToCheckpoint < closestDistance && checkpoint.activationStatus == true)
            {
                closestDistance = distanceToCheckpoint;
                closestCheckpoint = checkpoint;
            }
        }

        return closestCheckpoint;
    }

    public void PauseGame(bool _pause)
    {
        if (_pause)
            Time.timeScale = 0;
        else
            Time.timeScale = 1;
    }
}
