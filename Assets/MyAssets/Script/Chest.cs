using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class Chest : MonoBehaviour
{
    // 基本上和敌人掉落逻辑一致，只不过配合动画加入了协程

    [SerializeField] private ItemData[] dropItems;
    [SerializeField] private GameObject dropPrefab;
    private bool isOpened;
    public GameObject interactableHint;
    //Instantiate(interactableHint, hit.collider.transform.position + new Vector3(0, 1f, 0), Quaternion.identity);
    private Animator anim;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }
    private void Update()
    {
        interactableHint.SetActive(false);
    }


    public virtual void GenerateDrop()
    {
        if (!isOpened)
        {
            anim.SetTrigger("Open");
            StartCoroutine("WaitToDrop");
            isOpened = true;
        }
    }

    IEnumerator WaitToDrop()
    {
        // 等半秒掉
        yield return new WaitForSeconds(.5f);

        for (int i = 0; i < dropItems.Length; i++)
        {
            DropItem(dropItems[i]);
        }

        // 销毁箱子
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);

    }


    // 掉有itemData的东西
    private void DropItem(ItemData _itemData)
    {
        GameObject newDrop = Instantiate(dropPrefab, transform.position, Quaternion.identity);

        Vector2 randomVelocity = new Vector2(Random.Range(-2, 2), Random.Range(7, 10));

        newDrop.GetComponent<ItemObject>().SetupItem(_itemData, randomVelocity);
    }
}
