using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StormWarning : MonoBehaviour
{
    public Image ScreenImage;
    Color screenColor;

    public bool Flashing = false;
    bool flashOn = false;
    float flashOpacity = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        screenColor = ScreenImage.color;
    }

    // Update is called once per frame
    void Update()
    {
        if (Flashing)
            RedFlash();
    }

    public void FlashSwitch(bool trigger)
    {
        Flashing = trigger;
        screenColor.a = 0;
        ScreenImage.color = screenColor;
    }

    void RedFlash()
    {
        if (!flashOn)
            screenColor.a = Mathf.Lerp(screenColor.a, flashOpacity, Time.deltaTime);
        else
            screenColor.a = Mathf.Lerp(screenColor.a, 0, Time.deltaTime);

        if (screenColor.a >= flashOpacity-0.05f)
            flashOn = true;
        else if (screenColor.a <= 0.1f)
            flashOn = false;

        ScreenImage.color = screenColor;
        Debug.Log(screenColor.a);
    }
}