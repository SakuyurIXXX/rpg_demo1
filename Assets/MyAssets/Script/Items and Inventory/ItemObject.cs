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

    // 防止东西一掉落就被捡起来了，导致玩家不知道捡到了什么东西
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
