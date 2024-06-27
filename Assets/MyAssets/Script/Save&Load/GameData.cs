using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public int souls;

    public SerializableDictionary<string, bool> skillTree;

    public SerializableDictionary<string, int> inventory;
    public List<string> equipmentId;

    public SerializableDictionary<string, bool> checkpoints;
    public string closestCheckpointId;

    public SerializableDictionary<string, float> volumeSettings;
    public GameData()
    {
        this.souls = 0;

        skillTree = new SerializableDictionary<string, bool>();

        inventory = new SerializableDictionary<string, int>();
        equipmentId = new List<string>();

        checkpoints = new SerializableDictionary<string, bool>();
        closestCheckpointId = string.Empty;

        volumeSettings = new SerializableDictionary<string, float>();
    }
}
