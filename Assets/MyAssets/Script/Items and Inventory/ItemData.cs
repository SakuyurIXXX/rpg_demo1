using System.Text;
using UnityEngine;



#if UNITY_EDITOR
using UnityEditor;
#endif


public enum ItemType
{
    Material,
    Equipment
}

[CreateAssetMenu(fileName = "New Item Data", menuName = "Data/Item")]

public class ItemData : ScriptableObject
{
    public ItemType itemtype;
    public string itemName;
    [TextArea]
    public string itemDescription;
    public Sprite icon;
    public string itemId;

    [Range(0, 100)]
    public float dropChance; // 后面可能要改，同种材料不同敌人掉率不同

    protected StringBuilder sb = new StringBuilder();


    // 特别神秘，总之系统给item自动分配了id，之后可以根据id拿到对应item
    private void OnValidate()
    {
#if UNITY_EDITOR
        string path = AssetDatabase.GetAssetPath(this);
        itemId = AssetDatabase.AssetPathToGUID(path);
#endif
    }

    public virtual string GetDescription()
    {
        return itemDescription;
    }

}
