using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EntityFX : MonoBehaviour
{
    private SpriteRenderer sr;

    [Header("弹出文本")]
    [SerializeField] private GameObject popUpTextPrefab;
    [SerializeField] private GameObject normalDamageTextPrefab;
    [SerializeField] private GameObject criticalDamageTextPrefab;
    [SerializeField] private GameObject fireDamageTextPrefab;
    [SerializeField] private GameObject iceDamageTextPrefab;
    [SerializeField] private GameObject lightingDamageTextPrefab;

    [Header("击中闪光特效")]
    [SerializeField] private float flashDuration;
    private Material originalMat;

    [Header("异常状态颜色")]
    [SerializeField] private Color[] igniteColor;
    [SerializeField] private Color[] chillColor;
    [SerializeField] private Color[] shockColor;

    private GameObject myHealthBar;

    private void Start()
    {
        sr = GetComponentInChildren<SpriteRenderer>();

        if (GetComponentInChildren<UI_HealthBar>() != null)
            myHealthBar = GetComponentInChildren<UI_HealthBar>().gameObject;
    }

    // 受到伤害弹出伤害文本，根据伤害类型改变文本的prefab，也可作为弹出提示信息的文本
    public void CreatePopUpText(string _text, int _damageType)
    {
        float randomX = Random.Range(-1, 1);
        float randomY = Random.Range(0, 2);

        Vector3 positionOffset = new Vector3(randomX, randomY, 0);


        switch (_damageType)
        {
            case 0:
                popUpTextPrefab = criticalDamageTextPrefab;
                break;
            case 1:
                popUpTextPrefab = fireDamageTextPrefab;
                break;
            case 2:
                popUpTextPrefab = iceDamageTextPrefab;
                break;
            case 3:
                popUpTextPrefab = lightingDamageTextPrefab;
                break;
            default:
                popUpTextPrefab = normalDamageTextPrefab;
                break;
        }

        GameObject newText = Instantiate(popUpTextPrefab, transform.position + positionOffset, Quaternion.identity);

        newText.GetComponent<TextMeshPro>().text = _text;
    }

    private IEnumerator RedColorBlink()
    {

        sr.color = Color.red;
        yield return new WaitForSeconds(.1f);
        sr.color = Color.white;
    }


    private void CancelColorChange()
    {
        CancelInvoke();
        sr.color = Color.white;
    }

    public void IgniteFxFor(float _seconds)
    {
        InvokeRepeating("IgniteColorFX", 0, 0.2f);
        Invoke("CancelColorChange", _seconds);
    }


    public void ChillFxFor(float _seconds)
    {
        InvokeRepeating("ChillColorFX", 0, 0.2f);
        Invoke("CancelColorChange", _seconds);
    }

    public void ShockFxFor(float _seconds)
    {
        InvokeRepeating("ShockColorFX", 0, 0.2f);
        Invoke("CancelColorChange", _seconds);
    }


    private void IgniteColorFX()
    {
        if (sr.color != igniteColor[0])
            sr.color = igniteColor[0];
        else
            sr.color = igniteColor[1];
    }

    private void ChillColorFX()
    {
        if (sr.color != chillColor[0])
            sr.color = chillColor[0];
        else
            sr.color = chillColor[1];
    }

    private void ShockColorFX()
    {
        if (sr.color != shockColor[0])
            sr.color = shockColor[0];
        else
            sr.color = shockColor[1];
    }

    public void MakeTransparent(bool _transparent)
    {
        if (_transparent)
        {
            sr.color = Color.clear;

        }
        else
        {
            sr.color = Color.white;
        }
    }
}

