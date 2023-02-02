using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroySingleton : MonoBehaviour
{
    public static DontDestroySingleton instance { get; private set; }

    private void Awake()
    { 
        if (instance != null)
        {
            Destroy(this.gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(this.gameObject); //If not DontDestroyOnLoad, null instance on OnDestroy
    }
}
