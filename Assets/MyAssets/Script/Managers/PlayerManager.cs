using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour, ISaveManager
{
    public static PlayerManager instance;
    public Player player;

    public int souls;
    private void Awake()
    {
        if (instance != null)
            Destroy(instance.gameObject);
        else
            instance = this;
    }

    public bool HaveEnoughSouls(int _price)
    {
        if (_price > souls)
        {
            Debug.Log("Not enough money");
            return false;
        }

        souls -= _price;
        return true;
    }

    public int GetCurrentSouls() => souls;

    public void LoadData(GameData _data)
    {
        this.souls = _data.souls;
    }

    public void SaveData(ref GameData _data)
    {
        _data.souls = this.souls;
    }
}
