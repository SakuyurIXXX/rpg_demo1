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

    [ContextMenu("����checkpoint ID")]
    public void GenerateId()
    {
        // ȥGamaObject���Ҽ��ű�ʹ���������
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

            // ˮ���������ˮ�ˣ������Ȱ�gameObject.SetActive��
            // Ҫ��һ�����ȥ�����������ɵ���Ч����Э�̣���Ȼ����
        }

    }

    public void CreateTree()
    {
        dissolveTree.GetComponent<SpriteRenderer>().color = new Color(255, 255, 255, 255);
        dissolveTree.Appear(true, false);
    }
}
