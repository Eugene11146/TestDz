using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ConstraintController : MonoBehaviour
{
    public int selectedID = 0;
    public Toggle snapToggle;
    public GameObject[] constraints;
    public GameObject ropePreview;
    public GameObject lastGO;
    public GameObject selectedObj;
    public GameObject rotationBall;
    public GameObject thisBall;
    public GameObject DeleteEffect;
    public float snapScale = 0.1f;

    GameObject lastRopePreview;
    GameObject trg;
    Camera mainCamera;
    Vector2 posBegin;
    RaycastHit2D hit;
    bool snap = false;

    private void Start()
    {
        mainCamera = Camera.main;
    }

    void Update()
    {
        snap = snapToggle.isOn;
        if (selectedID != 12 && thisBall != null)
        {
            Destroy(thisBall);
        }

        if (selectedID > 0 && Input.touchCount <= 1 && selectedID != 12)
        {
            foreach (Touch touch in Input.touches)
            {
                if (touch.phase == TouchPhase.Began)
                {
                    hit = Physics2D.Raycast(mainCamera.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
                    if (hit.collider != null && hit.collider.TryGetComponent(out Rigidbody2D rigBody2D) && !IsPointerOverUIObject())
                    {
                        lastGO = hit.collider.gameObject;
                        trg = hit.collider.gameObject;
                        if (selectedID == 1)
                            lastGO.GetComponent<SpriteRenderer>().color = new Color(1, 0, 0, lastGO.GetComponent<SpriteRenderer>().color.a); //red
                        else
                            lastGO.GetComponent<SpriteRenderer>().color = new Color(0, 1, 1, lastGO.GetComponent<SpriteRenderer>().color.a); //cyan

                        if (snap)
                        {
                            posBegin = Snap(trg.transform.InverseTransformPoint(mainCamera.ScreenToWorldPoint(Input.mousePosition)));
                        }
                        else
                        {
                            posBegin = trg.transform.InverseTransformPoint(mainCamera.ScreenToWorldPoint(Input.mousePosition));
                        }

                        if (selectedID >= 2 && selectedID <= 9 && selectedID != 8)
                        {
                            lastRopePreview = Instantiate(ropePreview);
                            lastRopePreview.GetComponent<RopePreviewRender>().parent1 = hit.collider.gameObject;
                            lastRopePreview.GetComponent<RopePreviewRender>().localPos1 = hit.collider.transform.InverseTransformPoint(mainCamera.ScreenToWorldPoint(Input.mousePosition));
                        }
                    }

                }
                if (touch.phase == TouchPhase.Moved)
                {
                    hit = Physics2D.Raycast(mainCamera.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
                    if (lastRopePreview != null)
                    {
                        if (snap)
                        {
                            if (hit.collider != null)
                                lastRopePreview.GetComponent<RopePreviewRender>().localPos3 =
                                    lastRopePreview.GetComponent<RopePreviewRender>().parent1.transform.InverseTransformPoint
                                        (hit.collider.transform.TransformPoint(Snap(hit.collider.transform.InverseTransformPoint(mainCamera.ScreenToWorldPoint(Input.mousePosition)))));
                            else
                                lastRopePreview.GetComponent<RopePreviewRender>().localPos3 =
                                    lastRopePreview.GetComponent<RopePreviewRender>().parent1.transform.InverseTransformPoint(mainCamera.ScreenToWorldPoint(Input.mousePosition));
                        }
                        else
                            lastRopePreview.GetComponent<RopePreviewRender>().localPos3 =
                                lastRopePreview.GetComponent<RopePreviewRender>().parent1.transform.InverseTransformPoint(mainCamera.ScreenToWorldPoint(Input.mousePosition));
                    }
                    if (hit.collider == null && lastGO != null)
                    {
                        lastGO.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, lastGO.GetComponent<SpriteRenderer>().color.a); //white
                    }
                    if (hit.collider != null && !IsPointerOverUIObject())
                    {
                        if (hit.collider.TryGetComponent(out Rigidbody2D rigBody2D))
                        {
                            if (lastGO != null)
                            {
                                if (hit.collider.gameObject != lastGO)
                                {
                                    lastGO.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, lastGO.GetComponent<SpriteRenderer>().color.a); //white
                                }
                                else
                                {
                                    if (selectedID == 1)
                                        lastGO.GetComponent<SpriteRenderer>().color = new Color(1, 0, 0, lastGO.GetComponent<SpriteRenderer>().color.a); //red
                                    else
                                        lastGO.GetComponent<SpriteRenderer>().color = new Color(0, 1, 1, lastGO.GetComponent<SpriteRenderer>().color.a); //cyan
                                }
                            }
                            lastGO = hit.collider.gameObject;
                        }
                        if (hit.collider.isTrigger)
                        {
                            hit.collider.GetComponent<SpriteRenderer>().color = new Color(1, 0, 0, lastGO.GetComponent<SpriteRenderer>().color.a); //red
                            lastGO = hit.collider.gameObject;
                        }
                    }
                }
                if (touch.phase == TouchPhase.Ended)
                {
                    if (lastRopePreview != null)
                        Destroy(lastRopePreview);
                    if (lastGO != null)
                        lastGO.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, lastGO.GetComponent<SpriteRenderer>().color.a); //white
                    if (trg != null && selectedID != 1)
                    {
                        hit = Physics2D.Raycast(mainCamera.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
                        if (hit.collider != null && hit.collider.TryGetComponent(out Rigidbody2D rigBody2D))
                        {
                            AddConstraint(trg, hit.collider.gameObject, posBegin);
                        }
                        else
                        {
                            AddConstraint(trg, null, posBegin);
                        }    
                        trg = null;
                    }
                    if (selectedID == 1)
                    {
                        hit = Physics2D.Raycast(mainCamera.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
                        if (hit.collider != null && (hit.collider.TryGetComponent(out Rigidbody2D rigBody2D)))
                        {
                            AddConstraint(hit.collider.gameObject, null, new Vector2());
                        }
                    }
                }
            }
        }
        else if (selectedID == 12)
        {
            foreach (Touch touch in Input.touches)
            {
                if (touch.phase == TouchPhase.Began)
                {
                    hit = Physics2D.Raycast(mainCamera.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
                    if (hit.collider != null)
                    {
                        trg = hit.collider.gameObject;
                    }
                }
                if (touch.phase == TouchPhase.Ended)
                {
                    hit = Physics2D.Raycast(mainCamera.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
                    if (hit.collider != null && trg != null && trg == hit.collider.gameObject)
                    {
                        AddConstraint(null, null, new Vector2());
                    }
                }
            }
        }
        else
        {
            if (lastRopePreview != null)
                Destroy(lastRopePreview);
            if (lastGO != null)
                lastGO.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, lastGO.GetComponent<SpriteRenderer>().color.a); //white
            if (trg != null && selectedID != 1)
            {
                hit = Physics2D.Raycast(mainCamera.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
                if (hit.collider != null && hit.collider.TryGetComponent(out Rigidbody2D rigBody2D))
                {
                    AddConstraint(trg, hit.collider.gameObject, posBegin);
                }
                else
                {
                    AddConstraint(trg, null, posBegin);
                }
                trg = null;
            }
            if (selectedID == 1)
            {
                hit = Physics2D.Raycast(mainCamera.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
                if (hit.collider != null && (hit.collider.TryGetComponent(out Rigidbody2D rigBody2D)))
                {
                    AddConstraint(hit.collider.gameObject, null, new Vector2());
                }
            }

            if (lastGO)
                lastGO.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, lastGO.GetComponent<SpriteRenderer>().color.a); //white
        }
    }

    void AddConstraint(GameObject first, GameObject second, Vector2 firstPosBegin)
    {
        Vector2 secondPos = new Vector2(0, 0);
        if (second != null)
        {
            if (snap)
            {
                secondPos = Snap(second.transform.InverseTransformPoint(mainCamera.ScreenToWorldPoint(Input.mousePosition)));
            }
            else
            {
                secondPos = second.transform.InverseTransformPoint(mainCamera.ScreenToWorldPoint(Input.mousePosition));
            }
        }

        switch (selectedID)
        {
            case 1:
                if (!first.transform.root.transform.CompareTag("Bound") && !first.transform.root.transform.CompareTag("EasterEgg") && !IsPointerOverUIObject() && Input.touchCount <= 1)
                {
                    GameObject particle = Instantiate(DeleteEffect, first.transform.position, first.transform.rotation);
                    particle.transform.localScale = first.transform.lossyScale;
                    var shape = particle.GetComponent<ParticleSystem>().shape;
                    if (first.TryGetComponent(out SpriteRenderer spriteRenderer) && spriteRenderer.sprite != null)
                    {
                        shape.texture = spriteRenderer.sprite.texture;
                        shape.sprite = spriteRenderer.sprite;
                    }
                    else
                    {
                        if (first.transform.root.TryGetComponent(out spriteRenderer) && spriteRenderer.sprite != null)
                        {
                            shape.texture = spriteRenderer.sprite.texture;
                            shape.sprite = spriteRenderer.sprite;
                        }
                    }
                    particle.GetComponent<ParticleSystem>().Play();
                    if (first.CompareTag("Constraint"))
                    {
                        Destroy(first);
                    }
                    else
                    {
                        Destroy(first.transform.root.gameObject);
                    }
                }
                break;
            case 2:
                DistanceJoint2D distJoint;
                GameObject ropeThis;
                if (!first.transform.CompareTag("Bound") && !first.transform.root.CompareTag("Bound"))
                {
                    distJoint = first.AddComponent<DistanceJoint2D>();
                    distJoint.maxDistanceOnly = true;
                    if (second != null && second != first)
                        distJoint.connectedBody = second.GetComponent<Rigidbody2D>();
                    else
                        distJoint.connectedBody = GlobalSetting.defaultConstraintBody.GetComponent<Rigidbody2D>();
                    distJoint.enableCollision = true;
                    distJoint.anchor = firstPosBegin;
                    ropeThis = Instantiate(constraints[0]);
                    ropeThis.GetComponent<RopeRender>().parent1 = first;
                    ropeThis.GetComponent<RopeRender>().localPos1 = firstPosBegin;
                    if (second != null && second != first)
                    {
                        ropeThis.GetComponent<RopeRender>().parent3 = second;
                        ropeThis.GetComponent<RopeRender>().localPos3 = secondPos;
                        distJoint.connectedAnchor = secondPos;
                    }
                    else
                    {
                        ropeThis.GetComponent<RopeRender>().parent3 = GlobalSetting.defaultConstraintBody;
                        ropeThis.GetComponent<RopeRender>().localPos3 = GlobalSetting.defaultConstraintBody.transform.InverseTransformPoint(mainCamera.ScreenToWorldPoint(Input.mousePosition));
                        distJoint.connectedAnchor = GlobalSetting.defaultConstraintBody.transform.InverseTransformPoint(mainCamera.ScreenToWorldPoint(Input.mousePosition));
                    }
                    ropeThis.GetComponent<ConstraintRemover>().distanceJoint2D = distJoint;
                    ropeThis.transform.SetParent(first.transform);
                }
                break;
            case 3:
                if (!first.transform.CompareTag("Bound") && !first.transform.root.CompareTag("Bound"))
                {
                    distJoint = first.AddComponent<DistanceJoint2D>();
                    distJoint.maxDistanceOnly = true;
                    if (second != null && second != first)
                        distJoint.connectedBody = second.GetComponent<Rigidbody2D>();
                    else
                        distJoint.connectedBody = GlobalSetting.defaultConstraintBody.GetComponent<Rigidbody2D>();
                    distJoint.enableCollision = true;
                    distJoint.anchor = firstPosBegin;
                    distJoint.breakForce = 5000;
                    ropeThis = Instantiate(constraints[1]);
                    ropeThis.GetComponent<RopeRender>().parent1 = first;
                    ropeThis.GetComponent<RopeRender>().localPos1 = firstPosBegin;
                    if (second != null && second != first)
                    {
                        ropeThis.GetComponent<RopeRender>().parent3 = second;
                        ropeThis.GetComponent<RopeRender>().localPos3 = secondPos;
                        distJoint.connectedAnchor = secondPos;
                    }
                    else
                    {
                        ropeThis.GetComponent<RopeRender>().parent3 = GlobalSetting.defaultConstraintBody;
                        ropeThis.GetComponent<RopeRender>().localPos3 = GlobalSetting.defaultConstraintBody.transform.InverseTransformPoint(mainCamera.ScreenToWorldPoint(Input.mousePosition));
                        distJoint.connectedAnchor = GlobalSetting.defaultConstraintBody.transform.InverseTransformPoint(mainCamera.ScreenToWorldPoint(Input.mousePosition));
                    }
                    ropeThis.GetComponent<ConstraintRemover>().distanceJoint2D = distJoint;
                    ropeThis.transform.SetParent(first.transform);
                }
                break;
            case 4:
                if (!first.transform.CompareTag("Bound") && !first.transform.root.CompareTag("Bound"))
                {
                    distJoint = first.AddComponent<DistanceJoint2D>();
                    if (second != null && second != first)
                        distJoint.connectedBody = second.GetComponent<Rigidbody2D>();
                    else
                        distJoint.connectedBody = GlobalSetting.defaultConstraintBody.GetComponent<Rigidbody2D>();
                    distJoint.enableCollision = true;
                    distJoint.anchor = firstPosBegin;
                    ropeThis = Instantiate(constraints[2]);
                    ropeThis.GetComponent<FixedRopeRender>().parent1 = first;
                    ropeThis.GetComponent<FixedRopeRender>().localPos1 = firstPosBegin;
                    if (second != null && second != first)
                    {
                        ropeThis.GetComponent<FixedRopeRender>().parent3 = second;
                        ropeThis.GetComponent<FixedRopeRender>().localPos3 = secondPos;
                        distJoint.connectedAnchor = secondPos;
                    }
                    else
                    {
                        ropeThis.GetComponent<FixedRopeRender>().parent3 = GlobalSetting.defaultConstraintBody;
                        ropeThis.GetComponent<FixedRopeRender>().localPos3 = GlobalSetting.defaultConstraintBody.transform.InverseTransformPoint(mainCamera.ScreenToWorldPoint(Input.mousePosition));
                        distJoint.connectedAnchor = GlobalSetting.defaultConstraintBody.transform.InverseTransformPoint(mainCamera.ScreenToWorldPoint(Input.mousePosition));
                    }
                    ropeThis.GetComponent<ConstraintRemover>().distanceJoint2D = distJoint;
                    ropeThis.transform.SetParent(first.transform);
                }
                break;
            case 5:
                if (!first.transform.CompareTag("Bound") && !first.transform.root.CompareTag("Bound"))
                {
                    distJoint = first.AddComponent<DistanceJoint2D>();
                    if (second != null && second != first)
                        distJoint.connectedBody = second.GetComponent<Rigidbody2D>();
                    else
                        distJoint.connectedBody = GlobalSetting.defaultConstraintBody.GetComponent<Rigidbody2D>();
                    distJoint.enableCollision = true;
                    distJoint.anchor = firstPosBegin;
                    distJoint.breakForce = 5000;
                    ropeThis = Instantiate(constraints[3]);
                    ropeThis.GetComponent<FixedRopeRender>().parent1 = first;
                    ropeThis.GetComponent<FixedRopeRender>().localPos1 = firstPosBegin;
                    if (second != null && second != first)
                    {
                        ropeThis.GetComponent<FixedRopeRender>().parent3 = second;
                        ropeThis.GetComponent<FixedRopeRender>().localPos3 = secondPos;
                        distJoint.connectedAnchor = secondPos;
                    }
                    else
                    {
                        ropeThis.GetComponent<FixedRopeRender>().parent3 = GlobalSetting.defaultConstraintBody;
                        ropeThis.GetComponent<FixedRopeRender>().localPos3 = GlobalSetting.defaultConstraintBody.transform.InverseTransformPoint(mainCamera.ScreenToWorldPoint(Input.mousePosition));
                        distJoint.connectedAnchor = GlobalSetting.defaultConstraintBody.transform.InverseTransformPoint(mainCamera.ScreenToWorldPoint(Input.mousePosition));
                    }
                    ropeThis.GetComponent<ConstraintRemover>().distanceJoint2D = distJoint;
                    ropeThis.transform.SetParent(first.transform);
                }
                break;
            case 6:
                FixedJoint2D fixJoint;
                if (!first.transform.CompareTag("Bound") && !first.transform.root.CompareTag("Bound"))
                {
                    fixJoint = first.AddComponent<FixedJoint2D>();
                    if (second != null && second != first)
                        fixJoint.connectedBody = second.GetComponent<Rigidbody2D>();
                    else
                        fixJoint.connectedBody = GlobalSetting.defaultConstraintBody.GetComponent<Rigidbody2D>();
                    fixJoint.anchor = firstPosBegin;
                    if (second != null && second != first)
                    {
                        fixJoint.connectedAnchor = secondPos;
                    }
                    else
                    {
                        fixJoint.connectedAnchor = GlobalSetting.defaultConstraintBody.transform.InverseTransformPoint(mainCamera.ScreenToWorldPoint(Input.mousePosition));
                    }
                    ropeThis = Instantiate(constraints[4]);
                    ropeThis.GetComponent<FixedRopeRender>().parent1 = first;
                    ropeThis.GetComponent<FixedRopeRender>().localPos1 = firstPosBegin;
                    if (second != null && second != first)
                    {
                        ropeThis.GetComponent<FixedRopeRender>().parent3 = second;
                        ropeThis.GetComponent<FixedRopeRender>().localPos3 = secondPos;
                        fixJoint.connectedAnchor = secondPos;
                        ropeThis.GetComponent<ConstraintRemover>().constraintLocalPos = second.transform.InverseTransformPoint(mainCamera.ScreenToWorldPoint(Input.mousePosition));
                    }
                    else
                    {
                        ropeThis.GetComponent<FixedRopeRender>().parent3 = GlobalSetting.defaultConstraintBody;
                        ropeThis.GetComponent<FixedRopeRender>().localPos3 = GlobalSetting.defaultConstraintBody.transform.InverseTransformPoint(mainCamera.ScreenToWorldPoint(Input.mousePosition));
                        fixJoint.connectedAnchor = GlobalSetting.defaultConstraintBody.transform.InverseTransformPoint(mainCamera.ScreenToWorldPoint(Input.mousePosition));
                        ropeThis.GetComponent<ConstraintRemover>().constraintLocalPos = first.transform.InverseTransformPoint(mainCamera.ScreenToWorldPoint(Input.mousePosition));
                    }
                    ropeThis.GetComponent<ConstraintRemover>().fixedJoint2D = fixJoint;

                    ropeThis.transform.SetParent(first.transform);
                }
                break;
            case 7:
                if (!first.transform.CompareTag("Bound") && !first.transform.root.CompareTag("Bound"))
                {
                    fixJoint = first.AddComponent<FixedJoint2D>();
                    if (second != null && second != first)
                        fixJoint.connectedBody = second.GetComponent<Rigidbody2D>();
                    else
                        fixJoint.connectedBody = GlobalSetting.defaultConstraintBody.GetComponent<Rigidbody2D>();
                    fixJoint.anchor = firstPosBegin;
                    fixJoint.breakForce = 10000;
                    if (second != null && second != first)
                    {
                        fixJoint.connectedAnchor = secondPos;
                    }
                    else
                    {
                        fixJoint.connectedAnchor = GlobalSetting.defaultConstraintBody.transform.InverseTransformPoint(mainCamera.ScreenToWorldPoint(Input.mousePosition));
                    }
                    ropeThis = Instantiate(constraints[5]);
                    ropeThis.GetComponent<FixedRopeRender>().parent1 = first;
                    ropeThis.GetComponent<FixedRopeRender>().localPos1 = firstPosBegin;
                    if (second != null && second != first)
                    {
                        ropeThis.GetComponent<FixedRopeRender>().parent3 = second;
                        ropeThis.GetComponent<FixedRopeRender>().localPos3 = secondPos;
                        fixJoint.connectedAnchor = secondPos;
                        ropeThis.GetComponent<ConstraintRemover>().constraintLocalPos = second.transform.InverseTransformPoint(mainCamera.ScreenToWorldPoint(Input.mousePosition));
                    }
                    else
                    {
                        ropeThis.GetComponent<FixedRopeRender>().parent3 = GlobalSetting.defaultConstraintBody;
                        ropeThis.GetComponent<FixedRopeRender>().localPos3 = GlobalSetting.defaultConstraintBody.transform.InverseTransformPoint(mainCamera.ScreenToWorldPoint(Input.mousePosition));
                        fixJoint.connectedAnchor = GlobalSetting.defaultConstraintBody.transform.InverseTransformPoint(mainCamera.ScreenToWorldPoint(Input.mousePosition));
                        ropeThis.GetComponent<ConstraintRemover>().constraintLocalPos = first.transform.InverseTransformPoint(mainCamera.ScreenToWorldPoint(Input.mousePosition));
                    }
                    ropeThis.GetComponent<ConstraintRemover>().fixedJoint2D = fixJoint;
                    ropeThis.transform.SetParent(first.transform);
                }
                break;
            case 8:
                trg.SendMessage("Active", SendMessageOptions.DontRequireReceiver);
                break;
            case 9:
                SpringJoint2D springJoint;
                if (!first.transform.CompareTag("Bound") && !first.transform.root.CompareTag("Bound"))
                {
                    springJoint = first.AddComponent<SpringJoint2D>();
                    if (second != null && second != first)
                        springJoint.connectedBody = second.GetComponent<Rigidbody2D>();
                    else
                        springJoint.connectedBody = GlobalSetting.defaultConstraintBody.GetComponent<Rigidbody2D>();
                    springJoint.enableCollision = true;
                    springJoint.anchor = firstPosBegin;
                    springJoint.frequency = 6;
                    ropeThis = Instantiate(constraints[6]);
                    ropeThis.GetComponent<FixedSpringRender>().parent1 = first;
                    ropeThis.GetComponent<FixedSpringRender>().localPos1 = firstPosBegin;
                    if (second != null && second != first)
                    {
                        ropeThis.GetComponent<FixedSpringRender>().parent3 = second;
                        ropeThis.GetComponent<FixedSpringRender>().localPos3 = secondPos;
                        springJoint.connectedAnchor = secondPos;
                    }
                    else
                    {
                        ropeThis.GetComponent<FixedSpringRender>().parent3 = GlobalSetting.defaultConstraintBody;
                        ropeThis.GetComponent<FixedSpringRender>().localPos3 = GlobalSetting.defaultConstraintBody.transform.InverseTransformPoint(mainCamera.ScreenToWorldPoint(Input.mousePosition));
                        springJoint.connectedAnchor = GlobalSetting.defaultConstraintBody.transform.InverseTransformPoint(mainCamera.ScreenToWorldPoint(Input.mousePosition));
                    }
                    if (springJoint.connectedBody == null)
                    {
                        springJoint.connectedAnchor = mainCamera.transform.InverseTransformPoint(mainCamera.ScreenToWorldPoint(Input.mousePosition));
                        springJoint.connectedBody = GlobalSetting.defaultConstraintBody.GetComponent<Rigidbody2D>();
                    }
                    ropeThis.GetComponent<ConstraintRemover>().springJoint2D = springJoint;
                    ropeThis.transform.SetParent(first.transform);
                }
                break;
            case 10:
                RaycastHit2D[] hits;
                bool isPinned;
                if (!first.transform.CompareTag("Bound") && !first.transform.root.CompareTag("Bound"))
                {
                    ropeThis = Instantiate(constraints[7]);
                    ropeThis.transform.position = (Vector3)(Vector2)mainCamera.ScreenToWorldPoint(Input.mousePosition) + new Vector3(0, 0, -3);
                    ropeThis.transform.SetParent(first.transform);
                    hits = Physics2D.RaycastAll(mainCamera.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
                    isPinned = false;
                    foreach (RaycastHit2D ray in hits)
                    {
                        if (ray.collider.gameObject != first)
                        {
                            if (ray.collider.TryGetComponent(out Rigidbody2D r))
                            {
                                if (r.gameObject != mainCamera.gameObject)
                                {
                                    HingeJoint2D hinge = first.AddComponent<HingeJoint2D>();
                                    hinge.connectedBody = ray.collider.gameObject.GetComponent<Rigidbody2D>();
                                    if (snap)
                                        hinge.anchor = Snap(first.transform.InverseTransformPoint(mainCamera.ScreenToWorldPoint(Input.mousePosition)));
                                    else
                                        hinge.anchor = first.transform.InverseTransformPoint(mainCamera.ScreenToWorldPoint(Input.mousePosition));
                                    if (snap)
                                        hinge.connectedAnchor = Snap(ray.collider.transform.InverseTransformPoint(mainCamera.ScreenToWorldPoint(Input.mousePosition)));
                                    else
                                        hinge.connectedAnchor = ray.collider.transform.InverseTransformPoint(mainCamera.ScreenToWorldPoint(Input.mousePosition));
                                    ropeThis.GetComponent<ConstraintRemover>().hingeJoint2D = hinge;
                                    isPinned = true;
                                    break;
                                }
                            }
                        }
                    }
                    if (!isPinned)
                    {
                        HingeJoint2D hinge = first.AddComponent<HingeJoint2D>();
                        hinge.enableCollision = true;
                        hinge.connectedBody = GlobalSetting.defaultConstraintBody.GetComponent<Rigidbody2D>();
                        hinge.anchor = first.transform.InverseTransformPoint(mainCamera.ScreenToWorldPoint(Input.mousePosition));
                        hinge.connectedAnchor = mainCamera.transform.InverseTransformPoint(mainCamera.ScreenToWorldPoint(Input.mousePosition));
                        ropeThis.GetComponent<ConstraintRemover>().hingeJoint2D = hinge;
                        isPinned = true;
                    }
                }
                break;
            case 11:
                if (!first.transform.CompareTag("Bound") && !first.transform.root.CompareTag("Bound"))
                {
                    ropeThis = Instantiate(constraints[8]);
                    ropeThis.transform.position = (Vector3)(Vector2)mainCamera.ScreenToWorldPoint(Input.mousePosition) + new Vector3(0, 0, -3);
                    ropeThis.transform.SetParent(first.transform);
                    hits = Physics2D.RaycastAll(mainCamera.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
                    isPinned = false;
                    foreach (RaycastHit2D ray in hits)
                    {
                        if (ray.collider.gameObject != first)
                        {
                            if (ray.collider.TryGetComponent(out Rigidbody2D r))
                            {
                                if (r.gameObject != mainCamera.gameObject)
                                {
                                    HingeJoint2D hinge = first.AddComponent<HingeJoint2D>();
                                    hinge.connectedBody = ray.collider.gameObject.GetComponent<Rigidbody2D>();
                                    if (snap)
                                        hinge.anchor = Snap(first.transform.InverseTransformPoint(mainCamera.ScreenToWorldPoint(Input.mousePosition)));
                                    else
                                        hinge.anchor = first.transform.InverseTransformPoint(mainCamera.ScreenToWorldPoint(Input.mousePosition));
                                    if (snap)
                                        hinge.connectedAnchor = Snap(ray.collider.transform.InverseTransformPoint(mainCamera.ScreenToWorldPoint(Input.mousePosition)));
                                    else
                                        hinge.connectedAnchor = ray.collider.transform.InverseTransformPoint(mainCamera.ScreenToWorldPoint(Input.mousePosition));
                                    hinge.breakForce = 10000;
                                    ropeThis.GetComponent<ConstraintRemover>().hingeJoint2D = hinge;
                                    isPinned = true;
                                    break;
                                }
                            }
                        }
                    }
                    if (!isPinned)
                    {
                        HingeJoint2D hinge = first.AddComponent<HingeJoint2D>();
                        hinge.enableCollision = true;
                        hinge.connectedBody = GlobalSetting.defaultConstraintBody.GetComponent<Rigidbody2D>();
                        hinge.anchor = first.transform.InverseTransformPoint(mainCamera.ScreenToWorldPoint(Input.mousePosition));
                        hinge.connectedAnchor = mainCamera.transform.InverseTransformPoint(mainCamera.ScreenToWorldPoint(Input.mousePosition));
                        hinge.breakForce = 10000;
                        ropeThis.GetComponent<ConstraintRemover>().hingeJoint2D = hinge;
                        isPinned = true;
                    }
                }
                break;
            case 12:
                if (trg != null && trg.TryGetComponent(out Properties prop))
                {
                    trg.GetComponent<Properties>().levitate = true;
                    if (thisBall != null)
                    {
                        if (thisBall.GetComponent<RotateObject>().trg != trg)
                        {
                            Destroy(thisBall);
                            thisBall = Instantiate(rotationBall, GlobalSetting.mainCamera.transform.GetChild(0));
                            thisBall.transform.SetAsFirstSibling();
                            thisBall.GetComponent<RotateObject>().trg = trg;
                        }
                    }
                    else
                    {
                        thisBall = Instantiate(rotationBall, GlobalSetting.mainCamera.transform.GetChild(0));
                        thisBall.transform.SetAsFirstSibling();
                        thisBall.GetComponent<RotateObject>().trg = trg;
                    }
                }
                break;
        }
    }

    Vector3 Snap(Vector3 vec)
    {
        return new Vector3(Mathf.Round(vec.x * snapScale) / snapScale, Mathf.Round(vec.y * snapScale) / snapScale, Mathf.Round(vec.z * snapScale) / snapScale);
    }

    Vector2 lastTouchPos;
    private bool IsPointerOverUIObject()
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        if (Input.touchCount > 0)
        {
            eventDataCurrentPosition.position = new Vector2(Input.GetTouch(0).position.x, Input.GetTouch(0).position.y);
            lastTouchPos = eventDataCurrentPosition.position;
        }
        else
        {
            eventDataCurrentPosition.position = lastTouchPos;
        }
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        return results.Count > 0;
    }
}
