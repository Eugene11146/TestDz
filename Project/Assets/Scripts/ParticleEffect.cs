using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleEffect : MonoBehaviour
{
    public string effectName;

    private void OnParticleCollision(GameObject other)
    {
        other.SendMessage(effectName, SendMessageOptions.DontRequireReceiver);
        if (other.TryGetComponent(out Properties prop))
        {
            if (effectName == "Ignite")
            {
                prop.temperature += 5;
            }
            if (effectName == "Freezer")
            {
                other.SendMessage("Extinguish", SendMessageOptions.DontRequireReceiver);
                if (prop.temperature < 0)
                {
                    prop.temperature = Mathf.Clamp(prop.temperature - 1f, -217, prop.temperature);
                }
                else
                {
                    prop.temperature = Mathf.Clamp(prop.temperature - 5, -217, prop.temperature);
                }
            }
            if (effectName == "Extinguish")
            {
                prop.temperature = Mathf.Clamp(prop.temperature - 1f, GlobalSetting.envorimentTemperature, prop.temperature);
            }
        }
    }
}
