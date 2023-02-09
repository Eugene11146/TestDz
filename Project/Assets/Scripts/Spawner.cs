using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject GO;
    GameObject lastGO;
    Camera mainCamera;

    private void Start()
    {
        mainCamera = Camera.main;
    }

    public void Spawn()
    {
        if (!GlobalSetting.settings.weaponTutorial && GO.CompareTag("Weapon"))
        {
            Instantiate(GlobalSetting.weaponTutorial, mainCamera.transform.GetChild(0));
            GlobalSetting.settings.weaponTutorial = true;
            GlobalSetting.settings.SaveVolume();
        }
        else if (!GlobalSetting.settings.rotationTutorial)
        {
            Instantiate(GlobalSetting.rotationTutorial, mainCamera.transform.GetChild(0));
            GlobalSetting.settings.rotationTutorial = true;
            GlobalSetting.settings.SaveVolume();
        }
        else if (!GlobalSetting.settings.contextTutorial)
        {
            Instantiate(GlobalSetting.contextTutorial, mainCamera.transform.GetChild(0));
            GlobalSetting.settings.contextTutorial = true;
            GlobalSetting.settings.SaveVolume();
        }
        else if (!GlobalSetting.settings.selectTutorial)
        {
            Instantiate(GlobalSetting.selectTutorial, mainCamera.transform.GetChild(0));
            GlobalSetting.settings.selectTutorial = true;
            GlobalSetting.settings.SaveVolume();
        }

        Debug.Log("Spawned " + GO.name);
        GlobalSetting.spawnedObjects++;
        if (GlobalSetting.spawnedObjects >= 3)
        {
            GlobalSetting.mainCamera.GetComponent<AdsManager>().ShowAd();
            GlobalSetting.spawnedObjects = 0;
        }
        if (transform.parent.lossyScale.x > 0)
        {
            lastGO = Instantiate(GO, (Vector2)mainCamera.ScreenToWorldPoint(Input.mousePosition), new Quaternion());
        }
        else
        {
            lastGO = Instantiate(GO, (Vector2)mainCamera.ScreenToWorldPoint(Input.mousePosition), new Quaternion());
            lastGO.transform.localScale = new Vector3(-1, 1, 1);
        }
        if (!lastGO.activeInHierarchy)
        {
            lastGO.SetActive(true);
        }
        if (lastGO.name != "CopyContainer(Clone)")
        {
            var propertiesInGameObject = lastGO.GetComponentsInChildren<Properties>();
            foreach (Properties gameObjectProperties in propertiesInGameObject)
            {
                gameObjectProperties.ResizeColliders();
            }
        }
        else
        {
            lastGO.transform.GetChild(0).gameObject.SetActive(true);
            var propertiesInGameObject = lastGO.GetComponentsInChildren<Properties>();
            foreach (Properties gameObjectProperties in propertiesInGameObject)
            {
                gameObjectProperties.SetOriginalColliderSize();
                gameObjectProperties.ResizeColliders();
            }
            lastGO.transform.GetChild(0).gameObject.SetActive(false);
        }
    }

    public void DestroyLast()
    {
        if (lastGO != null)
            Destroy(lastGO);
    }
}
