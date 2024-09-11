using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StormDetect : MonoBehaviour
{
    private List<GameObject> DetectedStorms = new List<GameObject>();
    public GameObject SelectedStorm;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Storm")
            if (!DetectedStorms.Contains(collision.gameObject))
                DetectedStorms.Add(collision.gameObject);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Storm")
            if (DetectedStorms.Contains(collision.gameObject))
                DetectedStorms.Remove(collision.gameObject);
    }

    private void Update()
    {
        SelectStorm();
    }

    public void SelectStorm()
    {
        if (DetectedStorms.Count == 0)
        {
            HighlightStorm(false);
            SelectedStorm = null;
            return;
        }
        if (SelectedStorm != DetectedStorms[0])
        {
            HighlightStorm(false);
        }

        SelectedStorm = DetectedStorms[0];
        HighlightStorm(true);
    }

    private void HighlightStorm(bool indicate)
    {
        if (SelectedStorm != null)
            SelectedStorm.GetComponent<MiniStorm>().Indicate(indicate);
    }
}
