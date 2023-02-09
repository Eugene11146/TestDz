using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeMenuSkins : MonoBehaviour
{
    public GameObject[] scenes;
    public UISmoothSlide image;
    public Camera cam;
    public List<int> playedScenes;
    GameObject thisScene;

    private void Start()
    {
        InvokeRepeating("ChangeScene", 0f, 5f);
    }

    void ChangeScene()
    {
        image.Open();
        StartCoroutine(Delay(image.lerpTime));
    }

    IEnumerator Delay(float delay)
    {
        yield return new WaitForSeconds(delay);

        if (thisScene != null)
        {
            Destroy(thisScene);
            var particles = GameObject.FindGameObjectsWithTag("Particle");
            foreach (GameObject particle in particles)
            {
                Destroy(particle);
            }
            ClearEverything();
        }
        int rand = Random.Range(0, scenes.Length);
        while (playedScenes.Count > 0 && playedScenes.Contains(rand) && playedScenes.Count < scenes.Length)
        {
            rand = Random.Range(0, scenes.Length);
        }
        playedScenes.Add(rand);
        thisScene = Instantiate(scenes[rand], cam.transform.position, new Quaternion(), cam.transform);
        thisScene.transform.SetParent(null);
        if (playedScenes.Count == scenes.Length)
            playedScenes.Clear();

        image.Close();
    }

    public void ClearEverything()
    {
        var objects = FindObjectsOfType<GameObject>();

        for (int i = 0; i < objects.Length; ++i)
        {
            if (objects[i].transform.root.TryGetComponent(out Rigidbody2D r) || objects[i].transform.root.CompareTag("Ragdoll"))
            {
                if (!objects[i].transform.root.CompareTag("Bound"))
                    Destroy(objects[i]);
            }
        }
    }
}
