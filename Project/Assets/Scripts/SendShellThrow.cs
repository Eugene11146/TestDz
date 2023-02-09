using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SendShellThrow : MonoBehaviour
{
    public GameObject main;

    public void ThrowShell()
    {
        main.SendMessage("ThrowShell");
    }
}
