using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PistolShootingAnimation : MonoBehaviour
{
    public Animator animator;
    public GameObject mainObj;

    public void StartAnimation()
    {
        if (!animator.GetBool("Shoot"))
            animator.SetBool("Shoot", true);
    }

    public void Shoot()
    {
        mainObj.SendMessage("Shot");
    }    

    public void StopAnimation()
    {
        animator.SetBool("Shoot", false);
    }
}
