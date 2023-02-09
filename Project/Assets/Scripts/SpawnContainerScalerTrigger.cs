using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnContainerScalerTrigger : MonoBehaviour
{
    public void Send()
    {
        var containers = transform.root.GetComponentsInChildren<SpawnContainerScaler>();
        foreach (SpawnContainerScaler container in containers)
        {
            container.ResolveScale();
        }
    }
}
