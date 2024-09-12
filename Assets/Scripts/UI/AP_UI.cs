using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AP_UI : MonoBehaviour 
{
    [SerializeField]
    private Sprite[] apSprites;
    private Color colorHolder;

    private bool fadeIn;
    private bool fadeOut;

    private IEnumerator waitShort;
    private IEnumerator waitLonger;


    void Start() {
        fadeOut = false;
        colorHolder = this.gameObject.GetComponent<SpriteRenderer>().color;
        waitLonger = waitSecondsToFadeOut(4);
        StartCoroutine(waitLonger);
    }

    public void updateSprite(int charges) 
    {
        fadeOut = false;
        colorHolder.a = 1; 
        this.gameObject.GetComponent<SpriteRenderer>().color = colorHolder;
        this.gameObject.GetComponent<SpriteRenderer>().sprite = apSprites[charges];
        StartCoroutine(waitSecondsToFadeOut(2));
    }

    IEnumerator waitSecondsToFadeOut(float seconds) {
        yield return new WaitForSeconds(seconds);
        fadeOut = true;
    }

    void Update() {
        if (fadeOut) {
            if (colorHolder.a > 0) {
                colorHolder.a -= Time.deltaTime / 3;
                this.gameObject.GetComponent<SpriteRenderer>().color = colorHolder;
            }
        }
    }
}
