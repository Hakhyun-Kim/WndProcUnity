using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WndProcTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        WndProcHandler.Enable(true);
    }

    private void OnApplicationQuit()
    {
        WndProcHandler.Enable(false);
    }
}
