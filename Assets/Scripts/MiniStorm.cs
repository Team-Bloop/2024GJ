using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniStorm : MonoBehaviour
{
    [SerializeField]
    private float sizeLimit = 100;
    [SerializeField]
    private float ExpandRate = 0.01f;
    [SerializeField]
    private float PulseRange = 0.8f;
    [SerializeField]
    private float PulseRate = 2;
    [SerializeField]
    private bool Pulsing = true;

    public SpriteRenderer SelectedIndicator;

    bool shrinking = true;
    Vector3 currentScale;
    Vector3 shrinkScale;
    Vector3 ExpandScale;

    private void OnTriggerStay2D(Collider2D collision)
    {

    }

    private void OnTriggerExit2D(Collider2D collision)
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

    }

    private void Start()
    {
        currentScale = transform.localScale;
        shrinkScale = currentScale * PulseRange;
        ExpandScale = currentScale;
    }

    private void Update()
    {
        Expand();
        if (Pulsing)
            Pulse();
        else
            transform.localScale = currentScale;
        if (transform.localScale.x < 0.2f)
        {
            Collapse();
        }
    }

    /// <summary>
    /// Increases the size of the storm over time, adjusted by ExpandRate
    /// </summary>
    public void Expand()
    {
        if (currentScale.x < sizeLimit)
            currentScale += currentScale * ExpandRate * Time.deltaTime;
    }

    /// <summary>
    /// Makes the storm pulse in size, the
    ///     - frequency can be adjusted based on PulseRate
    ///     - size range can be adjusted based on PulseRange
    /// </summary>
    public void Pulse()
    {
        float scaleX;
        float scaleY;
        float scaleZ;

        ExpandScale = currentScale;
        shrinkScale = currentScale * PulseRange;
        if (shrinking)
        {
            scaleX = Mathf.Lerp(transform.localScale.x, shrinkScale.x, PulseRate * Time.deltaTime);
            scaleY = Mathf.Lerp(transform.localScale.y, shrinkScale.y, PulseRate * Time.deltaTime);
            scaleZ = Mathf.Lerp(transform.localScale.z, shrinkScale.z, PulseRate * Time.deltaTime);
        }
        else
        {
            scaleX = Mathf.Lerp(transform.localScale.x, ExpandScale.x, PulseRate * Time.deltaTime);
            scaleY = Mathf.Lerp(transform.localScale.y, ExpandScale.y, PulseRate * Time.deltaTime);
            scaleZ = Mathf.Lerp(transform.localScale.z, ExpandScale.z, PulseRate * Time.deltaTime);
        }

        transform.localScale = new Vector3(scaleX, scaleY, scaleZ);

        if (shrinkScale.x / transform.localScale.x > 0.95)
        { 
            shrinking = false;
            
        }
        else if (transform.localScale.x / ExpandScale.x > 0.95)
        {
            shrinking = true;
            
        }
    }

    /// <summary>
    /// Percentage of storm decreased
    /// </summary>
    /// <param name="rate"></param>
    public void Shrink(float rate)
    {
        transform.localScale *= rate;
    }

    public void Collapse()
    {
        Destroy(this);
    }

    public void Indicate(bool indicate)
    {
        SelectedIndicator.enabled = indicate;
    }
}