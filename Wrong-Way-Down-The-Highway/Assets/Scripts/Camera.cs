using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject Player;
   
    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(0, Player.transform.position.y + 7.5f, Player.transform.position.z - 6.5f);
        
    }
}
