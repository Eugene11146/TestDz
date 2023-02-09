using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.IO;

public class SpawnMenuSorting : MonoBehaviour
{
    public List<SpawnButtonList> buttons;
    public GameObject copy;
    public GameObject ghostPreset;
    public GameObject categoryPreset;
    public GameObject horizontalContainerPreset;
    public GameObject verticalContainerPreset;
    public GameObject buttonPreset;
    public GameObject spawnEmpty;
    public GameObject copyButton;
    public GameObject defaultObject;
    public int copyIndex;
    public int index;
    public float targetScale = 1;
    public float lerpTime = 0.2f;
    public List<string> fileNames;
    public List<GameObject> buttonsGO = new List<GameObject>();
    float timeStartedLerping;
    float lerpStart;
    int thisCopyIndex;

    void Start()
    {
        GameObject thisHorizontalContainer = gameObject;
        #region InstantitateButtons
        for (int i = 0; i < GlobalSetting.spawnButtonList.Count; ++i)
        {
            GameObject thisVerticalContainer = Instantiate(verticalContainerPreset, transform);
            thisVerticalContainer.GetComponent<ScrollRect>().viewport = transform.GetComponent<RectTransform>();
            for (int j = 0; j < GlobalSetting.spawnButtonList[i].category.Count; ++j)
            {
                GameObject thisCategory = Instantiate(categoryPreset);
                thisCategory.transform.SetParent(thisVerticalContainer.transform.GetChild(0));
                thisCategory.transform.localScale = new Vector3(1, 1, 1);
                thisCategory.GetComponentInChildren<TextMeshProUGUI>().text = GlobalSetting.spawnButtonList[i].category[j].name;
                for (int h = 0; h < GlobalSetting.spawnButtonList[i].category[j].buttons.Count; ++h)
                {
                    if (h % 3 == 0)
                    {
                        thisHorizontalContainer = Instantiate(horizontalContainerPreset, thisVerticalContainer.transform.GetChild(0));
                    }
                    GameObject thisButton = Instantiate(buttonPreset, thisHorizontalContainer.transform);
                    buttonsGO.Add(thisButton);
                    //thisButton.GetComponent<RectTransform>().anchoredPosition = new Vector3(j % 3 * 139 + 105 + i * 556, (j / 3) * -139 - 170);
                    thisButton.transform.localScale = new Vector3(1f, 1f, 1f);
                    thisButton.transform.GetChild(0).GetComponent<GhostRenderer>().root = GlobalSetting.spawnButtonList[i].category[j].buttons[h].Object;

                    thisButton.transform.GetComponentInChildren<TextMeshProUGUI>().text = GlobalSetting.spawnButtonList[i].category[j].buttons[h].name;
                    thisButton.name = GlobalSetting.spawnButtonList[i].category[j].buttons[h].name;

                    if (GlobalSetting.spawnButtonList[i].category[j].buttons[h].name == "Copy")
                    {
                        copyButton = thisButton;
                    }

                    Transform thisChild = thisButton.transform.GetChild(0);
                    thisChild.transform.GetChild(1).GetComponent<Spawner>().GO = GlobalSetting.spawnButtonList[i].category[j].buttons[h].Object;
                    thisChild.transform.GetChild(1).GetComponent<Image>().sprite = GlobalSetting.spawnButtonList[i].category[j].buttons[h].Icon;
                    if (i == copyIndex)
                    {
                        thisCopyIndex = buttonsGO.Count - 1;
                    }

                    if (h + 1 >= GlobalSetting.spawnButtonList[i].category[j].buttons.Count)
                    {
                        if ((h + 1) % 3 == 2)
                        {
                            Instantiate(spawnEmpty, thisHorizontalContainer.transform);
                        }
                    }
                }
            }
        }
        #endregion
    }

    Texture2D ArrayToMap(List<string> map, Texture2D mainTexture)
    {
        Texture2D texture = new Texture2D(mainTexture.width, mainTexture.height, TextureFormat.RGBA32, false);

        for (int i = 0; i < mainTexture.height; ++i)
        {
            for (int j = 0; j < mainTexture.width; ++j)
            {
                texture.SetPixel(j, i, new Color(0, 0, 0, 0));
            }
        }

        foreach (string hex in map)
        {
            Color color = new Color(Random.Range(0.2f, 1f), Random.Range(0.2f, 1f), Random.Range(0.2f, 1f), 1);
            Vector2 top = new Vector2(hexPairToInt(hex[0].ToString() + hex[1].ToString()), hexPairToInt(hex[2].ToString() + hex[3].ToString()));
            Vector2 bottom = new Vector2(hexPairToInt(hex[4].ToString() + hex[5].ToString()), hexPairToInt(hex[6].ToString() + hex[7].ToString()));
            for (int i = (int)top.x; i <= (int)bottom.x; ++i)
            {
                for (int j = (int)top.y; j < (int)bottom.y; ++j)
                {
                    texture.SetPixel(i, j, new Color(1, 1, 1, 1));
                    //texture.SetPixel(i, j, color);
                }
            }
        }
        texture.Apply();

        return texture;
    }
    int hexPairToInt(string hex)
    {
        int num = 0;
        if (hex[0] >= 'A')
        {
            num += (hex[0] - 55) * 16;
        }
        else
        {
            num += (hex[0] - 48) * 16;
        }
        if (hex[1] >= 'A')
        {
            num += (hex[1] - 55);
        }
        else
        {
            num += (hex[1] - 48);
        }
        return num;
    }

    private void Update()
    {
        GetComponent<RectTransform>().anchoredPosition = new Vector2(index * -GetComponentInChildren<RectTransform>().rect.width, 0);
        if (buttonsGO.Count > 0)
        {
            if (Mathf.Abs(targetScale - buttonsGO[0].transform.localScale.x) > 0)
            {
                for (int i = 0; i < buttonsGO.Count; ++i)
                {
                    buttonsGO[i].transform.localScale = new Vector3(Lerp(lerpStart, targetScale, timeStartedLerping, lerpTime), 1, 1);
                    buttonsGO[i].GetComponentInChildren<TextMeshProUGUI>().transform.localScale = new Vector3(Lerp(lerpStart, targetScale, timeStartedLerping, lerpTime), 1, 1);
                }
            }
        }
        else
        {
            Debug.Log("No buttons");
        }
        if (copy != null)
        {
            copyButton.transform.GetChild(0).GetComponent<GhostRenderer>().root = copy;
            copyButton.transform.GetChild(0).GetChild(1).GetComponent<Spawner>().GO = copy;
        }
    }

    public void Paste(GameObject go)
    { 
        if (go != null)
        {
            GameObject clone = go;
            if (clone.transform.childCount > 0)
                for (int i = clone.transform.GetChild(0).childCount - 1; i >= 0; --i)
                {
                    clone.transform.GetChild(0).GetChild(i).SetParent(null);
                }
            Destroy(clone);
        }
    }

    public void Revert() //Flip
    {
        timeStartedLerping = Time.unscaledTime;
        lerpStart = buttonsGO[0].transform.lossyScale.x;
        if (targetScale > 0)
            targetScale = -1;
        else
            targetScale = 1;
    }

    float Lerp(float start, float end, float timeStartLerping, float lerpTime = 1f)
    {
        float timeSinceStarted = Time.unscaledTime - timeStartLerping;

        float percentageComplete = timeSinceStarted / lerpTime;

        float result = Mathf.Lerp(start, end, Mathf.SmoothStep(0, 1, percentageComplete));

        return result;
    }
}
