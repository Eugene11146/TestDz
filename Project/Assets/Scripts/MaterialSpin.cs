using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialSpin : MonoBehaviour
{
    public Gun gun;
    public ItemThrower thrower;
    public Material spinMaterial;
    public GameObject main;
    public Animator animator;
    public float rotationAddition;
    public float lerpTime = 0.3f;

    Material thisMaterial;
    SpriteRenderer spriteRenderer;
    float thisRotation;
    float startLerp;
    float endLerp;
    float timeStartedLerping;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        thisMaterial = Instantiate(spinMaterial);
        spriteRenderer.material = thisMaterial;
    }

    void Update()
    {
        thisRotation = Lerp(startLerp, endLerp, timeStartedLerping, lerpTime);
        thisMaterial.SetFloat("_Intensivity", main.GetComponent<Properties>().thisMaterial.GetFloat("_Intensivity"));
        thisMaterial.SetVector("_Offset", new Vector4(0, thisRotation, 0, 0));
    }

    public void Spin()
    {
        startLerp = thisRotation;
        endLerp = thisRotation + rotationAddition;
        timeStartedLerping = Time.unscaledTime;
    }

    public void StopAnimation()
    {
        animator.SetBool("Shoot", false);
    }

    float Lerp(float start, float end, float timeStartLerping, float lerpTime = 1f)
    {
        float timeSinceStarted = Time.unscaledTime - timeStartLerping;

        float percentageComplete = timeSinceStarted / lerpTime;

        float result = Mathf.Lerp(start, end, Mathf.SmoothStep(0, 1, percentageComplete));

        return result;
    }
}
