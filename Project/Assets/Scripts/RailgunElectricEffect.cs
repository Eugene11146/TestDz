using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RailgunElectricEffect : MonoBehaviour
{
    public GameObject electricEffect;

    Gun gun;

    void Start()
    {
        gun = GetComponent<Gun>();
        gun.onShoot += OnShoot;
    }

    void OnShoot()
    {
        Instantiate(electricEffect, gun.muzzleFlashTransform);
    }
}
