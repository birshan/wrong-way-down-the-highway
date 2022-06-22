using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class VehicleSpawner : MonoBehaviour
{

    public List<GameObject> vehiclePrefabs = new List<GameObject>();
    public Queue<GameObject> vehicleQueue = new Queue<GameObject>();
    public Queue<GameObject> spawnedVehicles = new Queue<GameObject>();
    public Transform player;
    public float spawnAtDistanceFromPlayer = 20;
    public float secondsBetweenSpawns = 5f;
    public float minimiumOncomingVehicleSpeed = 3;
    public static int vehiclesPlayerHasEncountered; // does this fall under the vehicleSpawner class responsibilities?
    private float nextSpawnTime;
    
    void Start()
    {
        for(int i = 0; i < vehiclePrefabs.Count; i++)
        {
            var vehicle = Instantiate(vehiclePrefabs[i], new Vector3(0, 50, -5), Quaternion.identity, transform);
            vehicle.transform.localScale = new Vector3(0.45f, 0.45f, 0.45f); // fixed scale in prefab instead of script
            vehicle.SetActive(false);
            vehicleQueue.Enqueue(vehicle);
            
            
        }

        FindObjectOfType<Bike>().crashEvent += onCrashVehicleReset;
    }

    void FixedUpdate()
    {
        if(Time.time > nextSpawnTime)
        {
            nextSpawnTime = Time.time + secondsBetweenSpawns;
            var spawnedVehicle = vehicleQueue.Dequeue();
            vehiclesPlayerHasEncountered = vehiclesPlayerHasEncountered+1;
           // Debug.Log(vehiclesPlayerHasEncountered);
            if (vehiclesPlayerHasEncountered % 2 == 0)
            {
                spawnedVehicle.transform.position = new Vector3(Random.Range(-4.5f, 4.5f), 1, player.position.z + spawnAtDistanceFromPlayer);//spawns randomly
                spawnedVehicle.transform.rotation = Quaternion.Euler(new Vector3(0, 180, 0));//aim at bike
            }
            else
            {
                spawnedVehicle.transform.position = player.position + new Vector3(0,0,spawnAtDistanceFromPlayer); //spawns in front of player
                spawnedVehicle.transform.rotation = Quaternion.Euler(new Vector3(0, 180, 0));//aim at bike
            }
            
            spawnedVehicle.SetActive(true);
            //spawnedVehicle.GetComponent<Rigidbody>().AddForce(new Vector3(0,0,-1)* vehicleBaseSpeed, ForceMode.Impulse);
            
            //spawnedVehicle.GetComponent<Rigidbody>().velocity = transform.TransformDirection(new Vector3(0, 0, Random.Range(-10, -15)));
            
            spawnedVehicles.Enqueue(spawnedVehicle);

        }
        foreach (GameObject v in spawnedVehicles)
        {
            v.GetComponent<Rigidbody>().velocity = transform.TransformDirection(new Vector3(0, 0, Random.Range(-minimiumOncomingVehicleSpeed, -minimiumOncomingVehicleSpeed-5)));
        }
        if (player.position.z - 10 > spawnedVehicles.Peek().transform.position.z)
        {
            var reuseVehicle = spawnedVehicles.Dequeue();
            reuseVehicle.SetActive(false);
            vehicleQueue.Enqueue(reuseVehicle);
        }
    }

    void onCrashVehicleReset()
    {
        player.position = new Vector3(0, 0.5f, -1.65f);
        player.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
        for(int i=0; i< spawnedVehicles.Count; i++)
        {
            var reuseVehicle = spawnedVehicles.Dequeue();
            reuseVehicle.SetActive(false);
            vehicleQueue.Enqueue(reuseVehicle);
        }

        vehiclesPlayerHasEncountered = 0;
    }
}
