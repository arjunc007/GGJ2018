//Sandy-Test to set component to random spawn

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ramdom = UnityEngine.Random;

public class RandomSpawner : MonoBehaviour {

    public GameObject component;
    public List<GameObject> spawnPoints = new List<GameObject>();
    private int randomSpawn;

	// Use this for initialization
	void Start ()
    {
        randomSpawn = Random.Range(0, spawnPoints.Count);
        Instantiate(component, spawnPoints[randomSpawn].transform.position, Quaternion.identity);   //object, positon, rotation
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}
}
