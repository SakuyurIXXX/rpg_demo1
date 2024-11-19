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
        // 如果存档中记录的最后检查点是这个,就激活它
        if (SaveManager.instance.GetLastCheckpointId() == id)
        {
            ActivateCheckpoint();
        }
    }

    // 激活检查点
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

    // 在检查点休息
    public void RestAtCheckpoint(Player player)
    {
        
        // 先设置为最后使用的检查点
        GameManager.instance.SetLastCheckpoint(this);
        
        // 设置存档
        SaveManager.instance.SetLastCheckpointId(id);
        
        // 恢复玩家满血
        player.stats.currentHp = player.stats.GetMaxHpValue();
        
        // 重生所有非Boss敌人
        // TODO: 实现 EnemyManager
        
        // 播放休息动画和音效
        AudioManager.instance.PlaySFX(11, null);
        if (dissolveTree != null)
        {
            dissolveTree.gameObject.SetActive(true);
            dissolveTree.GetComponent<SpriteRenderer>().color = new Color(255, 255, 255, 0);
            Invoke("CreateTree", 0.01f);
        }

        // 最后保存
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
