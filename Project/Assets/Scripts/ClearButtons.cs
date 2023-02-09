using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;


public class ClearButtons : MonoBehaviour
{
    public ClearButton[] buttons;
    public GameObject buttonPreset;

    private void Start()
    {
        for (int i = 0; i < buttons.Length; ++i)
        {
            GameObject temp = Instantiate(buttonPreset);
            temp.transform.SetParent(transform);
            temp.transform.localScale = new Vector3(1, 1, 1);
            temp.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = buttons[i].name;
            temp.name = buttons[i].name;
            temp.GetComponent<Button>().onClick.AddListener(buttons[i].func.Invoke);
        }
        StartCoroutine(delayedMove());
    }

    IEnumerator delayedMove()
    {
        yield return new WaitForSecondsRealtime(0.01f);

        transform.position += new Vector3(0, -GetComponent<RectTransform>().rect.height, 0);
    }

    public void ClearEverything()
    {
        GlobalSetting.mainCamera.GetComponent<AdsManager>().ShowAd();
        var objects = FindObjectsOfType<GameObject>();

        for (int i = 0; i < objects.Length; ++i)
        {
            if (objects[i].transform.root.TryGetComponent(out Rigidbody2D r) || objects[i].transform.root.CompareTag("Ragdoll"))
            {
                if (!objects[i].transform.root.CompareTag("Bound") && !objects[i].transform.root.CompareTag("EasterEgg"))
                    Destroy(objects[i]);
            }
        }
    }
    public void ClearLiving()
    {
        var objects = FindObjectsOfType<GameObject>();

        for (int i = 0; i < objects.Length; ++i)
        {
            if (objects[i].transform.root.CompareTag("Ragdoll"))
            {
                Destroy(objects[i]);
            }
        }
    }
    public void ClearGibs()
    {
        var objects = FindObjectsOfType<GameObject>();

        for (int i = 0; i < objects.Length; ++i)
        {
            if (objects[i].transform.root.CompareTag("Gib"))
            {
                Destroy(objects[i]);
            }
        }
    }
    public void ClearCorpses()
    {
    }
}

[System.Serializable]
public class ClearButton
{
    public string name;
    public UnityEvent func;
}
