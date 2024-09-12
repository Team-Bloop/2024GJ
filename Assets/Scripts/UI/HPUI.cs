using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HPUI : MonoBehaviour
{
    [SerializeField]
    private GameObject hpBar;
    private RectTransform hpRectTransform;
    private Vector3 originalPosition;
    private float originalPositionX;
    // Start is called before the first frame update
    void Start()
    {
        hpRectTransform = hpBar.GetComponent<RectTransform>();
        originalPositionX = hpRectTransform.localPosition.x;
        originalPosition = hpRectTransform.localPosition;
    }

    public void changeHPBarPosition(float percentage) {
        // percentage is percentage of hp change
        float hpValue = percentage * hpRectTransform.rect.width;
        Vector3 checkVector = hpRectTransform.localPosition + Vector3.left * hpValue;
        if (hpRectTransform.localPosition.x > originalPositionX - hpRectTransform.rect.width
            && checkVector.x < originalPositionX) {
            Debug.Log($"HP LOST (NEGATIVE FOR RECOVERED): {percentage * 100}%");
            hpValue = percentage * hpRectTransform.rect.width;
            hpRectTransform.localPosition = checkVector;
        }
    }

}
