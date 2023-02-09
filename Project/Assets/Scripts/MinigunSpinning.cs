using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinigunSpinning : MonoBehaviour
{
    public Gun gunScript;
    public Material spinMaterial;
    public GameObject main;
    public AudioSource loopSource;
    public AudioSource stopSource;

    Material thisMaterial;
    SpriteRenderer spriteRenderer;
    float spinSpeed = 0;
    bool isShoting = false;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        thisMaterial = Instantiate(spinMaterial);
        spriteRenderer.material = thisMaterial;
    }

    void Update()
    {
        thisMaterial.SetFloat("_Intensivity", main.GetComponent<Properties>().thisMaterial.GetFloat("_Intensivity"));

        loopSource.pitch = Time.timeScale;
        stopSource.pitch = Time.timeScale;
        if (gunScript.isShooting)
        {
            if (!isShoting)
            {
                loopSource.Play();
                isShoting = true;
            }
            thisMaterial.SetVector("_Offset", new Vector4(0, thisMaterial.GetVector("_Offset").y + Time.deltaTime * 30, 0, 0));
            spinSpeed = 30;
        }
        else if(spinSpeed > 0)
        {
            if (isShoting)
            {
                loopSource.Stop();
                stopSource.Play();
                isShoting = false;
            }
            thisMaterial.SetVector("_Offset", new Vector4(0, thisMaterial.GetVector("_Offset").y + Time.deltaTime * spinSpeed, 0, 0));
            spinSpeed -= Time.deltaTime * 20;
            if (spinSpeed < 0.1f && ((thisMaterial.GetVector("_Offset").y * 10) % 10 < 1 || (thisMaterial.GetVector("_Offset").y * 10) % 10 > 9))
            {
                spinSpeed = 0;
                thisMaterial.SetVector("_Offset", new Vector4(0, 0, 0, 0));
            }
        }
    }
}
