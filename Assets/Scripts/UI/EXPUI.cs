using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
namespace UI {
    public class EXPUI : MonoBehaviour {

        [SerializeField]
        private GameObject expBar;
        private RectTransform expRectTransform;
        private Vector3 originalPosition;
        private float originalPositionX;
        
        [SerializeField]
        private GameObject levelValue;
        private string currentLevelText;

        // Start is called before the first frame update
        private void Start() {
            expRectTransform = expBar.GetComponent<RectTransform>();
            originalPositionX = expRectTransform.localPosition.x;
            originalPosition = expRectTransform.localPosition;
            currentLevelText = levelValue.GetComponent<TextMeshProUGUI>().text;
        }

        public void updateEXPUI(float percentage, int level)
        {
            changeExpBarPosition(percentage);
            updateLevelText(level);
        }

        private void updateLevelText(int level)
        {
            levelValue.GetComponent<TextMeshProUGUI>().text = $"LVL {level}";
        }

        public void changeExpBarPosition(float percentage)     
        {
            float increaseValue = percentage * expRectTransform.rect.width;
            Vector3 checkVector = originalPosition + Vector3.right * increaseValue;
            if (checkVector.x < originalPositionX + expRectTransform.rect.width) {
                //Debug.Log($"CURRENT EXP AT: {percentage * 100}%");
                increaseValue = percentage * expRectTransform.rect.width;
                expRectTransform.localPosition = originalPosition + Vector3.right * increaseValue;
            }
        }
    }


}
