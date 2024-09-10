using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StormBorderManager : MonoBehaviour
{
    PlayerManager  playerManager;
    [SerializeField]
    float ShrinkRate = 0.01f;

    private void Start()
    {
        playerManager = GetComponent<PlayerManager>();
    }
    private void Update()
    {
        Shrink();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
    }

    /// <summary>
    /// 
    /// </summary>
    public void Shrink()
    {
        transform.localScale -= transform.localScale * ShrinkRate * Time.deltaTime;
    }
}
