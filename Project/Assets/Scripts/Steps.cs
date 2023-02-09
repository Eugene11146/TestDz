using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

public class Steps : MonoBehaviour
{
    public List<StepButton> steps;
    public GameObject defaultStepButton;

    private void Start()
    {
        for (int i = 0; i < steps.Count; ++i)
        {
            GameObject thisStep = Instantiate(defaultStepButton, transform.GetChild(0));
            thisStep.GetComponentInChildren<TextMeshProUGUI>().text = steps[i].name;
            thisStep.GetComponentInChildren<Button>().onClick.AddListener(steps[i].stepFunctions.Invoke);
        }
    }
}

[System.Serializable]
public struct StepButton
{
    public string name;
    public UnityEvent stepFunctions;
}
