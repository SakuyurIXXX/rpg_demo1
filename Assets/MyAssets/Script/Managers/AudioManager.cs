using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [SerializeField] private float sfxMiniDistance;
    [SerializeField] private AudioSource[] sfx;
    [SerializeField] private AudioSource[] bgm;

    public bool playBgm;
    private int bgmIndex;

    [SerializeField] private float bgmIntervalTime;
    private float bgmtimer;

    private bool canPlaySFX;


    private void Awake()
    {
        if (instance != null)
            Destroy(instance.gameObject);
        else
            instance = this;

        Invoke("AllowSFX", 0.1f);
    }

    private void Update()
    {
        //bgmtimer -= Time.deltaTime;

        //if (!playBgm)
        //    StopAllBGM();
        //else
        //{
        //    if (!bgm[bgmIndex].isPlaying && bgmtimer <= 0)
        //    {
        //        PlayBGM(bgmIndex);
        //        bgmtimer = bgmIntervalTime; // 应该改成bgm停了再计时，以后再改
        //    }
        //}
    }
    public void PlaySFX(int _sfxIndex, Transform _source)
    {
        if (canPlaySFX == false)
            return;

        if (_source != null && Vector2.Distance(PlayerManager.instance.player.transform.position, _source.position) > sfxMiniDistance)
            return;
        if (_sfxIndex < sfx.Length)
        {
            sfx[_sfxIndex].pitch = Random.Range(.85f, 1.1f);    // 随机音调
            sfx[_sfxIndex].Play();
        }
    }

    public void StopSFX(int _index) => sfx[_index].Stop();

    public void StopSFXWithTime(int _index) => StartCoroutine(DecreaseVolume(sfx[_index]));

    public void StopBGMWithTime(int _index) => StartCoroutine(DecreaseVolume(bgm[_index]));

    // 声音淡出效果，可用于SFX和BGM
    private IEnumerator DecreaseVolume(AudioSource _audio)
    {
        float defaultVolume = _audio.volume;

        while (_audio.volume > .1f)
        {
            _audio.volume -= _audio.volume * .2f;
            yield return new WaitForSeconds(.4f);
            if (_audio.volume <= .1f)
            {
                _audio.Stop();
                _audio.volume = defaultVolume;
                break;
            }
        }

    }

    public void PlayBGM(int _bgmIndex)
    {
        bgmIndex = _bgmIndex;

        StopAllBGM();

        bgm[bgmIndex].Play();
    }

    public void PlayerRandomBGM()
    {
        bgmIndex = Random.Range(0, bgm.Length);
        PlayBGM(bgmIndex);
    }
    public void StopBGM(int _bgmIndex)
    {
        bgm[_bgmIndex].Stop();
    }

    public void StopAllBGM()
    {
        for (int i = 0; i < bgm.Length; i++)
        {
            bgm[i].Stop();
        }
    }


    private void AllowSFX() => canPlaySFX = true;
}
