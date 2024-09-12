using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SETTINGSBUTTON : MonoBehaviour
{
    public static SETTINGSBUTTON instance;

    // we only want a single settings menu anyways...
    private void Awake() {
        DontDestroyOnLoad(this);
        if (instance == null) {
            instance = this;
        } else if (instance != this) {
            Destroy(gameObject);
        }
    }
    
}
