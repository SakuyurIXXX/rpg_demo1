using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class Chest : MonoBehaviour
{
    // �����Ϻ͵��˵����߼�һ�£�ֻ������϶���������Э��

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
        // �Ȱ����
        yield return new WaitForSeconds(.5f);

        for (int i = 0; i < dropItems.Length; i++)
        {
            DropItem(dropItems[i]);
        }

        // ��������
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);

    }


    // ����itemData�Ķ���
    private void DropItem(ItemData _itemData)
    {
        GameObject newDrop = Instantiate(dropPrefab, transform.position, Quaternion.identity);

        Vector2 randomVelocity = new Vector2(Random.Range(-2, 2), Random.Range(7, 10));

        newDrop.GetComponent<ItemObject>().SetupItem(_itemData, randomVelocity);
    }
}
