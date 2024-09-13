using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class GAME_OVER : MonoBehaviour
{
    // Start is called before the first frame update
    void Start() 
    {

    }

    public void StartGame() 
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("MASTER_SCENE");
    }
}
