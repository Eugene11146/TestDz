using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostRenderer : MonoBehaviour
{
    public List<SpriteRenderer> sprites;
    public List<Vector3> localPositions;
    public List<Vector3> lossyScale;
    public List<Quaternion> localRotation;
    public GameObject root;
    public GameObject ghostPrefab;
    SpawnMenuSorting sorting;
    GameObject lastGhost;

    private void Start()
    {
        sorting = transform.parent.parent.parent.parent.parent.GetComponent<SpawnMenuSorting>();
    }

    public void OnDrag()
    {
        Debug.Log("Ghost spawned");
        sprites.Clear();
        localPositions.Clear();
        lossyScale.Clear();
        localRotation.Clear();
        GetAllChildren();
        CreateGhost();
    }

    public void EndDrag()
    {
        Destroy(lastGhost);
    }

    public void CreateGhost()
    {
        GameObject GO = Instantiate(ghostPrefab, Camera.main.ScreenToWorldPoint(Input.mousePosition), Quaternion.identity);
        lastGhost = GO;
        GO.AddComponent<Ghost>();
        for (int i = 0; i < sprites.Count; ++i)
        {
            GameObject thisObj = Instantiate(ghostPrefab, Camera.main.ScreenToWorldPoint(Input.mousePosition) + localPositions[i], localRotation[i]);
            thisObj.transform.SetParent(GO.transform);
            thisObj.GetComponent<SpriteRenderer>().sprite = sprites[i].sprite;
            thisObj.GetComponent<SpriteRenderer>().color = sprites[i].color * new Color(1, 1, 1, 0.5f);
            thisObj.GetComponent<SpriteRenderer>().sortingOrder = sprites[i].sortingOrder;
            thisObj.transform.localScale = lossyScale[i];
        }
        GO.transform.localScale = new Vector3(GO.transform.localScale.x * sorting.targetScale, GO.transform.localScale.y, GO.transform.localScale.z);
    }

    public void GetAllChildren()
    {
        if (root.TryGetComponent(out SpriteRenderer renderer))
        {
            sprites.Add(renderer);
            localPositions.Add(root.transform.position - root.transform.position);
            lossyScale.Add(root.transform.lossyScale);
            localRotation.Add(root.transform.rotation);
        }
        if (root.transform.childCount > 0)
        {
            foreach (Transform child in root.transform)
            {
                GetAllChildren(child.gameObject);
            }
        }
    }
    public void GetAllChildren(GameObject GO)
    {
        if (GO.TryGetComponent(out SpriteRenderer renderer))
        {
            sprites.Add(renderer);
            localPositions.Add(GO.transform.position - root.transform.position);
            lossyScale.Add(GO.transform.lossyScale);
            localRotation.Add(GO.transform.rotation);
        }
        if (GO.transform.childCount > 0)
        {
            foreach (Transform child in GO.transform)
            {
                GetAllChildren(child.gameObject);
            }
        }
    }
}
