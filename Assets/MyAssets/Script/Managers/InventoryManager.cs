using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class InventoryManager : MonoBehaviour, ISaveManager
{
    public static InventoryManager instance;

    // ��ʼ���ʣ������в�����
    public List<ItemData> startingItems;

    // װ��
    public List<InventoryItem> equipment;
    public Dictionary<ItemData_Equipment, InventoryItem> equipmentDictionary;

    // װ�����
    public List<InventoryItem> inventory;
    public Dictionary<ItemData, InventoryItem> inventoryDictionary;

    // ����
    public List<InventoryItem> stash;
    public Dictionary<ItemData, InventoryItem> stashDictionary;


    [Header("��Ʒ��UI")]

    // ��������
    [SerializeField] private Transform inventorySlotParent;
    [SerializeField] private Transform stashSlotParent;
    [SerializeField] private Transform equipmentSlotParent;
    [SerializeField] private Transform statSlotParent;

    // ���Ǹ���
    private UI_ItemSlot[] inventoryItemSlot;
    private UI_ItemSlot[] stashItemSlot;
    private UI_EquipmentSlot[] equipmentSlot;
    private UI_StatSlot[] statSlot;

    [Header("���ݿ�")]
    public List<InventoryItem> loadedItems; // loadʱ��id�����ݿ��е�idƥ�䣬���ƥ��Ѵ浵���ݷ����inventory,player,stash,skill tree�ȵ�
    public List<ItemData_Equipment> loadedEquipment;


    [Header("��Ʒ��ȴ")]
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

        // ���˰����BUG�������Ϸ���ؿ�inventory����s&l�����⣬��������AddStartingItems()��LoadData()ִ��֮����ִ��
        Invoke("AddStartingItems", 0.001f);

    }

    public bool CanCraft(ItemData_Equipment _itemToCraft, List<InventoryItem> _requiredMaterials)
    {
        List<InventoryItem> materialsToRemove = new List<InventoryItem>();

        for (int i = 0; i < _requiredMaterials.Count; i++)
        {
            if (stashDictionary.TryGetValue(_requiredMaterials[i].data, out InventoryItem stashValue)) // �ж������Ƿ��㹻
            {
                if (stashValue.stackSize < _requiredMaterials[i].stackSize)
                {
                    Debug.Log("���ϲ���");
                    return false;
                }
                else
                {
                    materialsToRemove.Add(stashValue);
                }

            }
            else
            {
                Debug.Log("���ϲ���");
                return false;
            }
        }


        for (int i = 0; i < materialsToRemove.Count; i++)
        {
            RemoveItem(materialsToRemove[i].data);
        }

        AddItem(_itemToCraft);
        Debug.Log("�����ɹ���" + _itemToCraft.name);

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
            Debug.Log("��ȴ��");
    }

    private void AddStartingItems()
    {
        // ����浵�е�װ��������װ����װ��װ����
        foreach (ItemData_Equipment item in loadedEquipment)
        {
            EquipItem(item);
        }

        // ����浵�е�item
        if (loadedItems.Count > 0)
        {
            // ����Щ
            foreach (InventoryItem item in loadedItems)
            {
                // �м���
                for (int i = 0; i < item.stackSize; i++)
                {
                    AddItem(item.data);
                }
            }

            // ���浵�е�Items,�����ֱ����������ʼ���ʻ���
            return;

        }

        // ����ʼ���ʣ������в�����
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
        // �������Ը���UI
        for (int i = 0; i < statSlot.Length; i++)
        {
            statSlot[i].UpadateStatValueUI();
        }

        // װ������UI
        for (int i = 0; i < equipmentSlot.Length; i++)
        {
            foreach (KeyValuePair<ItemData_Equipment, InventoryItem> item in equipmentDictionary)
            {
                if (item.Key.equipmentType == equipmentSlot[i].equipmentType)

                    equipmentSlot[i].UpdateSlot(item.Value);
            }
        }

        // װ��������UI
        for (int i = 0; i < inventoryItemSlot.Length; i++)
        {
            inventoryItemSlot[i].CleanUpSlot();
        }

        // ��������UI
        for (int i = 0; i < stashItemSlot.Length; i++)
        {
            stashItemSlot[i].CleanUpSlot();
        }

        // װ�����UI
        for (int i = 0; i < inventory.Count; i++)
        {
            inventoryItemSlot[i].UpdateSlot(inventory[i]);
        }

        // ����UI
        for (int i = 0; i < stash.Count; i++)
        {
            stashItemSlot[i].UpdateSlot(stash[i]);
        }
    }
    public void AddItem(ItemData _item)
    {

        // װ����inventory
        if (_item.itemtype == ItemType.Equipment)
            AddToInventory(_item);
        // �������Ϸ�stash
        else if (_item.itemtype == ItemType.Material)
            AddToStash(_item);


        UpdateSlotUI();
    }

    private void AddToStash(ItemData _item)
    {
        // ����ֵ��������������ֱ�Ӽ�
        if (stashDictionary.TryGetValue(_item, out InventoryItem value))
            value.AddStack();
        else
        // û�оʹ������µ�
        {
            InventoryItem newItem = new InventoryItem(_item);
            stash.Add(newItem);
            stashDictionary.Add(_item, newItem);
        }
    }

    private void AddToInventory(ItemData _item)
    {
        // ����ֵ��������������ֱ�Ӽ�
        if (inventoryDictionary.TryGetValue(_item, out InventoryItem value))
            value.AddStack();
        else
        // û�оʹ������µ�
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
            // ����=1��ʱ�����ɾ����ʱ������������ȫ��ح
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
            // ƥ��item��equipmentType��װ�����ӵ�����
            if (item.Key.equipmentType == _type)

                equipedItem = item.Key;
        }
        return equipedItem;
    }

    public void LoadData(GameData _data)
    {
        foreach (KeyValuePair<string, int> pair in _data.inventory)
        {
            // �����ݿ�����
            foreach (var item in GetItemDataBase())
            {
                // ���浵��Ϣ�е�id�����ݿ��¼��id����ƥ�䣬���������id���Ѷ�Ӧ��ֵ�����inventory
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


    // key = ��Ӧitem��id , value = ��Ӧitem������
    public void SaveData(ref GameData _data)
    {
        // �����ʱ��������ʣ���ֹ�ڿ�ʼʱ��������Խ��Խ����
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

    // ��asset�����е�Equipment��itemDataд��itemDataBase���棬�൱��ע�ᣬ�ڶ�����ʱ�������Щ��Ϣ�Ѷ�Ӧ��item�����Inventory
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
