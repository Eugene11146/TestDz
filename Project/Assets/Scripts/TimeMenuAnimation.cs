using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeMenuAnimation : MonoBehaviour
{
    public Animator animator;

    private void Update()
    {
        animator.SetFloat("TimeScale", Time.timeScale);
    }
}
