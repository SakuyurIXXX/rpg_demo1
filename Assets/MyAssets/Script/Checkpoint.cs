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
        // ����浵�м�¼�������������,�ͼ�����
        if (SaveManager.instance.GetLastCheckpointId() == id)
        {
            ActivateCheckpoint();
        }
    }

    // �������
    public void ActivateCheckpoint()
    {
        AudioManager.instance.PlaySFX(11, null);
        activationStatus = true;

        if (dissolveTree != null)
        {
            dissolveTree.gameObject.SetActive(true);
            dissolveTree.GetComponent<SpriteRenderer>().color = new Color(255, 255, 255, 0);
            Invoke("CreateTree", 0.01f);
        }
    }

    // �ڼ�����Ϣ
    public void RestAtCheckpoint(Player player)
    {
        
        // ������Ϊ���ʹ�õļ���
        GameManager.instance.SetLastCheckpoint(this);
        
        // ���ô浵
        SaveManager.instance.SetLastCheckpointId(id);
        
        // �ָ������Ѫ
        player.stats.currentHp = player.stats.GetMaxHpValue();
        
        // �������з�Boss����
        // TODO: ʵ�� EnemyManager
        
        // ������Ϣ��������Ч
        AudioManager.instance.PlaySFX(11, null);
        if (dissolveTree != null)
        {
            dissolveTree.gameObject.SetActive(true);
            dissolveTree.GetComponent<SpriteRenderer>().color = new Color(255, 255, 255, 0);
            Invoke("CreateTree", 0.01f);
        }

        // ��󱣴�
        SaveManager.instance.SaveGame();
    }

    public void CreateTree()
    {
        if (dissolveTree != null && dissolveTree.gameObject.activeSelf)
        {
            dissolveTree.GetComponent<SpriteRenderer>().color = new Color(255, 255, 255, 255);
            dissolveTree.Appear(true, false);
        }
    }
}
