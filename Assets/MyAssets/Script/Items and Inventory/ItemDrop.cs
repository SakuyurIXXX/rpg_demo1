using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDrop : MonoBehaviour
{
    [SerializeField] private ItemData[] possibleDrop; // ���ܵ����������Ʒ
    [SerializeField] private GameObject dropPrefab;

    public virtual void GenerateDrop()
    {
        // �������п��ܵ�������ʵ��˵ĵ�
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
