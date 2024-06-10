using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyListener : MonoBehaviour
{
    public delegate void KeyEvent(KeyCode keyCode);
    public KeyEvent OnKeyDown;
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            OnKeyDown?.Invoke(KeyCode.Space);
        }
    }
}
