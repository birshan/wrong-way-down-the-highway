using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UserInterface : MonoBehaviour
{
    
 
    [SerializeField] private Text score; //need to know when the player passes a car - for now just counting vehicles instantiated
    private int numberOfVehiclesPassed;


    [SerializeField] private GameObject gameOverUI; // need to know when the player collides


    public Button restartGame;
    // Start is called before the first frame update
   
    void Start()
    {
        score.text = "Score: 0";
        FindObjectOfType<Bike>().crashEvent += GameOver;
        FindObjectOfType<GameManager>().restartEvent += ResetUI;
    }

    // Update is called once per frame
    void Update()
    {
        numberOfVehiclesPassed = VehicleSpawner.vehiclesPlayerHasEncountered;
        score.text = "Score: " + numberOfVehiclesPassed;
    }

    public void GameOver()
    {
        gameOverUI.SetActive(true);
    }

    public void ResetUI()
    {
        gameOverUI.SetActive(false);
    }

    
}
