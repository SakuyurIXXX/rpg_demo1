using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class AttackPauseManager : MonoBehaviour
{
    public static AttackPauseManager instance;

    private bool isShake;
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    public void SetImapctFX(CinemachineImpulseSource _impulseSource, Vector3 _shakeForce, int _lookDirection, int _pauseTime)
    {
        CameraShake(_impulseSource, _shakeForce, _lookDirection);
        HitPause(_pauseTime);
    }

    //  震源 力V3 方向
    public void CameraShake(CinemachineImpulseSource _impulseSource, Vector3 _shakeForce, int _lookDirection)
    {
        _impulseSource.m_DefaultVelocity = new Vector3(_shakeForce.x * _lookDirection, _shakeForce.y);
        _impulseSource.GenerateImpulse();
    }


    public void HitPause(int duration)
    {
        StartCoroutine(Pause(duration));
    }

    IEnumerator Pause(int duration)
    {
        float pauseTime = duration / 60f; // 根据帧数调整的顿帧时间，可能需要优化修改
        Time.timeScale = 0;
        yield return new WaitForSecondsRealtime(pauseTime);
        Time.timeScale = 1;
    }
}
