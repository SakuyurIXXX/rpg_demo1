using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class InventoryManager : MonoBehaviour, ISaveManager
{
    public static InventoryManager instance;

    // 初始物资，开发中测试用
    public List<ItemData> startingItems;

    // 装备
    public List<InventoryItem> equipment;
    public Dictionary<ItemData_Equipment, InventoryItem> equipmentDictionary;

    // 装备库存
    public List<InventoryItem> inventory;
    public Dictionary<ItemData, InventoryItem> inventoryDictionary;

    // 背包
    public List<InventoryItem> stash;
    public Dictionary<ItemData, InventoryItem> stashDictionary;


    [Header("物品栏UI")]

    // 这是容器
    [SerializeField] private Transform inventorySlotParent;
    [SerializeField] private Transform stashSlotParent;
    [SerializeField] private Transform equipmentSlotParent;
    [SerializeField] private Transform statSlotParent;

    // 这是格子
    private UI_ItemSlot[] inventoryItemSlot;
    private UI_ItemSlot[] stashItemSlot;
    private UI_EquipmentSlot[] equipmentSlot;
    private UI_StatSlot[] statSlot;

    [Header("数据库")]
    public List<InventoryItem> loadedItems; // load时将id与数据库中的id匹配，如果匹配把存档数据发配给inventory,player,stash,skill tree等等
    public List<ItemData_Equipment> loadedEquipment;


    [Header("物品冷却")]
    public ItemData_Equipment currentFlask;
    private float lastTimeUsed;


    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

    }


    private void Start()
    {
        inventory = new List<InventoryItem>();
        inventoryDictionary = new Dictionary<ItemData, InventoryItem>();

        stash = new List<InventoryItem>();
        stashDictionary = new Dictionary<ItemData, InventoryItem>();

        equipment = new List<InventoryItem>();
        equipmentDictionary = new Dictionary<ItemData_Equipment, InventoryItem>();


        inventoryItemSlot = inventorySlotParent.GetComponentsInChildren<UI_ItemSlot>();
        stashItemSlot = stashSlotParent.GetComponentsInChildren<UI_ItemSlot>();
        equipmentSlot = equipmentSlotParent.GetComponentsInChildren<UI_EquipmentSlot>();
        statSlot = statSlotParent.GetComponentsInChildren<UI_StatSlot>();

        // 找了半天的BUG，解决游戏中重开inventory不能s&l的问题，方法是让AddStartingItems()在LoadData()执行之后再执行
        Invoke("AddStartingItems", 0.001f);

    }

    public bool CanCraft(ItemData_Equipment _itemToCraft, List<InventoryItem> _requiredMaterials)
    {
        List<InventoryItem> materialsToRemove = new List<InventoryItem>();

        for (int i = 0; i < _requiredMaterials.Count; i++)
        {
            if (stashDictionary.TryGetValue(_requiredMaterials[i].data, out InventoryItem stashValue)) // 判断数量是否足够
            {
                if (stashValue.stackSize < _requiredMaterials[i].stackSize)
                {
                    Debug.Log("材料不足");
                    return false;
                }
                else
                {
                    materialsToRemove.Add(stashValue);
                }

            }
            else
            {
                Debug.Log("材料不足");
                return false;
            }
        }


        for (int i = 0; i < materialsToRemove.Count; i++)
        {
            RemoveItem(materialsToRemove[i].data);
        }

        AddItem(_itemToCraft);
        Debug.Log("制作成功：" + _itemToCraft.name);

        return true;
    }


    public void UseFlask()
    {
        currentFlask = GetEquipment(EquipmentType.Flask);

        if (currentFlask == null)
            return;

        bool canUseFlask = Time.time > lastTimeUsed + currentFlask.itemCooldown;

        if (canUseFlask)
        {
            currentFlask.ExecuteItemEffect();
            AudioManager.instance.PlaySFX(13, null);
            lastTimeUsed = Time.time;
        }
        else
            Debug.Log("冷却中");
    }

    private void AddStartingItems()
    {
        // 发配存档中的装备（将已装备的装备装备）
        foreach (ItemData_Equipment item in loadedEquipment)
        {
            EquipItem(item);
        }

        // 发配存档中的item
        if (loadedItems.Count > 0)
        {
            // 有哪些
            foreach (InventoryItem item in loadedItems)
            {
                // 有几个
                for (int i = 0; i < item.stackSize; i++)
                {
                    AddItem(item.data);
                }
            }

            // 检查存档中的Items,如果有直接跳过发初始物资环节
            return;

        }

        // 发初始物资，开发中测试用
        for (int i = 0; i < startingItems.Count; i++)
        {
            AddItem(startingItems[i]);
        }
    }

    public void EquipItem(ItemData _item)
    {
        ItemData_Equipment newEquipment = _item as ItemData_Equipment;
        InventoryItem newItem = new InventoryItem(newEquipment);

        ItemData_Equipment oldEquipment = null;

        foreach (KeyValuePair<ItemData_Equipment, InventoryItem> item in equipmentDictionary)
        {
            if (item.Key.equipmentType == newEquipment.equipmentType)

                oldEquipment = item.Key;
        }

        if (oldEquipment != null)
        {
            UnequipItem(oldEquipment);
            AddItem(oldEquipment);
        }

        equipment.Add(newItem);
        equipmentDictionary.Add(newEquipment, newItem);
        newEquipment.AddModifiers();

        RemoveItem(_item);
        UpdateSlotUI();

    }

    public void UnequipItem(ItemData_Equipment itemToRemove)
    {
        if (equipmentDictionary.TryGetValue(itemToRemove, out InventoryItem value))
        {
            equipment.Remove(value);
            equipmentDictionary.Remove(itemToRemove);
            itemToRemove.RemoveModifiers();
        }
    }

    private void UpdateSlotUI()
    {
        // 人物属性格子UI
        for (int i = 0; i < statSlot.Length; i++)
        {
            statSlot[i].UpadateStatValueUI();
        }

        // 装备格子UI
        for (int i = 0; i < equipmentSlot.Length; i++)
        {
            foreach (KeyValuePair<ItemData_Equipment, InventoryItem> item in equipmentDictionary)
            {
                if (item.Key.equipmentType == equipmentSlot[i].equipmentType)

                    equipmentSlot[i].UpdateSlot(item.Value);
            }
        }

        // 装备库存格子UI
        for (int i = 0; i < inventoryItemSlot.Length; i++)
        {
            inventoryItemSlot[i].CleanUpSlot();
        }

        // 背包格子UI
        for (int i = 0; i < stashItemSlot.Length; i++)
        {
            stashItemSlot[i].CleanUpSlot();
        }

        // 装备库存UI
        for (int i = 0; i < inventory.Count; i++)
        {
            inventoryItemSlot[i].UpdateSlot(inventory[i]);
        }

        // 背包UI
        for (int i = 0; i < stash.Count; i++)
        {
            stashItemSlot[i].UpdateSlot(stash[i]);
        }
    }
    public void AddItem(ItemData _item)
    {

        // 装备放inventory
        if (_item.itemtype == ItemType.Equipment)
            AddToInventory(_item);
        // 其他材料放stash
        else if (_item.itemtype == ItemType.Material)
            AddToStash(_item);


        UpdateSlotUI();
    }

    private void AddToStash(ItemData _item)
    {
        // 如果字典里有这个东西，直接加
        if (stashDictionary.TryGetValue(_item, out InventoryItem value))
            value.AddStack();
        else
        // 没有就创建个新的
        {
            InventoryItem newItem = new InventoryItem(_item);
            stash.Add(newItem);
            stashDictionary.Add(_item, newItem);
        }
    }

    private void AddToInventory(ItemData _item)
    {
        // 如果字典里有这个东西，直接加
        if (inventoryDictionary.TryGetValue(_item, out InventoryItem value))
            value.AddStack();
        else
        // 没有就创建个新的
        {
            InventoryItem newItem = new InventoryItem(_item);
            inventory.Add(newItem);
            inventoryDictionary.Add(_item, newItem);
        }
    }

    public void RemoveItem(ItemData _item)
    {
        // inventory
        if (inventoryDictionary.TryGetValue(_item, out InventoryItem value))
        {
            // 数量=1的时候调用删除就时连数量带格子全部丨
            if (value.stackSize <= 1)
            {
                inventory.Remove(value);
                inventoryDictionary.Remove(_item);
            }
            // -1
            else
                value.RemoveStack();
        }

        //stash
        if (stashDictionary.TryGetValue(_item, out InventoryItem stashValue))
        {
            if (stashValue.stackSize <= 1)
            {
                stash.Remove(stashValue);
                stashDictionary.Remove(_item);
            }
            else
                stashValue.RemoveStack();
        }

        UpdateSlotUI();
    }

    public ItemData_Equipment GetEquipment(EquipmentType _type)
    {
        ItemData_Equipment equipedItem = null;

        foreach (KeyValuePair<ItemData_Equipment, InventoryItem> item in equipmentDictionary)
        {
            // 匹配item的equipmentType和装备格子的属性
            if (item.Key.equipmentType == _type)

                equipedItem = item.Key;
        }
        return equipedItem;
    }

    public void LoadData(GameData _data)
    {
        foreach (KeyValuePair<string, int> pair in _data.inventory)
        {
            // 读数据库数据
            foreach (var item in GetItemDataBase())
            {
                // 将存档信息中的id与数据库记录的id进行匹配，若存在这个id，把对应的值分配给inventory
                if (item != null && item.itemId == pair.Key)
                {
                    InventoryItem itemToLoad = new InventoryItem(item);
                    itemToLoad.stackSize = pair.Value;

                    loadedItems.Add(itemToLoad);
                }
            }
        }

        foreach (string loadedItemId in _data.equipmentId)
        {
            foreach (var item in GetItemDataBase())
            {
                if (item != null && loadedItemId == item.itemId)
                {
                    loadedEquipment.Add(item as ItemData_Equipment);
                }
            }
        }
    }


    // key = 对应item的id , value = 对应item的数量
    public void SaveData(ref GameData _data)
    {
        // 保存的时候清空物资，防止在开始时发配物资越发越多了
        _data.inventory.Clear();
        _data.equipmentId.Clear();


        foreach (KeyValuePair<ItemData, InventoryItem> pair in inventoryDictionary)

        {
            _data.inventory.Add(pair.Key.itemId, pair.Value.stackSize);
        }

        foreach (KeyValuePair<ItemData, InventoryItem> pair in stashDictionary)
        {
            _data.inventory.Add(pair.Key.itemId, pair.Value.stackSize);
        }

        foreach (KeyValuePair<ItemData_Equipment, InventoryItem> pair in equipmentDictionary)
        {
            _data.equipmentId.Add(pair.Key.itemId);
        }

    }

    // 把asset中已有的Equipment的itemData写到itemDataBase里面，相当于注册，在读档的时候根据这些信息把对应的item分配给Inventory
    private List<ItemData> GetItemDataBase()
    {
        List<ItemData> itemDataBase = new List<ItemData>();
        string[] assetNames = AssetDatabase.FindAssets("", new[] { "Assets/MyAssets/Data/Items" });

        foreach (string SOName in assetNames)
        {
            var SOPath = AssetDatabase.GUIDToAssetPath(SOName);
            var itemData = AssetDatabase.LoadAssetAtPath<ItemData>(SOPath);
            itemDataBase.Add(itemData);
        }

        return itemDataBase;
    }
}
