using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class UI_VolumeSlider : MonoBehaviour
{

    public Slider slider;
    public string parameter;

    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private float mutiplier;



    public void SliderValue(float _value) => audioMixer.SetFloat(parameter, Mathf.Log10(_value) * mutiplier);
    // 在设置里把slider的minValue设置成0.001，不然值为0时触底反弹了


    public void LoadSlider(float _value)
    {
        if (_value >= 0.001f)
            slider.value = _value;
    }
}
