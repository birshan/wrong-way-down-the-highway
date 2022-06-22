using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadGenerator : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject roadPrefab;
    public Transform player;
    static float placeNextRoatAt = 0;
    
    public float maxNumberOfRoadsInFrontOfPlayer = 3;

    private float roadLength = 10f;
    
    static Queue<GameObject> roadObjectQueue = new Queue<GameObject>();
    static Queue<GameObject> activatedRoads = new Queue<GameObject>();
    

    private void Awake()
    {
        for (int i = 0; i < 10; i++)
        {
            var new_road = Instantiate(roadPrefab, new Vector3(0, 0, 0), Quaternion.identity, transform);
            new_road.SetActive(false);
            roadObjectQueue.Enqueue(new_road);

        }

    }

    private void Start()
    {
        FindObjectOfType<Bike>().crashEvent += onCrashRoadReset;
    }

    // Update is called once per frame
    void Update()
    {
        if(player.position.z >= placeNextRoatAt - maxNumberOfRoadsInFrontOfPlayer*roadLength)
        {
            var placedRoad = PlaceRoad(roadObjectQueue);
            activatedRoads.Enqueue(placedRoad);
        }

        if(player.position.z - activatedRoads.Peek().transform.position.z > roadLength)
        {
            var previouslyPlacedRoad = activatedRoads.Dequeue();
            previouslyPlacedRoad.SetActive(false);
            roadObjectQueue.Enqueue(previouslyPlacedRoad);
        }
            

    }

    GameObject PlaceRoad(Queue<GameObject> roadObjectQueue)
    {
        GameObject nextRoad = roadObjectQueue.Dequeue();
        nextRoad.transform.position = new Vector3(0, 0, placeNextRoatAt);
        placeNextRoatAt += 10;
        nextRoad.SetActive(true);


        return nextRoad;
    }

    void onCrashRoadReset()
    {
        player.position = new Vector3(0, 0, -1.65f);
        for (int i = 0; i < activatedRoads.Count; i++)
        {
            var previouslyPlacedRoad = activatedRoads.Dequeue();
            previouslyPlacedRoad.SetActive(false);
            roadObjectQueue.Enqueue(previouslyPlacedRoad);
        }
        placeNextRoatAt = 10;
    }
}




