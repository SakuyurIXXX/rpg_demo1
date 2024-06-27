using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationTriggers : MonoBehaviour
{
    private Player player => GetComponentInParent<Player>(); // 相当于在start里写了个GetComponentInParent<Player>()

    private void AnimationFinishTrigger() => player.AnimtionFinishTrigger();
    //private void AttackOver() => player.EndAttack();






    #region 注释:AnimationFinishTrigger()是怎么用的？
    // 往常都是父对象Player去控制子对象Animator,让它干一些事情比如播动画，这次反过来了
    // Animator挂载了PlayerAnimationTriggers，所以可以选择在动画中设定的关键帧调用这个函数

    // Animator -> AnimationFinishTrigger();
    // AnimationFinishTrigger() => player.AnimtionFinishTrigger();
    // player.AnimtionFinishTrigger() => playerState.AnimationFinishTrigger();
    // 于是使进入该状态时为false的triggerCalled = true; 从而退出这个状态
    // 使用这个方法的好处是可以与动画的关键帧联动判断是否退出状态，比如攻击连段的combo

    /*为什么要这么绕？
    因为教程中triggerCalled写在PlayerState里，要想改变这个参数就要在PlayerState里写方法
    然后在Player里再写个方法调用PlayerState里的方法
    然后因为脚本挂载Animator这个子对象上，所有又要在这个脚本里调用Player里的方法
    最后在动画的关键帧上触发这个脚本里的方法*/

    /*能不能把triggerCalled写在Player里？这样少一步调用
     不行，因为要确认当前处在什么状态
     */
    #endregion


}
