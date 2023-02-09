using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class ContextMenu : MonoBehaviour
{
    public MenuButton[] buttons;
    public MenuButton IgniteButton;
    public MenuButton TakeOffButton;
    public MenuButton[] buttonsForLiving;
    public GameObject defaultButton;
    public GameObject trg;
    public GameObject block;
    public GameObject copyContainer;
    public GameObject origin;
    public GameObject resizeCross;
    public float lerpTime;
    public bool isClosing = false;
    float timeStartedLerping;
    GameObject copyContainerPrivate;
    CameraMovementForPhone cameraMovementForPhone;
    Vector2 targetScale;
    Vector2 startScale;
    Vector3 lastCamPos;
    Transform container;
    Camera mainCamera;

    public GameObject FreezeEffect;
    public GameObject UnfreezeEffect;
    public GameObject GravityOn;
    public GameObject GravityOff;
    public GameObject DeleteEffect;
    public GameObject DisableCollisonEffect;
    public GameObject EnableCollisonEffect;

    private void Start()
    {
        mainCamera = GlobalSetting.mainCamera;
        cameraMovementForPhone = mainCamera.GetComponent<CameraMovementForPhone>();
        timeStartedLerping = Time.unscaledTime;
        startScale = new Vector2(0, 0);
        container = transform.GetChild(0).transform.GetChild(0);
        RectTransform canvasRect = transform.parent.GetComponent<RectTransform>();
        Vector2 screenBounds = canvasRect.sizeDelta / 2;
        RectTransform rec = GetComponent<RectTransform>();
        targetScale = new Vector2(rec.sizeDelta.x, Mathf.Clamp(buttons.Length, 0, 4) * 145);
        rec.sizeDelta = new Vector2(0.01f, 0.01f);
        rec.anchoredPosition = new Vector2(Mathf.Clamp(rec.anchoredPosition.x, -screenBounds.x + targetScale.x / 2, screenBounds.x - targetScale.x / 2),
                Mathf.Clamp(rec.anchoredPosition.y, -screenBounds.y + targetScale.y / 2f, screenBounds.y - targetScale.y / 2));
        lastCamPos = mainCamera.transform.position;
        transform.SetSiblingIndex(transform.parent.childCount - 2);
        AddButtons(buttons);
        if (trg.TryGetComponent(out Properties prop) && prop.canBurn)
        {
            AddButtons(new MenuButton[] {IgniteButton});
        }
        /*if (trg.TryGetComponent(out Wear wear) && wear.weared)
        {
            AddButtons(new MenuButton[] {TakeOffButton});
        }*/

        if (Input.GetTouch(0).deltaPosition.magnitude > 0 || trg.CompareTag("EasterEgg"))
        {
            Close();
        }
    }

    public void AddButtons(MenuButton[] buttons)
    {
        foreach (MenuButton button in buttons)
        {
            GameObject thisButton = Instantiate(defaultButton);
            thisButton.transform.SetParent(container);
            RectTransform thisButtonRectTransform = thisButton.GetComponent<RectTransform>();
            thisButtonRectTransform.anchoredPosition = new Vector3(0f, 0f, 0f);
            thisButtonRectTransform.offsetMax = new Vector2(0f, thisButtonRectTransform.offsetMax.y);
            thisButtonRectTransform.offsetMin = new Vector2(0f, thisButtonRectTransform.offsetMin.y);
            thisButton.transform.localScale = new Vector3(1f, 1f, 1f);
            Transform buttonText = thisButton.transform.GetChild(0);
            buttonText.GetComponent<ToggleText>().textOff = button.textOff;
            buttonText.GetComponent<ToggleText>().textOn = button.textOn;
            thisButton.GetComponent<Button>().onClick.AddListener(button.func.Invoke);
            thisButton.name = button.name;
            if (button.checkFunc != null)
                button.checkFunc.Invoke();
        }
    }

    private void Update()
    {
        if (Vector3.Magnitude(mainCamera.transform.position - lastCamPos) > 0 && !cameraMovementForPhone.follow)
            Close();
        else if (Vector3.Magnitude(mainCamera.transform.position - lastCamPos) > 1 && cameraMovementForPhone.follow)
            Close();
        lastCamPos = mainCamera.transform.position;

        RectTransform rec = GetComponent<RectTransform>();
        rec.sizeDelta = Lerp(startScale, targetScale, timeStartedLerping, lerpTime);
        if ((rec.sizeDelta - targetScale).magnitude < 0.001f)
            rec.sizeDelta = targetScale;
        if (rec.sizeDelta.magnitude == 0)
            Destroy(gameObject);
        if (trg == null)
        {
            Close();
        }
        if (isClosing && targetScale != new Vector2(0, 0))
        {
            timeStartedLerping = Time.unscaledTime;
            startScale = GetComponent<RectTransform>().sizeDelta;
            targetScale = new Vector2(0f, 0f);
        }
    }

    public void Close()
    {
        if (!isClosing)
        {
            timeStartedLerping = Time.unscaledTime;
            startScale = GetComponent<RectTransform>().sizeDelta;
            targetScale = new Vector2(0f, 0f);
            isClosing = true;
        }
    }
    public void Parent()
    {
        if (origin != null)
        {
            if (origin.TryGetComponent(out Rigidbody2D b)) //Parent
            {
                //RemoveConstrains(origin);
                foreach (GameObject GO in cameraMovementForPhone.selectedObjects)
                {
                    if (GO.transform.root.gameObject != trg && !GO.transform.root.CompareTag("Ragdoll"))
                    {
                        if (GO.transform.root.TryGetComponent(out Rigidbody2D r))
                        {
                            RemoveConstrains(GO.transform.root.gameObject);
                            GO.transform.root.gameObject.AddComponent<RigidBodyData>().Remember();
                            Destroy(GO.transform.root.GetComponent<Rigidbody2D>());
                            GO.transform.root.transform.SetParent(trg.transform);
                        }
                    }
                }
            }
            else if (origin.TryGetComponent(out RigidBodyData d)) //Unparent
            {
                if (cameraMovementForPhone.selectedObjects.Count <= 1)
                {
                    RemoveConstrains(trg);
                    origin.transform.SetParent(null);
                    Rigidbody2D rigidbody = origin.AddComponent<Rigidbody2D>();
                    rigidbody.mass = d.mass;
                    rigidbody.centerOfMass = d.centreOfMass;
                    Destroy(d);
                }
                else
                {
                    RemoveConstrains(trg);
                    foreach (GameObject GO in cameraMovementForPhone.selectedObjects)
                    {
                        if (GO.TryGetComponent(out RigidBodyData r))
                        {
                            RemoveConstrains(GO);
                            GO.transform.SetParent(null);
                            Rigidbody2D rigidbody = GO.AddComponent<Rigidbody2D>();
                            rigidbody.mass = d.mass;
                            rigidbody.centerOfMass = d.centreOfMass;
                            Destroy(r);
                        }
                    }
                }
            }
        }
        Close();
    }
    void RemoveConstrains(GameObject G)
    {
        var ropes = G.GetComponents<DistanceJoint2D>();
        foreach (DistanceJoint2D co in ropes)
        {
            Destroy(co);
        }
        var fixropes = G.GetComponents<FixedJoint2D>();
        foreach (FixedJoint2D co in fixropes)
        {
            Destroy(co);
        }
        var sliders = G.GetComponents<SliderJoint2D>();
        foreach (SliderJoint2D co in sliders)
        {
            Destroy(co);
        }
        var hinges = G.GetComponents<HingeJoint2D>();
        foreach (HingeJoint2D co in hinges)
        {
            Destroy(co);
        }
    }
    public void ParentCheck()
    {
        GameObject button = GameObject.Find("Parent");
        if (origin != null)
            button.GetComponentInChildren<ToggleText>().toggle = origin.TryGetComponent(out RigidBodyData o);
        else
            button.GetComponentInChildren<ToggleText>().toggle = trg.TryGetComponent(out RigidBodyData r);
    }
    public void Copy()
    {
        cameraMovementForPhone.CleanSelected();
        GameObject container;        

        if (cameraMovementForPhone.selectedObjects.Count > 0 && cameraMovementForPhone.selectedObjects.Find(x => x.transform.gameObject == trg))
        {
            List<GameObject> copyRoots = new List<GameObject>();
            if (GameObject.Find("CopyContainer"))
            {
                Destroy(GameObject.Find("CopyContainer"));
            }
            copyContainerPrivate = Instantiate(copyContainer, (Vector2)mainCamera.ScreenToWorldPoint(Input.mousePosition), new Quaternion());
            GameObject tempGO = Instantiate(copyContainer, (Vector2)mainCamera.ScreenToWorldPoint(Input.mousePosition), new Quaternion());
            tempGO.name = "PasteContainer";
            foreach (GameObject GO in cameraMovementForPhone.selectedObjects)
            {
                if (GO.transform.root != null && GO.transform.root != copyContainerPrivate.transform && !copyRoots.Find(x => x.transform.root.GetInstanceID() == GO.transform.root.GetInstanceID()))
                {
                    GO.transform.root.SetParent(tempGO.transform);
                    copyRoots.Add(GO.transform.root.gameObject);
                }
            }

            tempGO.transform.SetParent(copyContainerPrivate.transform);
            copyContainerPrivate.name = "CopyContainer";
            container = copyContainerPrivate;

            tempGO.SetActive(false);
            GameObject clone = Instantiate(copyContainerPrivate);

            clone.transform.GetChild(0).gameObject.SetActive(true);
            var propertiesInSelectedObjects = clone.transform.root.GetComponentsInChildren<Properties>();
            foreach (Properties properties in propertiesInSelectedObjects)
            {
                properties.SetOriginalColliderSize();
                properties.ResizeColliders();
            }
            clone.transform.GetChild(0).gameObject.SetActive(false);

            foreach (Transform GO in tempGO.transform)
            {
                var constraints = GO.GetComponentsInChildren<ConstraintRemover>(); //Remove constraints
                if (constraints.Length > 0)
                {
                    foreach (ConstraintRemover constraint in constraints)
                    {
                        if (constraint.connectedBody.transform.root != GO.root && constraint.connectedBody != GlobalSetting.defaultConstraintBody)
                        {
                            Destroy(constraint.gameObject);
                        }
                    }
                }
            }

            cameraMovementForPhone.selectedObjects.Clear();
        }
        else
        {
            if (GameObject.Find("CopyContainer"))
            {
                Destroy(GameObject.Find("CopyContainer"));
            }
            copyContainerPrivate = Instantiate(copyContainer, (Vector2)mainCamera.ScreenToWorldPoint(Input.mousePosition), new Quaternion());
            GameObject tempGO = Instantiate(copyContainer, (Vector2)mainCamera.ScreenToWorldPoint(Input.mousePosition), new Quaternion());
            tempGO.name = "PasteContainer";

            GameObject temp = Instantiate(trg.transform.root.gameObject, trg.transform.position, trg.transform.rotation);
            temp.transform.SetParent(tempGO.transform);

            var constraints = tempGO.GetComponentsInChildren<ConstraintRemover>();
            if (constraints.Length > 0)
            { 
                foreach (ConstraintRemover constraint in constraints)
                {
                    if (constraint.connectedBody != GlobalSetting.defaultConstraintBody)
                    {
                        Destroy(constraint.gameObject);
                    }
                }
            }

            tempGO.transform.SetParent(copyContainerPrivate.transform);
            copyContainerPrivate.name = "CopyContainer";
            container = copyContainerPrivate;
            tempGO.SetActive(false);
        }

        GlobalSetting.spawnMenu.copy = container;
        Close();
    }
    public void Follow()
    {
        if (!cameraMovementForPhone.follow)
        {
            cameraMovementForPhone.follow = true;
            cameraMovementForPhone.followGO = trg;
        }
        else
        {
            cameraMovementForPhone.follow = false;
            cameraMovementForPhone.followGO = null;
        }
        Close();
    }
    public void FollowCheck()
    {
        GameObject button = GameObject.Find("Follow");
        button.GetComponentInChildren<ToggleText>().toggle = cameraMovementForPhone.follow;
    }
    public void Remove()
    {
        if (trg != null)
        {
            GameObject particle = Instantiate(DeleteEffect, trg.transform.position, trg.transform.rotation);
            particle.transform.localScale = trg.transform.lossyScale;
            var shape = particle.GetComponent<ParticleSystem>().shape;
            shape.texture = trg.GetComponent<SpriteRenderer>().sprite.texture;
            shape.sprite = trg.GetComponent<SpriteRenderer>().sprite;
            particle.GetComponent<ParticleSystem>().Play();
        }

        if (origin != null && origin.CompareTag("Constraint"))
        {
            Destroy(origin);
        }
        else
        {
            if (cameraMovementForPhone.selectedObjects.Count == 0)
            {
                Destroy(trg);
            }
            else
            {
                for (int i = 0; i < cameraMovementForPhone.selectedObjects.Count; ++i)
                {
                    if (cameraMovementForPhone.selectedObjects[i].transform.root != null)
                        Destroy(cameraMovementForPhone.selectedObjects[i]);
                    else
                        Destroy(cameraMovementForPhone.selectedObjects[i]);
                }
            }
            cameraMovementForPhone.selectedObjects.Clear();
        }
        Close();
    }
    public void Mirror()
    {
        if (trg != null)
        {
            GameObject thisSelectContainer = null;
            if (cameraMovementForPhone.selectedObjects.Count > 0)
            {
                thisSelectContainer = Instantiate(cameraMovementForPhone.selectedContainer);
                thisSelectContainer.transform.position = mainCamera.ScreenToWorldPoint(Input.GetTouch(0).position);
                for (int i = 0; i < cameraMovementForPhone.selectedObjects.Count; ++i)
                {
                    cameraMovementForPhone.selectedObjects[i].transform.root.SetParent(thisSelectContainer.transform);
                }
                trg = thisSelectContainer;
            }
            trg.transform.root.localScale = new Vector3(-trg.transform.root.localScale.x, trg.transform.root.localScale.y, trg.transform.root.localScale.z);
            if (thisSelectContainer != null)
            {
                int count = thisSelectContainer.transform.childCount;
                for (int i = 0; i < count; ++i)
                {
                    if (thisSelectContainer.transform.GetChild(0).transform.parent == thisSelectContainer.transform)
                        thisSelectContainer.transform.GetChild(0).SetParent(null);
                }
                Destroy(thisSelectContainer);
            }
        }
        Close();
    }
    public void Freeze()
    {
        var objects = cameraMovementForPhone.selectedObjects;
        if (objects.Count > 0 && objects.Find(x => x.transform.gameObject == trg))
        {
            if (trg.TryGetComponent(out Rigidbody2D trgRigidBody2D))
            {
                if (trgRigidBody2D.constraints == RigidbodyConstraints2D.None)
                {
                    playEffect(FreezeEffect);

                    for (int i = 0; i < objects.Count; ++i)
                    {
                        if (objects[i].TryGetComponent(out Rigidbody2D rb))
                        {
                            rb.constraints = RigidbodyConstraints2D.FreezeAll;
                            rb.velocity = new Vector2(0f, 0f);
                            rb.angularVelocity = 0f;
                        }
                    }
                }
                else
                {
                    playEffect(UnfreezeEffect);

                    for (int i = 0; i < cameraMovementForPhone.selectedObjects.Count; ++i)
                    {
                        if (objects[i].TryGetComponent(out Rigidbody2D rb))
                        {
                            rb.constraints = RigidbodyConstraints2D.None;
                            rb.AddForce(new Vector2(0, 0.0001f));
                        }
                    }
                }
            }
        }
        else
        {
            if (trg.TryGetComponent(out Rigidbody2D trgRigidBody2D))
            {
                if (trgRigidBody2D.constraints == RigidbodyConstraints2D.None)
                {
                    playEffect(FreezeEffect);

                    trgRigidBody2D.constraints = RigidbodyConstraints2D.FreezeAll;
                    trgRigidBody2D.velocity = new Vector2(0f, 0f);
                    trgRigidBody2D.angularVelocity = 0f;
                }
                else
                {
                    playEffect(UnfreezeEffect);

                    trgRigidBody2D.constraints = RigidbodyConstraints2D.None;
                    trgRigidBody2D.AddForce(new Vector2(0, 0.0001f));
                }
            }
        }
        Close();
    }
    public void FreezeCheck()
    {
        GameObject button = GameObject.Find("Freeze");
        if (trg.TryGetComponent(out Rigidbody2D trgRigidBody2D))
            button.GetComponentInChildren<ToggleText>().toggle = !(trgRigidBody2D.constraints == RigidbodyConstraints2D.None);
    }
    public void Active()
    {
        if (cameraMovementForPhone.selectedObjects.Count != 0 && cameraMovementForPhone.selectedObjects.Find(x => x.transform.gameObject == trg))
        {
            for (int i = 0; i < cameraMovementForPhone.selectedObjects.Count; ++i)
            {
                cameraMovementForPhone.selectedObjects[i].SendMessage("Active", SendMessageOptions.DontRequireReceiver);
            }
        }
        else
        {
            trg.SendMessage("Active");
        }
        Close();
    }
    public void Gravity()
    {
        var objects = cameraMovementForPhone.selectedObjects;
        if (objects.Count != 0 && objects.Find(x => x.transform.gameObject == trg))
        {
            if (trg.TryGetComponent(out Properties prop))
            {
                if (prop)
                {
                    playEffect(GravityOff);

                    for (int i = 0; i < objects.Count; ++i)
                    {
                        if (objects[i].TryGetComponent(out Properties propInObj))
                            propInObj.gravity = false;
                    }
                }
                else
                {
                    playEffect(GravityOn);

                    for (int i = 0; i < objects.Count; ++i)
                    {
                        if (objects[i].TryGetComponent(out Properties propInObj))
                            propInObj.gravity = true;
                    }
                }
            }
        }
        else
        {
            if (trg.TryGetComponent(out Properties prop))
            {
                if (prop.gravity)
                {
                    playEffect(GravityOff);

                    prop.gravity = false;
                }
                else
                {
                    playEffect(GravityOn);

                    prop.gravity = true;
                }
            }
        }
        Close();
    }
    public void GravityCheck()
    {
        GameObject button = GameObject.Find("Gravity");
        if (trg.TryGetComponent(out Rigidbody2D trgRigidBody2D))
            button.GetComponentInChildren<ToggleText>().toggle = trgRigidBody2D.gravityScale > 0;
    }
    public void NoCollide()
    {
        if (cameraMovementForPhone.selectedObjects.Count > 0 && cameraMovementForPhone.selectedObjects.Find(x => x.transform.gameObject == trg)) //31 is nocollide layer
        {
            if (trg.layer != 31)
            {
                //playEffect(DisableCollisonEffect);
                for (int i = 0; i < cameraMovementForPhone.selectedObjects.Count; ++i)
                {
                    cameraMovementForPhone.selectedObjects[i].layer = 31;
                }
            }
            else
            {
                //playEffect(EnableCollisonEffect);
                for (int i = 0; i < cameraMovementForPhone.selectedObjects.Count; ++i)
                {
                    cameraMovementForPhone.selectedObjects[i].layer = cameraMovementForPhone.selectedObjects[i].GetComponent<Properties>().layer;
                }
            }
        }
        else
        {
            if (trg.layer != 31)
            {
                //playEffect(DisableCollisonEffect);
                trg.layer = 31;
            }
            else
            {
                //playEffect(EnableCollisonEffect);
                trg.layer = trg.GetComponent<Properties>().layer;
            }
        }
        Close();
    }
    public void NoCollideCheck()
    {
        GameObject button = GameObject.Find("NoCollide");
        button.GetComponentInChildren<ToggleText>().toggle = trg.layer == 31;
    }
    public void Resize()
    {
        if (GlobalSetting.resizer == null)
        {
            GlobalSetting.resizer = Instantiate(resizeCross, GlobalSetting.mainCamera.transform.GetChild(0));
            GlobalSetting.resizer.transform.SetSiblingIndex(GlobalSetting.mainCamera.transform.GetChild(0).childCount - 2);
            GlobalSetting.resizer.GetComponent<SetUIObjectPos>().trgObject = trg;
        }
        else
        {
            if (GlobalSetting.resizer.GetComponent<SetUIObjectPos>().trgObject != trg)
            {
                GlobalSetting.resizer.GetComponent<SetUIObjectPos>().isDetroying = true;
                GlobalSetting.resizer = Instantiate(resizeCross, GlobalSetting.mainCamera.transform.GetChild(0));
                GlobalSetting.resizer.transform.SetSiblingIndex(GlobalSetting.mainCamera.transform.GetChild(0).childCount - 2);
                GlobalSetting.resizer.GetComponent<SetUIObjectPos>().trgObject = trg;
            }
            else
            {
                GlobalSetting.resizer.GetComponent<SetUIObjectPos>().isDetroying = true;
            }
        }
        Close();
    }
    public void ResizeCheck()
    {
        GameObject button = GameObject.Find("Resize");
        if (GlobalSetting.resizer != null)
        {
            button.GetComponentInChildren<ToggleText>().toggle = GlobalSetting.resizer.GetComponent<SetUIObjectPos>().trgObject == trg;
        }
        else
        {
            button.GetComponentInChildren<ToggleText>().toggle = false;
        }
    }
    public void Ignite()
    {
        if (!trg.GetComponent<Properties>().isOnFire)
        {
            if (cameraMovementForPhone.selectedObjects.Count != 0 && cameraMovementForPhone.selectedObjects.Find(x => x.transform.gameObject == trg))
            {
                for (int i = 0; i < cameraMovementForPhone.selectedObjects.Count; ++i)
                {
                    if (cameraMovementForPhone.selectedObjects[i].TryGetComponent(out Properties prop))
                    {
                        if (!prop.isOnFire)
                            prop.Ignite();
                    }
                }
            }
            else
            {
                trg.GetComponent<Properties>().Ignite();
            }
        }
        else
        {
            if (cameraMovementForPhone.selectedObjects.Count != 0 && cameraMovementForPhone.selectedObjects.Find(x => x.transform.gameObject == trg))
            {
                for (int i = 0; i < cameraMovementForPhone.selectedObjects.Count; ++i)
                {
                    if (cameraMovementForPhone.selectedObjects[i].TryGetComponent(out Properties prop))
                    {
                        if (prop.isOnFire)
                            prop.Extinguish();
                    }
                }
            }
            else
            {
                trg.GetComponent<Properties>().Extinguish();
            }
        }
        Close();
    }
    public void IgniteCheck()
    {
        GameObject button = GameObject.Find("Ignite");
        button.GetComponentInChildren<ToggleText>().toggle = trg.GetComponent<Properties>().isOnFire;
    }
    public void ActivationToggle()
    {
        if (trg.TryGetComponent(out Gun gun))
        {
            if (gun.toggleActivation)
                gun.toggleActivation = false;
            else
                gun.toggleActivation = true;
        }
        if (trg.TryGetComponent(out ItemThrower thrower))
        {
            if (thrower.toggleActivation)
                thrower.toggleActivation = false;
            else
                thrower.toggleActivation = true;
        }
        if (trg.TryGetComponent(out Spray spray))
        {
            if (spray.toggleActivation)
                spray.toggleActivation = false;
            else
                spray.toggleActivation = true;
        }
        Close();
    }
    public void ActivationToggleCheck()
    {
        bool canBeToggle = false;
        if (trg.TryGetComponent(out Gun gun))
        {
            GameObject button = GameObject.Find("ActivationToggle");
            button.GetComponentInChildren<ToggleText>().toggle = gun.toggleActivation;
            canBeToggle = true;
        }
        if (trg.TryGetComponent(out ItemThrower thrower))
        {
            GameObject button = GameObject.Find("ActivationToggle");
            button.GetComponentInChildren<ToggleText>().toggle = thrower.toggleActivation;
            canBeToggle = true;
        }
        if (trg.TryGetComponent(out Spray spray))
        {
            GameObject button = GameObject.Find("ActivationToggle");
            button.GetComponentInChildren<ToggleText>().toggle = spray.toggleActivation;
            canBeToggle = true;
        }
        if (!canBeToggle)
        {
            Destroy(GameObject.Find("ActivationToggle"));
        }
    }

    //Human methods
    public void Sit()
    {
        if (cameraMovementForPhone.selectedObjects.Count == 0)
        {
            trg.transform.root.GetComponent<Animator>().SetTrigger("Sit");
        }
        else
        {
            for (int i = 0; i < cameraMovementForPhone.selectedObjects.Count; ++i)
            {
                if (cameraMovementForPhone.selectedObjects[i].transform.root.TryGetComponent(out Animator animator))
                    animator.SetTrigger("Sit");
            }
        }
        Close();
    }
    public void Walk()
    {
        if (cameraMovementForPhone.selectedObjects.Count == 0)
        {
            trg.transform.root.GetComponent<Animator>().SetTrigger("WalkPermanently");
        }
        else
        {
            for (int i = 0; i < cameraMovementForPhone.selectedObjects.Count; ++i)
            {
                if (cameraMovementForPhone.selectedObjects[i].transform.root.TryGetComponent(out Animator animator))
                    animator.SetTrigger("WalkPermanently");
            }
        }
        Close();
    }
    public void StopAnimations()
    {
        trg.transform.root.GetComponent<Animator>();
        if (cameraMovementForPhone.selectedObjects.Count == 0)
        {
            trg.transform.root.GetComponent<Animator>().SetTrigger("StopAnimation");
        }
        else
        {
            for (int i = 0; i < cameraMovementForPhone.selectedObjects.Count; ++i)
            {
                if (cameraMovementForPhone.selectedObjects[i].transform.root.TryGetComponent(out Animator animator))
                    animator.SetTrigger("StopAnimation");
            }
        }
        Close();
    }

    Vector3 Lerp(Vector3 startPos, Vector3 endPos, float timeStartLerping, float lerpTime = 1f)
    {
        float timeSinceStarted = Time.unscaledTime - timeStartLerping;

        float percentageComplete = timeSinceStarted / lerpTime;

        Vector3 result = Vector3.Lerp(startPos, endPos, Mathf.SmoothStep(0, 1, percentageComplete));

        return result;
    }
    void playEffect(GameObject effect)
    {
        GameObject thisParticle = Instantiate(effect, trg.transform.position, new Quaternion());
        var mainModule = thisParticle.GetComponent<ParticleSystem>().main;
        mainModule.useUnscaledTime = true;
        var shape = thisParticle.GetComponent<ParticleSystem>().shape;
        shape.sprite = trg.GetComponent<SpriteRenderer>().sprite;
        shape.scale = trg.transform.localScale;
        shape.rotation = new Vector3(0, 0, trg.transform.eulerAngles.z);
        thisParticle.GetComponent<ParticleSystem>().Play();
    }
}

[System.Serializable]
public class MenuButton
{
    public string name;
    public string textOn;
    public string textOff;
    public UnityEvent func;
    public UnityEvent checkFunc;
}
