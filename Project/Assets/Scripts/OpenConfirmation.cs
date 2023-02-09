using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class OpenConfirmation : MonoBehaviour
{
    public GameObject confirmationObject;
    public string confirmationText = "";
    public UnityEvent unityEvent;

    public void Open()
    {
        GameObject localconfirmationObject = Instantiate(confirmationObject, GlobalSetting.mainCamera.transform.GetChild(0).GetChild(0));
        Button button = localconfirmationObject.GetComponent<ConfirmationWindowLinks>().confirmationButton;
        button.onClick.AddListener(unityEvent.Invoke);
        if (confirmationText != "")
        {
            localconfirmationObject.GetComponent<ConfirmationWindowLinks>().confirmationText.text = confirmationText;
        }
    }
}
