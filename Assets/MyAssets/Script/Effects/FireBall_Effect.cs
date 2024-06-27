using UnityEngine;

//[CreateAssetMenu(fileName = "FireBall_Effect", menuName = "Data/ItemEffect/FireBall_Effect")]
public class FireBall_Effect : ItemEffect
{
    [SerializeField] private GameObject fireBallPrefeb;
    public override void ExecuteEffect()
    {
        GameObject newFireBall = Instantiate(fireBallPrefeb);
    }
}
