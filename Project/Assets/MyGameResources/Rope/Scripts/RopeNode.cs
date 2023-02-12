using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Скрипт звена веревки(создающий все звенья)
/// </summary>
public class RopeNode : MonoBehaviour
{
    [HideInInspector]
    public GameObject Target;

    /// <summary>
    /// Левый конец
    /// </summary>
    [HideInInspector]
    public GameObject LeftBond;

    /// <summary>
    /// Правый конец
    /// </summary>
    [HideInInspector]
    public GameObject RightBond;

    [SerializeField]
    private GameObject ropesample;

    [SerializeField]
    private float step = 0.2f; //шаг веревки 

    private SpringJoint2D[] sj; //все компоненты пружины

    void Start()
    {
        sj = GetComponents<SpringJoint2D>();
    
        sj[0].enabled = false;
        sj[1].enabled = false;

        Vector3 targetVec = Target.transform.position - transform.position;
 
        if (targetVec.magnitude > step)
        {
            GameObject newrope = Instantiate(ropesample, transform.position + targetVec.normalized * step, Quaternion.identity);
           
            RopeNode newrope_rope = newrope.GetComponent<RopeNode>();
   
            newrope_rope.LeftBond = gameObject;
            newrope_rope.Target = Target;
            RightBond = newrope;
        }
        else 
        {
            RightBond = Target;
       
            SpringJoint2D ropeknot_sj = Target.AddComponent<SpringJoint2D>();
          
            ropeknot_sj.frequency = 25;
            ropeknot_sj.dampingRatio = 1;
        
            ropeknot_sj.connectedBody = GetComponent<Rigidbody2D>();
        }
       
        sj[0].connectedBody = LeftBond.GetComponent<Rigidbody2D>();
        sj[1].connectedBody = RightBond.GetComponent<Rigidbody2D>();

        sj[0].enabled = true;
        sj[1].enabled = true;
    }
}
