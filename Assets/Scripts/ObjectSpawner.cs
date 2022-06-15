using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    public GameObject objectToSpawn;
    private PlacementIndicator placementIndicator;
    [HideInInspector]
    public bool isSpawned = false;
    //public GameObject AIGirl;
    void Start()
    {
        placementIndicator = FindObjectOfType<PlacementIndicator>();
        objectToSpawn.SetActive(false);
        //AIGirl.SetActive(false);
    }
    void Update()
    {
        if (Input.touchCount > 0 && Input.touches[0].phase == TouchPhase.Began && !isSpawned)
        {
            //objectToSpawn.transform.position = Vector3.zero;
            objectToSpawn.transform.position = placementIndicator.transform.position;
            objectToSpawn.SetActive(true);
            //GameObject obj = Instantiate(objectToSpawn, placementIndicator.transform.position, placementIndicator.transform.rotation);
            isSpawned = true;
        }
    }
}
