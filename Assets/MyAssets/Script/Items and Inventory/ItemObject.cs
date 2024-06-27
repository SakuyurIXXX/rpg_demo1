using UnityEngine;

public class ItemObject : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private ItemData itemData;

    private void SetupVisuals()
    {
        if (itemData == null)
            return;
        GetComponent<SpriteRenderer>().sprite = itemData.icon;
        gameObject.name = "item - " + itemData.itemName;
    }

    public void SetupItem(ItemData _itemData, Vector2 _velocity)
    {
        itemData = _itemData;
        rb.velocity = _velocity;
        SetupVisuals();
    }

    // ��ֹ����һ����ͱ��������ˣ�������Ҳ�֪������ʲô����
    public void WaitToPickUpItem()
    {
        Invoke("PickUpItem", 0.3f);
    }

    public void PickUpItem()
    {
        AudioManager.instance.PlaySFX(10, null);
        InventoryManager.instance.AddItem(itemData);
        Destroy(gameObject);
    }
}
