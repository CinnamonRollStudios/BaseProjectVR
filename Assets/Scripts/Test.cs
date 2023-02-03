using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    public string text;

    public void RunTest()
    {
        Debug.Log(text);
    }

    public void RunTest(int num)
    {
        Debug.Log(text + ": " + num);
    }

    public void RunTest(float num)
    {
        Debug.Log(text + ": " + num);
    }
}
