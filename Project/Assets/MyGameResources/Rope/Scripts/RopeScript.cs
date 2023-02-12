using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Скрипт вервки, вешаем на олин из концов
/// </summary>
public class RopeScript : MonoBehaviour
{
    [SerializeField]
    private GameObject target;

    [SerializeField]
    private GameObject samplerope;

    [SerializeField]
    private float step = 0.2f;

    [SerializeField]
    private float frequency;

    [SerializeField]
    private float dampingRatio;

    void Start()
    {
        Vector3 targetVector = target.transform.position - transform.position;
   
        GameObject newrope = Instantiate(samplerope, transform.position + targetVector.normalized * step, Quaternion.identity);

        RopeNode newrope_rnes = newrope.GetComponent<RopeNode>();
        newrope_rnes.LeftBond = gameObject;
        newrope_rnes.Target = target;

        SpringJoint2D source_sj = gameObject.AddComponent<SpringJoint2D>();
        source_sj.frequency = frequency;
        source_sj.dampingRatio = dampingRatio;
        source_sj.connectedBody = newrope.GetComponent<Rigidbody2D>();
    }
}
