using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CastController : MonoBehaviour
{
    [SerializeField] bool canBeCounter = false;

    [SerializeField] private Transform check;
    [SerializeField] private Vector2 boxSize;
    [SerializeField] private LayerMask whatIsPlayer;
    public CinemachineImpulseSource impulseSource;

    private CharacterStats mystats;

    public void SetupSpell(CharacterStats _stats) => mystats = _stats;

    private void AnimationTrigger()
    {
        Collider2D[] colliders = Physics2D.OverlapBoxAll(check.position, boxSize, whatIsPlayer);

        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Player>() != null)
            {
                hit.GetComponent<Player>().SetKnockbackForce(new Vector2(3, 3));
                mystats.DoDamage(hit.GetComponent<CharacterStats>(), hit.GetComponent<Player>().lookDirection * -1);

                // ¶ÙÖ¡+ÆÁÄ»Õð¶¯
                impulseSource = GetComponent<CinemachineImpulseSource>();
                AttackPauseManager.instance.SetImapctFX(impulseSource, new Vector3(0.4f, 0.3f, 0), 0, 6);
            }
        }
    }

    private void OnDrawGizmos() => Gizmos.DrawWireCube(check.position, boxSize);

    private void SelfDestroy() => Destroy(gameObject);

    public bool CanBeCounter()
    {
        if (canBeCounter == true)
        {
            CloseCounterAttackWindow();
            return true;
        }
        return false;
    }

    private void OpenCounterAttackWindow()
    {
        canBeCounter = true;
    }

    private void CloseCounterAttackWindow()
    {
        canBeCounter = false;
    }
}
