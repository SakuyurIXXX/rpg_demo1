using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationTriggers : MonoBehaviour
{
    private Player player => GetComponentInParent<Player>(); // �൱����start��д�˸�GetComponentInParent<Player>()

    private void AnimationFinishTrigger() => player.AnimtionFinishTrigger();
    //private void AttackOver() => player.EndAttack();






    #region ע��:AnimationFinishTrigger()����ô�õģ�
    // �������Ǹ�����Playerȥ�����Ӷ���Animator,������һЩ������粥��������η�������
    // Animator������PlayerAnimationTriggers�����Կ���ѡ���ڶ������趨�Ĺؼ�֡�����������

    // Animator -> AnimationFinishTrigger();
    // AnimationFinishTrigger() => player.AnimtionFinishTrigger();
    // player.AnimtionFinishTrigger() => playerState.AnimationFinishTrigger();
    // ����ʹ�����״̬ʱΪfalse��triggerCalled = true; �Ӷ��˳����״̬
    // ʹ����������ĺô��ǿ����붯���Ĺؼ�֡�����ж��Ƿ��˳�״̬�����繥�����ε�combo

    /*ΪʲôҪ��ô�ƣ�
    ��Ϊ�̳���triggerCalledд��PlayerState�Ҫ��ı����������Ҫ��PlayerState��д����
    Ȼ����Player����д����������PlayerState��ķ���
    Ȼ����Ϊ�ű�����Animator����Ӷ����ϣ�������Ҫ������ű������Player��ķ���
    ����ڶ����Ĺؼ�֡�ϴ�������ű���ķ���*/

    /*�ܲ��ܰ�triggerCalledд��Player�������һ������
     ���У���ΪҪȷ�ϵ�ǰ����ʲô״̬
     */
    #endregion


}
