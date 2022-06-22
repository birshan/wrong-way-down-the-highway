using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{

    public delegate void Restart();
    public event Restart restartEvent;
    // Start is called before the first frame update
    void Start()
    {
        FindObjectOfType<Bike>().crashEvent += PauseGame;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PauseGame()
    {
        Time.timeScale = 0;
    }

    public void RestartGame()
    {
        
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        if (restartEvent != null)   //instead of restarting the scene i now reset the objects and unpause tha game
        {
            restartEvent();
        }

        Time.timeScale = 1;
    }
}
