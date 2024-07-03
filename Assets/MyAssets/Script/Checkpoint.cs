using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public string id;
    public bool activationStatus;
    public Dissolve dissolveTree;

    private void Start()
    {
    }

    [ContextMenu("生成checkpoint ID")]
    public void GenerateId()
    {
        // 去GamaObject里右键脚本使用这个方法
        id = System.Guid.NewGuid().ToString();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Player>() != null && activationStatus == false)
        {
            ActivateCheckpoint();
        }
    }

    public void ActivateCheckpoint()
    {
        AudioManager.instance.PlaySFX(11, null);
        activationStatus = true;

        if (dissolveTree != null)
        {
            dissolveTree.gameObject.SetActive(true);
            dissolveTree.GetComponent<SpriteRenderer>().color = new Color(255, 255, 255, 0);
            Invoke("CreateTree", 0.1f);

            // 水多加面面多加水了，这里先把gameObject.SetActive，
            // 要等一会才能去调用让树生成的特效开启协程，不然报错
        }

    }

    public void CreateTree()
    {
        dissolveTree.GetComponent<SpriteRenderer>().color = new Color(255, 255, 255, 255);
        dissolveTree.Appear(true, false);
    }
}
