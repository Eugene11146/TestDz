using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RotateObject : MonoBehaviour
{
    public GameObject trg;
    public GameObject destroyEffect;

    Rigidbody2D rigidBody2D;
    RectTransform rectTransform;
    CameraMovementForPhone cameraMovement;
    Vector2 targetScale;
    Vector2 startScale;
    Vector2 positionAddition;
    float timeStartedLerping;
    float lerpTime = 0.3f;


    private void Start()
    {
        targetScale = new Vector2(100, 100);
        rectTransform = GetComponent<RectTransform>();
        timeStartedLerping = Time.unscaledTime;
        startScale = new Vector2(0.01f, 0.01f);
        rectTransform.sizeDelta = new Vector2(0, 0);
        cameraMovement = GlobalSetting.mainCamera.GetComponent<CameraMovementForPhone>();
        positionAddition = new Vector2(Mathf.Cos(trg.transform.eulerAngles.z * Mathf.Deg2Rad), Mathf.Sin(trg.transform.eulerAngles.z * Mathf.Deg2Rad));
    }

    void Update()
    {
        rectTransform.sizeDelta = Lerp(startScale, targetScale, timeStartedLerping, lerpTime);
        if (trg == null)
        {
            Destroy(gameObject);
        }
        else
        {
            transform.position = GlobalSetting.mainCamera.WorldToScreenPoint((Vector2)trg.transform.position + positionAddition);

            if (trg.TryGetComponent(out rigidBody2D))
            {
                Vector2 dir = (trg.transform.position - GlobalSetting.mainCamera.ScreenToWorldPoint(transform.position)).normalized;
                if (Time.timeScale > 0)
                {
                    rigidBody2D.rotation = -Mathf.Atan2(dir.x, dir.y) * Mathf.Rad2Deg - 90;
                }
                else
                {
                    trg.transform.rotation = Quaternion.Euler(new Vector3(trg.transform.eulerAngles.x, trg.transform.eulerAngles.y, -Mathf.Atan2(dir.x, dir.y) * Mathf.Rad2Deg - 90));
                }
            }
            if (trg.transform.CompareTag("Weapon"))
            {
                cameraMovement.shootingSlider.GetComponent<UISmoothSlide>().Open();
                cameraMovement.shootingSlider.value = (GlobalSetting.mainCamera.ScreenToWorldPoint(transform.position) - trg.transform.position).magnitude / 4;
                if (cameraMovement.shootingSlider.value >= 0.5f)
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

    public void Drag()
    {
        transform.position = GlobalSetting.mainCamera.ScreenToWorldPoint(Input.mousePosition);
        positionAddition = (Vector2)transform.position - (Vector2)trg.transform.position;
    }

    public void ActivateWeapon()
    {
        if (trg != null)
        {
            if (trg.TryGetComponent(out Detonator detonator))
                detonator.Active();
            if (trg.TryGetComponent(out ItemThrower thrower))
                thrower.isShooting = true;
            if (trg.TryGetComponent(out Gun gun))
                gun.isShooting = true;
            if (trg.TryGetComponent(out Spray spray))
                spray.isShooting = true;
            if (trg.TryGetComponent(out Trap trap))
                trap.Active();
        }
    }
    public void DeactivateWeapon()
    {
        if (trg != null)
        {
            if (trg.TryGetComponent(out Gun gun))
                gun.isShooting = false;
            if (trg.TryGetComponent(out Spray spray))
                spray.isShooting = false;
            if (trg.TryGetComponent(out ItemThrower thrower))
                thrower.isShooting = false;
        }
    }

    private void OnDestroy()
    {
        DeactivateWeapon();
        cameraMovement.shootingSlider.GetComponent<UISmoothSlide>().Close();
    }

    public void Close()
    {
        startScale = rectTransform.sizeDelta;
        timeStartedLerping = Time.unscaledTime;
        targetScale = new Vector2(0, 0);
    }

    Vector3 Lerp(Vector3 startPos, Vector3 endPos, float timeStartLerping, float lerpTime = 1f)
    {
        float timeSinceStarted = Time.unscaledTime - timeStartLerping;

        float percentageComplete = timeSinceStarted / lerpTime;

        Vector3 result = Vector3.Lerp(startPos, endPos, Mathf.SmoothStep(0, 1, percentageComplete));

        return result;
    }
}
