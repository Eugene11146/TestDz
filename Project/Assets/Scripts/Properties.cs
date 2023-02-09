using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Properties : MonoBehaviour
{
    public Texture2D meltMap;
    public Transform grabTransform;
    public Material thisMaterial;
    public List<SpriteRenderer> materialIgnoreList;
    public Vector3 velocity;
    public Vector3 lastVelocity;
    public int layer;
    public float temperature;
    public float temperatureAddition;
    public float fireTicker = 0;
    public bool canBeTaken = false;
    public bool canBeActivated = false;
    public bool canGlow = true;
    public bool canBurn = true;
    public bool isOnFire = false;
    public bool levitate = false;
    public bool gravity = true;

    GameObject thisFire;
    SpriteRenderer[] spriteRenderersInChildren;
    List<Properties> propertiesInCollision = new List<Properties>();
    Rigidbody2D rb;
    List<Vector2> originalBoxCollidersSizes = new List<Vector2>();
    List<Vector2[]> originalPolygonCollidersPoints = new List<Vector2[]>();
    Vector3 lastPos;
    Vector3 lastLossyScale;
    float lastTemperature;
    bool isUnderWater = false;


    private void Awake()
    {
        layer = gameObject.layer;
        if (grabTransform == null)
            grabTransform = transform;
        temperature = GlobalSetting.envorimentTemperature;

        SetMaterial();

        TryGetComponent(out rb);

        GetOriginalSizes();
    }

    void SetMaterial()
    {
        thisMaterial = Instantiate(GlobalSetting.defaultMaterial);
        GetComponent<SpriteRenderer>().material = thisMaterial;
        if (meltMap != null)
        {
            thisMaterial.SetTexture("_MeltMap", meltMap);
        }
        thisMaterial.SetFloat("_RandomSeed", Random.Range(0, 1000f));
        spriteRenderersInChildren = GetComponentsInChildren<SpriteRenderer>();
        foreach (SpriteRenderer spriteRenderer in spriteRenderersInChildren)
        {
            if ((materialIgnoreList.Count != 0 && !materialIgnoreList.Contains(spriteRenderer)) || materialIgnoreList.Count == 0)
                spriteRenderer.material = thisMaterial;
        }
    }

    public void GetOriginalSizes()
    {
        originalBoxCollidersSizes.Clear();
        var boxColliders = GetComponents<BoxCollider2D>();
        if (boxColliders.Length > 0)
        {
            foreach (BoxCollider2D boxCollider in boxColliders)
            {
                originalBoxCollidersSizes.Add(boxCollider.size);
            }
        }
        originalPolygonCollidersPoints.Clear();
        var polygonColliders = GetComponents<PolygonCollider2D>();
        if (polygonColliders.Length > 0)
        {
            foreach (PolygonCollider2D polygonCollider in polygonColliders)
            {
                originalPolygonCollidersPoints.Add(polygonCollider.points);
            }
        }
    }

    public void SetGlowMap()
    {
        if (thisMaterial == null)
        {
            thisMaterial = Instantiate(GlobalSetting.defaultMaterial);
        }
        meltMap.filterMode = FilterMode.Point;
        thisMaterial.SetTexture("_MeltMap", meltMap);
    }

    public void SetOriginalColliderSize()
    {
        float offsetSubstructor = 0.0191855f;
        originalBoxCollidersSizes.Clear();
        originalPolygonCollidersPoints.Clear();
        var boxColliders = GetComponents<BoxCollider2D>();
        if (boxColliders.Length > 0)
        {
            foreach (BoxCollider2D boxCollider in boxColliders)
            {
                originalBoxCollidersSizes.Add(boxCollider.size + new Vector2(offsetSubstructor / Abs(transform.lossyScale).x, offsetSubstructor / Abs(transform.lossyScale).y));
            }
        }
        var polygonColliders = GetComponents<PolygonCollider2D>();
        if (polygonColliders.Length > 0)
        {
            foreach (PolygonCollider2D polygonCollider in polygonColliders)
            {
                Vector2[] points = polygonCollider.points;
                for (int i = 0; i <  polygonCollider.points.Length; ++i)
                {
                    Vector2 normalizedPoint = points[i].normalized;
                    points[i] = new Vector2(points[i].x + normalizedPoint.x * offsetSubstructor / Abs(transform.lossyScale).x,
                                    points[i].y + normalizedPoint.y * offsetSubstructor / Abs(transform.lossyScale).y);
                }
                originalPolygonCollidersPoints.Add(points);
            }
        }
        lastLossyScale = Abs(transform.lossyScale);
    }

    public void ResizeColliders()
    {
        float offsetSubstructor = 0.0191855f;
        BoxCollider2D[] boxColliders = GetComponents<BoxCollider2D>();
        if (boxColliders.Length > 0 && originalBoxCollidersSizes.Count == 0)
        {
            GetOriginalSizes();
        }
        for (int i = 0; i < boxColliders.Length; ++i)
        {
            boxColliders[i].size = new Vector2(Mathf.Clamp(originalBoxCollidersSizes[i].x - offsetSubstructor / Abs(transform.lossyScale).x, 0.003f / Abs(transform.lossyScale).x, originalBoxCollidersSizes[i].x),
                                            Mathf.Clamp(originalBoxCollidersSizes[i].y - offsetSubstructor / Abs(transform.lossyScale).y, 0.003f / Abs(transform.lossyScale).y, originalBoxCollidersSizes[i].y));
        }
        PolygonCollider2D[] polygonColliders = GetComponents<PolygonCollider2D>();
        if (polygonColliders.Length > 0 && originalPolygonCollidersPoints.Count == 0)
        {
            GetOriginalSizes();
        }
        foreach (PolygonCollider2D polygonCollider in polygonColliders)
        {
            Vector2[] points = polygonCollider.points;
            for (int i = 0; i < points.Length; ++i)
            {
                Vector2 normalizedPoint = originalPolygonCollidersPoints[0][i].normalized;
                points[i] = new Vector2(originalPolygonCollidersPoints[0][i].x - normalizedPoint.x * offsetSubstructor / Abs(transform.lossyScale).x,
                                        originalPolygonCollidersPoints[0][i].y - normalizedPoint.y * offsetSubstructor / Abs(transform.lossyScale).y);
            }
            polygonCollider.SetPath(0, points);
        }
        lastLossyScale = Abs(transform.lossyScale);
    }

    Vector3 Abs(Vector3 vector)
    {
        return new Vector3(Mathf.Abs(vector.x), Mathf.Abs(vector.y), Mathf.Abs(vector.z));
    }

    private void Update()
    {
        temperature = Mathf.Clamp(temperature, -273, temperature);
        if (lastLossyScale != transform.lossyScale && transform.root.gameObject.name != "SelectedContainer(Clone)" && (rb != null && rb.bodyType != RigidbodyType2D.Static || rb == null))
        {
            ResizeColliders();
        }

        try
        {
            thisMaterial.SetFloat("_Intensivity", thisMaterial.GetFloat("_Intensivity"));
        }
        catch
        {
            SetMaterial();
        }

        if (rb != null)
        {
            if (levitate)
            {
                rb.gravityScale = 0;
                rb.velocity = new Vector2(0, 0);
                rb.angularVelocity = 0;

                if ((GlobalSetting.defaultConstraintController.thisBall != null && GlobalSetting.defaultConstraintController.thisBall.GetComponent<RotateObject>().trg != gameObject) ||
                    GlobalSetting.defaultConstraintController.thisBall == null)
                {
                    levitate = false;
                }
            }
            else
            {
                if (gravity)
                    rb.gravityScale = 1;
                else
                    rb.gravityScale = 0;
            }
        }

        if (propertiesInCollision.Count > 0)
        {
            for (int i = 0; i < propertiesInCollision.Count; ++i)
            {
                if (propertiesInCollision[i] == null)
                {
                    propertiesInCollision.RemoveAt(i);
                    --i;
                }
            }
        }

        if (Mathf.Abs(GlobalSetting.envorimentTemperature - temperature) > 0.01f)
        {
            if (temperature > 0)
                temperature += Mathf.Clamp(GlobalSetting.envorimentTemperature - temperature, -5, 5) * Time.deltaTime;
            else
                temperature += Mathf.Clamp(GlobalSetting.envorimentTemperature - temperature, -0.3f, 0.3f) * Time.deltaTime;
        }
        else
        {
            temperature = GlobalSetting.envorimentTemperature;
        }

        if (temperature != lastTemperature)
        {
            OnTemperatureChanged();
        }
        if (isOnFire)
        {
            thisMaterial.SetFloat("_BurnScale", Mathf.Clamp(thisMaterial.GetFloat("_BurnScale") + Time.deltaTime * 0.02f, 0, 1));
            temperature += Time.deltaTime * 35;
        }

        lastTemperature = temperature;
    }

    void OnTemperatureChanged()
    {
        if (canGlow)
        {
            thisMaterial.SetFloat("_Intensivity", Mathf.Clamp(temperature - 500, 0, 1200) / 400f);
        }

        if (temperature < 0)
        {
            if (isOnFire)
            {
                Extinguish();
            }
            thisMaterial.SetFloat("_FreezeScale", Mathf.Clamp(-temperature / 30, 0, 0.6f) + 0.4f);
        }
        else
        {
            thisMaterial.SetFloat("_FreezeScale", 0);
        }

        if (canBurn && (isOnFire || (temperature - lastTemperature > 150 && thisMaterial.GetFloat("_BurnScale") < 1)) && thisFire == null && temperature > 0)
            Ignite();
        if (!isOnFire && thisFire != null)
            Extinguish();
        if (isOnFire && thisFire == null)
            isOnFire = false;

        if (canBurn && thisFire != null && thisMaterial.GetFloat("_BurnScale") == 1)
        {
            ParticleSystem.MainModule thisParticleMain = thisFire.GetComponent<ParticleSystem>().main;
            if (thisMaterial.GetFloat("_BurnScale") == 1)
            {
                thisParticleMain.startSizeX = Mathf.Clamp(thisParticleMain.startSizeX.constant - Time.deltaTime * 0.1f, 0f, 1f);
                thisParticleMain.startSizeY = thisParticleMain.startSizeX.constant / 2f;
                if (thisParticleMain.startSize.constant < 0.1f)
                {
                    Extinguish();
                }
            }
        }
        if (propertiesInCollision.Count > 0)
        {
            foreach (Properties prop in propertiesInCollision)
            {
                temperature += Mathf.Clamp(prop.temperature - temperature, -50, 50) * Time.deltaTime;
                if (isOnFire && !prop.isOnFire)
                {
                    prop.Ignite();
                }
            }
        }

        if (fireTicker > 0f)
        {
            fireTicker -= Time.deltaTime;
        }
        else
        {
            fireTicker = 0;
        }
    }

    private void FixedUpdate()
    {
        lastVelocity = velocity;
        velocity = lastPos - transform.position;
        lastPos = transform.position;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent(out Properties properties) && !propertiesInCollision.Find(x => x == properties))
        {
            propertiesInCollision.Add(properties);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent(out Properties properties) && propertiesInCollision.Find(x => x == properties))
        {
            propertiesInCollision.Remove(properties);
        }
    }

    public void Ignite()
    {
        if (canBurn && thisFire == null && !isOnFire && thisMaterial.GetFloat("_BurnScale") != 1 && fireTicker == 0)
        {
            thisFire = Instantiate(GlobalSetting.fireEffect, transform.position, transform.rotation);
            thisFire.transform.SetParent(transform);

            ParticleSystem thisParticle = thisFire.GetComponent<ParticleSystem>();
            ParticleSystem.MainModule thisParticleMain = thisParticle.main;
            SpriteRenderer thisSpriteRenderer = transform.GetComponent<SpriteRenderer>();

            var thisParticleEmmision = thisParticle.emission;
            thisParticleEmmision.rateOverTime = Mathf.Clamp((int)(thisSpriteRenderer.sprite.rect.width * thisSpriteRenderer.sprite.rect.height * 0.0028828125f) * (2500 / thisSpriteRenderer.sprite.pixelsPerUnit / thisSpriteRenderer.sprite.pixelsPerUnit), 1, 50);

            var thisParticleShape = thisParticle.shape;
            thisParticleShape.sprite = thisSpriteRenderer.sprite;
            thisParticleShape.texture = thisSpriteRenderer.sprite.texture;

            thisParticleMain.maxParticles = (int)(thisParticleEmmision.rateOverTime.constant * 4);
            thisParticleMain.startSizeX = Mathf.Clamp(thisSpriteRenderer.sprite.rect.width * thisSpriteRenderer.sprite.rect.height / 512f * (2500 / thisSpriteRenderer.sprite.pixelsPerUnit / thisSpriteRenderer.sprite.pixelsPerUnit), 0.4f, 1);
            thisParticleMain.startSizeY = thisParticleMain.startSizeX.constant / 2f;

            thisParticle.Play();

            isOnFire = true;
        }
    }

    public void Extinguish()
    {
        if (isOnFire && thisFire != null)
        {
            fireTicker = 5f;
            ParticleSystem thisParticle = thisFire.GetComponent<ParticleSystem>();
            thisParticle.Stop();

            isOnFire = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Water"))
        {
            if (temperature - GlobalSetting.envorimentTemperature > 100 && !isUnderWater)
            {
                Instantiate(GlobalSetting.soundEmmiter, transform.position, transform.rotation).GetComponent<AudioSource>().PlayOneShot(GlobalSetting.mainCamera.GetComponent<SoundController>().fastBurn[Random.Range(0, 3)]);
            }
            if (temperature > 500)
                temperature = 500;
            isUnderWater = true;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Water"))
        {
            if (isOnFire)
                Extinguish();
            temperature += Mathf.Clamp(GlobalSetting.envorimentTemperature - temperature, -300, 300) * Time.deltaTime;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Water"))
        {
            isUnderWater = false;
        }
    }
}
