using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CameraMovementForPhone : MonoBehaviour
{
    public Toggle snapToggle;
    public Toggle dragToggle;
    public Slider shootingSlider;
    public GameObject menu;
    public GameObject mainCanvas;
    public GameObject followGO;
    public GameObject selectZone;
    public GameObject selectedContainer;
    public GameObject UnfollowMenu;
    public GameObject capturedSelectZone;
    public GameObject GOtarget;
    public Vector2 maxPos;
    public List<GameObject> selectedObjects;
    public List<UISmoothSlide> activationButtons;
    public ConstraintController constraintController;
    public LayerMask grabMask;
    public float minZoom = 0.5f;
    public float maxZoom = 10f;
    public float scrollSensitivity = 0.1f;
    public float rotationSpeed = 0;
    public float snapScale = 4;
    public float rotationSnapScale = 4;
    public bool follow = false;
    public bool snap;

    Camera mainCamera;
    GameObject thisSelectContainer;
    GameObject lastGO;
    TargetJoint2D trgJoint; //For drag
    Rigidbody2D trgRigidbody2D;
    Vector3 mousePos;
    Vector3 mouseDifference;
    Vector2 capturedSelectZonePos;
    RaycastHit2D hit;
    int fingerId = 0;
    float angleDifference; //For object rotation
    float trgAngle; //For object rotation
    float tapRate = 0;
    bool startRotating = false; //For object rotation
    bool isRotating = false; //For object rotation
    bool isDraging = false;
    bool wasDragged = false;
    bool canDrag = false;
    bool canRotate = false;
    bool wasRotated = false;
    bool justStopped = false;

    private void Start()
    {
        mainCamera = Camera.main;
        mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
    }

    public void SetScrollSensitivity()
    {
        scrollSensitivity = GlobalSetting.settings.ZoomSensitivity;
    }

    void Update()
    {
        if (tapRate > 10 && lastGO == GOtarget)
        {
            ContextMenuOpen();
            tapRate = 0;
        }
        else if (tapRate > 0)
        {
            tapRate -= Time.unscaledDeltaTime * 20;
        }
        else
        {
            tapRate = 0;
        }

        snap = snapToggle.isOn;
        UISmoothSlide slide = UnfollowMenu.GetComponent<UISmoothSlide>();
        if (GOtarget != null && startRotating)
            trgRigidbody2D.MoveRotation(trgAngle);

        if (follow)
        {
            if (slide.target == slide.startPos)
                slide.Open();
            Follow();
        }
        else
        {
            if (slide.target == slide.targetPos + slide.startPos)
                slide.Close();
        }
        if (Input.touchCount == 0) 
        {
            if (isDraging && GOtarget != null) //Garbage clean
            {
                isRotating = false;
                startRotating = false;
                var trgJoints = GOtarget.GetComponents<TargetJoint2D>();
                for (int i = 0; i < trgJoints.Length; ++i)
                {
                    Destroy(trgJoints[i]);
                }
            }
            if (justStopped)
            {
                ResetValues();
                justStopped = false;
            }
        }
        else if(Input.touchCount == 2)
        {
            justStopped = true;

            if (!isDraging)
            {
                Zoom();
            }
            else
            {
                if (!IsPointerOverUIObject(1))
                {
                    if (!isRotating)
                    {
                        canRotate = true;
                        wasRotated = true;
                    }
                }
                if (IsPointerOverUIObject(1))
                {
                    MoveObject();
                }
                if (canRotate)
                {
                    if (Time.timeScale != 0)
                        Rotate();
                    else
                    {
                        if (selectedObjects.Count > 0 && thisSelectContainer == null && selectedObjects.Find(x => x == GOtarget))
                        {
                            thisSelectContainer = Instantiate(selectedContainer);
                            thisSelectContainer.transform.position = mainCamera.ScreenToWorldPoint(Input.mousePosition);
                            for (int i = 0; i < selectedObjects.Count; ++i)
                            {
                                selectedObjects[i].transform.root.SetParent(thisSelectContainer.transform);
                            }
                            GOtarget = thisSelectContainer;
                        }
                        if (thisSelectContainer != null)
                        {
                            GOtarget = thisSelectContainer;
                        }
                        RotatePaused();
                    }
                }
            }
        }
        else if (Input.touchCount == 1)
        {
            if (Input.GetTouch(0).fingerId != fingerId)
            {
                fingerId = Input.GetTouch(0).fingerId;
                ResetValues();
            }
            justStopped = true;
            if (wasRotated)
            {
                canRotate = false;
                wasRotated = false;
            }
            if (GOtarget != null && GOtarget.transform.CompareTag("Weapon") && trgRigidbody2D.constraints != RigidbodyConstraints2D.FreezeAll)
            {
                if (GOtarget.TryGetComponent(out Gun gun))
                    gun.isShooting = false;
                if (GOtarget.TryGetComponent(out Spray spray))
                    spray.isShooting = false;
                if (GOtarget.TryGetComponent(out ItemThrower thrower))
                    thrower.isShooting = false;
                shootingSlider.GetComponent<UISmoothSlide>().Close();
            }
            isRotating = false;
            foreach (Touch touch in Input.touches)
            {
                if (touch.phase == TouchPhase.Began)
                {
                    Collider2D coll = null;
                    var hits = Physics2D.RaycastAll(mainCamera.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
                    if (hits.Length > 0 && !hits[hits.Length - 1].collider.CompareTag("Water") && !hits[hits.Length - 1].collider.CompareTag("Bound"))
                    {
                        if (!hits[hits.Length - 1].collider.CompareTag("Water"))
                        {
                            hit = hits[hits.Length - 1];
                            coll = hit.collider;
                        }
                        else if (hits.Length > 1)
                        {
                            hit = hits[hits.Length - 2];
                            coll = hit.collider;
                        }
                        else
                        {
                            hit = new RaycastHit2D();
                        }
                    }
                    else
                    {
                        coll = Physics2D.OverlapCircle(mainCamera.ScreenToWorldPoint(Input.mousePosition), 0.08f, grabMask);
                    }

                    if (thisSelectContainer != null && coll != null && (coll.gameObject.TryGetComponent(out Rigidbody2D b)) && selectedObjects.Find(x => x.transform.gameObject == coll.gameObject))
                    {
                        int count = thisSelectContainer.transform.childCount;
                        for (int i = 0; i < count; ++i)
                        {
                            if (thisSelectContainer.transform.GetChild(0).transform.parent == thisSelectContainer.transform)
                                thisSelectContainer.transform.GetChild(0).SetParent(null);
                        }
                        thisSelectContainer.transform.position = mainCamera.ScreenToWorldPoint(Input.mousePosition);
                        for (int i = 0; i < selectedObjects.Count; ++i)
                        {
                            selectedObjects[i].transform.root.SetParent(thisSelectContainer.transform);
                        }
                        GOtarget = thisSelectContainer;
                    }
                    mousePos = mainCamera.ScreenToWorldPoint(Input.GetTouch(Input.touchCount - 1).position);

                    if (!IsPointerOverUIObject())
                    {
                        canDrag = true;
                        if (coll != null)
                        {
                            if (coll.gameObject.TryGetComponent(out Rigidbody2D a) && !a.isKinematic) //Add TargetJoint to object
                            {
                                StartDragging(coll);
                                if (selectedObjects.Count == 0 || (selectedObjects.Count > 0 && !selectedObjects.Find(x => x == GOtarget)))
                                {
                                    mouseDifference = mainCamera.ScreenToWorldPoint(Input.mousePosition) - GOtarget.transform.position;
                                }
                            }
                            else if (coll.gameObject.TryGetComponent(out RigidBodyData d))
                            {
                                if (coll.transform.root.gameObject.TryGetComponent(out Rigidbody2D r))
                                    StartDragging(coll.transform.root.gameObject);
                                else if (coll.transform.root.GetChild(0).gameObject.TryGetComponent(out Rigidbody2D rig))
                                    StartDragging(coll.transform.root.GetChild(0).gameObject);
                            }
                            else if (coll.transform.parent != null && coll.transform.parent.TryGetComponent(out Rigidbody2D bd) && !bd.isKinematic)
                            {
                                StartDragging(coll.transform.parent.gameObject);
                                if (selectedObjects.Count == 0 || (selectedObjects.Count > 0 && !selectedObjects.Find(x => x == GOtarget)))
                                {
                                    mouseDifference = mainCamera.ScreenToWorldPoint(Input.mousePosition) - GOtarget.transform.position;
                                }
                            }
                        }
                        

                        if (GOtarget != null && GOtarget.GetComponent<Properties>().canBeActivated && (constraintController.selectedID == 0 || constraintController.selectedID == 12))
                        {
                            foreach (UISmoothSlide slider in activationButtons)
                            {
                                slider.Open();
                            }
                        }
                    }
                    else
                    {
                        canDrag = false;
                    }
                }
                if (touch.phase == TouchPhase.Stationary)
                {
                    if (canDrag && !wasDragged)
                    {
                        StartCoroutine(DelayCoroutine("SelectZoneOpen", 0.2f, mainCamera.ScreenToWorldPoint(Input.mousePosition)));
                    }
                }
                if (touch.phase == TouchPhase.Moved)
                {
                    if (GOtarget != null && (constraintController.selectedID == 0 || constraintController.selectedID == 12)) //Move object
                    {
                        MoveObject();
                    }
                    else if (canDrag && GOtarget == null) //Move camera
                    {
                        if (!follow && dragToggle.isOn)
                            mainCamera.transform.position += mousePos - mainCamera.ScreenToWorldPoint(Input.GetTouch(0).position);
                        if (Vector3.Magnitude((Vector2)mainCamera.WorldToScreenPoint(mousePos) - Input.GetTouch(0).position) > 0)
                            wasDragged = true;
                    }
                    else if (capturedSelectZone != null)
                    {
                        RectTransform capturedSelectZoneRect = capturedSelectZone.GetComponent<RectTransform>();
                        if (((Vector2)Input.mousePosition - capturedSelectZonePos).x > 0)
                        {
                            if (((Vector2)Input.mousePosition - capturedSelectZonePos).y > 0)
                                capturedSelectZoneRect.pivot = new Vector2(0, 0);
                            else
                                capturedSelectZoneRect.pivot = new Vector2(0, 1);
                        }
                        else
                        {
                            if (((Vector2)Input.mousePosition - capturedSelectZonePos).y > 0)
                                capturedSelectZoneRect.pivot = new Vector2(1, 0);
                            else
                                capturedSelectZoneRect.pivot = new Vector2(1, 1);
                        }
                        capturedSelectZoneRect.sizeDelta = Abs(mainCamera.ScreenToViewportPoint((Vector2)Input.mousePosition - capturedSelectZonePos) * mainCanvas.GetComponent<RectTransform>().sizeDelta) / 3;
                        GetObjectsInSelectZone();
                    }
                }
                if (touch.phase == TouchPhase.Ended)
                {
                    ResetValues();
                }
                if (!wasDragged && isDraging)
                {
                    StartCoroutine(DelayCoroutine("ContextMenuOpen", 0.2f, mainCamera.ScreenToWorldPoint(Input.mousePosition)));
                }

                mousePos = mainCamera.ScreenToWorldPoint(Input.GetTouch(0).position);
            }
        }
        if (selectedObjects.Count == 0 && thisSelectContainer != null)
        {
            int count = thisSelectContainer.transform.childCount;
            for (int i = 0; i < count; ++i)
            {
                if (thisSelectContainer.transform.GetChild(0).transform.parent == thisSelectContainer.transform)
                    thisSelectContainer.transform.GetChild(0).SetParent(null);
            }
            Destroy(thisSelectContainer);
        }
        for (int i = 0; i < selectedObjects.Count; ++i)
        {
            if (selectedObjects[i] == null)
                selectedObjects.RemoveAt(selectedObjects.FindIndex(x => x == null));
        }
        if (GOtarget == null)
            shootingSlider.GetComponent<UISmoothSlide>().Close();

        transform.position = new Vector3(Mathf.Clamp(transform.position.x, -mainCamera.orthographicSize, maxPos.x), Mathf.Clamp(transform.position.y, -mainCamera.orthographicSize, maxPos.y), transform.position.z);
    }

    IEnumerator DelayCoroutine(string func, float delay, Vector3 pos)
    {
        Vector3 startPos = pos;
        yield return new WaitForSecondsRealtime(delay);
        if ((constraintController.selectedID == 0 || constraintController.selectedID == 12))
        {
            if (Vector3.Magnitude(mainCamera.ScreenToWorldPoint(Input.mousePosition) - startPos) == 0 && !follow)
            {
                SendMessage(func);
            }
            else if (Vector3.Magnitude(mainCamera.ScreenToWorldPoint(Input.mousePosition) - startPos) < 0.01f && follow)
            {
                SendMessage(func);
            }
        }
    }

    void MoveObject()
    {
        wasDragged = true;
        if (Time.timeScale != 0 && trgJoint != null) //Move object
        {
            if (snap)
                trgJoint.target = new Vector2(Mathf.Round(mainCamera.ScreenToWorldPoint(Input.GetTouch(0).position).x * snapScale) / snapScale,
                    Mathf.Round(mainCamera.ScreenToWorldPoint(Input.GetTouch(0).position).y * snapScale) / snapScale);
            else
                trgJoint.target = mainCamera.ScreenToWorldPoint(Input.GetTouch(0).position);
            mainCanvas.GetComponent<MenusCloser>().CloseMenus(0);
        }
        else //Move object paused
        {
            if (GOtarget != null)
            {
                Debug.Log(GOtarget.name);
                if (selectedObjects.Count > 0 && thisSelectContainer == null && selectedObjects.Find(x => x == GOtarget))
                {
                    thisSelectContainer = Instantiate(selectedContainer);
                    if (snap)
                        thisSelectContainer.transform.position = new Vector2(Mathf.Round((mainCamera.ScreenToWorldPoint(Input.GetTouch(0).position).x - mouseDifference.x) * snapScale) / snapScale,
                        Mathf.Round((mainCamera.ScreenToWorldPoint(Input.GetTouch(0).position).y - mouseDifference.y) * snapScale) / snapScale);
                    else
                        thisSelectContainer.transform.position = mainCamera.ScreenToWorldPoint(Input.GetTouch(0).position) - mouseDifference;
                    for (int i = 0; i < selectedObjects.Count; ++i)
                    {
                        selectedObjects[i].transform.root.SetParent(thisSelectContainer.transform);
                    }
                    GOtarget = thisSelectContainer;
                }
                if (snap)
                {
                    GOtarget.transform.position = new Vector2(Mathf.Round((mainCamera.ScreenToWorldPoint(Input.GetTouch(0).position).x - mouseDifference.x) * snapScale) / snapScale,
                        Mathf.Round((mainCamera.ScreenToWorldPoint(Input.GetTouch(0).position).y - mouseDifference.y) * snapScale) / snapScale);
                }
                else
                {
                    GOtarget.transform.position = (Vector2)mainCamera.ScreenToWorldPoint(Input.GetTouch(0).position) - (Vector2)mouseDifference;
                }
                if (trgRigidbody2D != null)
                    trgRigidbody2D.velocity = new Vector2(0, 0);
            }
        }
    }
    public void SelectZoneOpen()
    {
        if (canDrag && !wasDragged)
        {
            for (int i = 0; i < selectedObjects.Count; ++i)
            {
                if (selectedObjects[i] != null)
                {
                    if (selectedObjects[i].transform.parent == null)
                        selectedObjects[i].transform.SetParent(null);
                }
                else
                {
                    selectedObjects.RemoveAt(selectedObjects.FindIndex(x => x == null));
                }
            }
            canDrag = false;
            capturedSelectZone = Instantiate(selectZone);
            capturedSelectZone.transform.SetParent(mainCanvas.transform);
            capturedSelectZone.transform.SetSiblingIndex(0);
            capturedSelectZone.GetComponent<RectTransform>().localScale = new Vector3(3, 3, 3);
            capturedSelectZone.GetComponent<RectTransform>().position = Input.mousePosition;
            capturedSelectZonePos = Input.mousePosition;
        }
    }
    public void ContextMenuOpen()
    {
        if (!wasDragged)
        {
            hit = Physics2D.Raycast(mainCamera.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            if (GOtarget != null)
            {
                GameObject contextMenu = Instantiate(menu);
                contextMenu.transform.SetParent(mainCanvas.transform);
                contextMenu.transform.localScale = new Vector3(1f, 1f, 1f);
                contextMenu.transform.position = Input.GetTouch(0).position;
                contextMenu.GetComponent<ContextMenu>().trg = GOtarget;
                if (hit.collider != null)
                {
                    contextMenu.GetComponent<ContextMenu>().origin = hit.collider.gameObject;
                }
            }
            wasDragged = true;
        }   
    }
    void StartDragging(Collider2D coll)
    {
        GOtarget = coll.gameObject;
        if (lastGO != null && lastGO == GOtarget)
            tapRate += 10;
        if (lastGO != null && lastGO != GOtarget)
            tapRate = 10;
        if (Time.timeScale != 0 && (constraintController.selectedID == 0 || constraintController.selectedID == 12))
        {
            trgJoint = GOtarget.AddComponent<TargetJoint2D>();
            trgJoint.anchor = GOtarget.transform.InverseTransformPoint(mainCamera.ScreenToWorldPoint(Input.GetTouch(0).position));
            trgJoint.target = mainCamera.ScreenToWorldPoint(Input.GetTouch(0).position);
            trgJoint.frequency = 20;
        }
        trgRigidbody2D = GOtarget.GetComponent<Rigidbody2D>();
        isDraging = true;
    }
    void StartDragging(GameObject obj)
    {
        GOtarget = obj;
        if (lastGO != null && lastGO == GOtarget)
            tapRate += 10;
        if (lastGO != null && lastGO != GOtarget)
            tapRate = 10;
        if (Time.timeScale != 0 && (constraintController.selectedID == 0 || constraintController.selectedID == 12))
        {
            trgJoint = GOtarget.AddComponent<TargetJoint2D>();
            trgJoint.anchor = GOtarget.transform.InverseTransformPoint(mainCamera.ScreenToWorldPoint(Input.GetTouch(0).position));
            trgJoint.target = mainCamera.ScreenToWorldPoint(Input.GetTouch(0).position);
            trgJoint.frequency = 20;
        }
        trgRigidbody2D = GOtarget.GetComponent<Rigidbody2D>();
        isDraging = true;
    }
    void ResetValues()
    {
        var objects = GameObject.FindGameObjectsWithTag("Ghost");
        foreach (GameObject obj in objects)
        {
            Destroy(obj);
        }
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
        if (GOtarget != null && trgRigidbody2D != null)
        {
            lastGO = GOtarget;
            if (trgRigidbody2D.constraints != RigidbodyConstraints2D.FreezeAll)
                trgRigidbody2D.freezeRotation = false;
            if (GOtarget.transform.CompareTag("Weapon"))
            {
                if (GOtarget.TryGetComponent(out Gun gun))
                    gun.isShooting = false;
                if (GOtarget.TryGetComponent(out ItemThrower thrower))
                    thrower.isShooting = false;
                if (GOtarget.TryGetComponent(out Spray spray))
                    spray.isShooting = false;
                shootingSlider.GetComponent<UISmoothSlide>().Close();
            }
            var trgJoints = GOtarget.GetComponents<TargetJoint2D>();
            for (int i = 0; i < trgJoints.Length; ++i)
            {
                Destroy(trgJoints[i]);
            }
        }
        if (capturedSelectZone != null)
            Destroy(capturedSelectZone);
        for (int i = 0; i < selectedObjects.Count; ++i)
        {
            if (selectedObjects[i] != null)
            {
                if (selectedObjects[i].transform.parent == null)
                    selectedObjects[i].transform.SetParent(null);
            }
        }
        foreach (UISmoothSlide slider in activationButtons)
        {
            slider.Close();
        }

        GOtarget = null;
        canDrag = false;
        isDraging = false;
        isRotating = false;
        startRotating = false;
        wasDragged = false;
        canRotate = false;
        wasRotated = false;
        rotationSpeed = 0;
    }
    void Zoom()
    {
        Touch touchZero = Input.GetTouch(0);
        Touch touchOne = Input.GetTouch(1);

        Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
        Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

        float prevTouchDeltaMag = (touchZeroPrevPos - touchOnePrevPos).magnitude;
        float touchDeltaMag = (touchZero.position - touchOne.position).magnitude;

        float deltaMagnitudeDiff = prevTouchDeltaMag - touchDeltaMag;

        mainCamera.orthographicSize += deltaMagnitudeDiff * scrollSensitivity / 1000f;
        mainCamera.orthographicSize = Mathf.Max(mainCamera.orthographicSize, minZoom);
        mainCamera.orthographicSize = Mathf.Min(mainCamera.orthographicSize, maxZoom);

        mainCamera.transform.position = new Vector3(mainCamera.transform.position.x, mainCamera.transform.position.y, -10 - (mainCamera.orthographicSize - minZoom) / (maxZoom - minZoom) * 10f);
    }
    void RotatePaused()
    {
        if (GOtarget != null)
        {
            Vector3 dir = Input.GetTouch(0).position - Input.GetTouch(1).position;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            if (!isRotating)
            {
                angleDifference = trgRigidbody2D.rotation - angle;
            }

            isRotating = true;
            if (snap)
                GOtarget.transform.eulerAngles = new Vector3(0f, 0f, Mathf.Round((angle + angleDifference) * rotationSnapScale) / rotationSnapScale);
            else
                GOtarget.transform.eulerAngles = new Vector3(0f, 0f, angle + angleDifference);
            trgAngle = angle + angleDifference;
        }
    }
    void Rotate()
    {
        if (GOtarget != null && trgJoint != null && trgRigidbody2D.constraints != RigidbodyConstraints2D.FreezeAll)
        {
            if (!startRotating) //Rotation
            {
                isRotating = true;
                startRotating = true;
                Vector3 dir = Input.GetTouch(0).position - Input.GetTouch(1).position;
                float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
                angleDifference = trgRigidbody2D.rotation - angle;
            }
            else
            {
                Vector3 dir;
                float angle;
                if (GOtarget.transform.CompareTag("Weapon"))
                {
                    dir = GOtarget.transform.position - mainCamera.ScreenToWorldPoint(Input.GetTouch(1).position);
                    angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
                    if (GOtarget.transform.lossyScale.x < 0)
                        angle = angle + 180;
                    if (!isRotating)
                    {
                        angleDifference = angle + 180;
                    }
                    isRotating = true;
                    trgJoint.target = mainCamera.ScreenToWorldPoint(Input.GetTouch(0).position);
                    if (snap)
                        trgRigidbody2D.MoveRotation(Mathf.Round((angle + 180) * rotationSnapScale) / rotationSnapScale);
                    else
                        trgRigidbody2D.MoveRotation(angle + 180);
                    trgAngle = angle + 180;
                }
                else
                {
                    dir = Input.GetTouch(0).position - Input.GetTouch(1).position;
                    angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
                    if (!isRotating)
                    {
                        angleDifference = trgRigidbody2D.rotation - angle;
                    }
                    isRotating = true;
                    trgJoint.target = mainCamera.ScreenToWorldPoint(Input.GetTouch(0).position);
                    if (snap)
                        trgRigidbody2D.MoveRotation(Mathf.Round((angle + angleDifference) * rotationSnapScale) / rotationSnapScale);
                    else
                        trgRigidbody2D.MoveRotation(angle + angleDifference);
                    trgAngle = angle + angleDifference;
                }
            }
            if (GOtarget.transform.CompareTag("Weapon"))
            {
                shootingSlider.GetComponent<UISmoothSlide>().Open();
                shootingSlider.value = (mainCamera.ScreenToViewportPoint(Input.GetTouch(0).position) - mainCamera.ScreenToViewportPoint(Input.GetTouch(1).position)).magnitude;
                if (shootingSlider.value >= 0.5f)
                {
                    ActivateWeapon();
                }
                else
                {
                    DeactivateWeapon();
                }
            }
        }
    }
    void Follow()
    {
        if (followGO != null)
            mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, new Vector3(followGO.transform.position.x, followGO.transform.position.y, -10), 50 * Time.deltaTime);
        else
        {
            follow = false;
        }
    }
    public void Unfollow()
    {
        follow = false;
    }
    public void CleanSelected()
    {
        int count = selectedObjects.Count;
        for (int i = 0; i < count; ++i)
        {
            if (selectedObjects[i].TryGetComponent(out SpriteRenderer s))
            {
                s.color = Color.white;
            }
        }
    }
    public void PaintSelected()
    {
        int count = selectedObjects.Count;
        for (int i = 0; i < count; ++i)
        {
            if (selectedObjects[i].TryGetComponent(out SpriteRenderer s))
            {
                s.color = new Color(1, 1, 1, 0.7f);
            }
        }
    }
    void GetObjectsInSelectZone()
    {
        CleanSelected();
        selectedObjects.Clear();
        GameObject[] allObjects = FindObjectsOfType<GameObject>();
        for (int i = 0; i < allObjects.Length; ++i)
        {
            if (IsInRange(capturedSelectZonePos, Input.mousePosition, mainCamera.WorldToScreenPoint(allObjects[i].transform.position)) && (allObjects[i].TryGetComponent(out Rigidbody2D r) && !r.isKinematic 
                || allObjects[i].TryGetComponent(out RigidBodyData rd)) && allObjects[i].transform.position.z > -10 && !allObjects[i].CompareTag("EasterEgg"))
            {
                selectedObjects.Add(allObjects[i]);
                selectedObjects[selectedObjects.Count - 1].GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0.7f);
            }
        }
    }
    bool IsInRange(Vector2 v1, Vector2 v2, Vector2 v3)
    {
        return v3.x >= Mathf.Min(v1.x, v2.x) && v3.x <= Mathf.Max(v1.x, v2.x) && v3.y >= Mathf.Min(v1.y, v2.y) && v3.y <= Mathf.Max(v1.y, v2.y);
    }
    Vector2 Abs(Vector2 V)
    {
        return new Vector2(Mathf.Abs(V.x), Mathf.Abs(V.y));
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
    private bool IsPointerOverUIObject(int touchIndex)
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        if (Input.touchCount > 0)
        {
            eventDataCurrentPosition.position = new Vector2(Input.GetTouch(touchIndex).position.x, Input.GetTouch(touchIndex).position.y);
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
    public void ActivateTarget()
    {
        if (GOtarget != null)
        {
            GOtarget.SendMessage("Active", SendMessageOptions.DontRequireReceiver);
        }
    }
    public void ActivateWeapon()
    {
        if (GOtarget != null)
        {
            if (GOtarget.TryGetComponent(out Detonator detonator))
                detonator.Active();
            if (GOtarget.TryGetComponent(out ItemThrower thrower))
                thrower.isShooting = true;
            if (GOtarget.TryGetComponent(out Gun gun))
                gun.isShooting = true;
            if (GOtarget.TryGetComponent(out Spray spray))
                spray.isShooting = true;
            if (GOtarget.TryGetComponent(out Trap trap))
                trap.Active();
        }
    }
    public void DeactivateWeapon()
    {
        if (GOtarget != null)
        {
            if (GOtarget.TryGetComponent(out Gun gun))
                gun.isShooting = false;
            if (GOtarget.TryGetComponent(out Spray spray))
                spray.isShooting = false;
            if (GOtarget.TryGetComponent(out ItemThrower thrower))
                thrower.isShooting = false;
        }
    }
}
