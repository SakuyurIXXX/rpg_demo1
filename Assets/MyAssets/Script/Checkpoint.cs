using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public string id;
    public bool activationStatus;

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

    }
}
