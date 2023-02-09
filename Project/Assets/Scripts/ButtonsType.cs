using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonsType : MonoBehaviour
{
    public int count;
    public GameObject preset;
    public GameObject spawnContainer;
    public TypeButton[] typeButtons;

    private void Start()
    {
        for (int i = 0; i < typeButtons.Length; ++i)
        {
            GameObject temp = Instantiate(preset);
            temp.transform.SetParent(transform);
            temp.name = typeButtons[i].name;
            temp.transform.GetChild(0).GetComponent<Image>().sprite = typeButtons[i].icon;
            temp.transform.localScale = new Vector3(1, 1, 1);
            temp.GetComponent<ChangeSpawnIndex>().index = i;
            temp.GetComponent<ChangeSpawnIndex>().menu = spawnContainer;
        }
        StartCoroutine(delayedMove());
    }

    IEnumerator delayedMove()
    {
        yield return new WaitForSecondsRealtime(0.01f);

        transform.position += new Vector3(GetComponent<RectTransform>().rect.width, 0, 0);
    }
}

[System.Serializable]
public class TypeButton
{
    public string name;
    public Sprite icon;
}
