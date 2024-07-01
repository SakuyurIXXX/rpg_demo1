using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDrop : MonoBehaviour
{
    [SerializeField] private ItemData[] possibleDrop; // 可能掉落的所有物品
    [SerializeField] private GameObject dropPrefab;

    public virtual void GenerateDrop()
    {
        // 遍历所有可能掉落物，概率到了的掉
        for (int i = 0; i < possibleDrop.Length; i++)
        {
            if (Random.Range(0, 100) <= possibleDrop[i].dropChance)
            {
                DropItem(possibleDrop[i]);
            }
        }

    }
    protected void DropItem(ItemData _itemData)
    {
        GameObject newDrop = Instantiate(dropPrefab, transform.position, Quaternion.identity);

        Vector2 randomVelocity = new Vector2(Random.Range(-2, 2), Random.Range(7, 10));

        newDrop.GetComponent<ItemObject>().SetupItem(_itemData, randomVelocity);
    }
}
