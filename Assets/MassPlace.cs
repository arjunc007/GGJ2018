using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MassPlace : MonoBehaviour
{
    public GameObject component;
    public List <GameObject> spawnPoints = new List<GameObject>(); 

	// Use this for initialization
	void Start ()
    {
		foreach(GameObject spawnpoint in spawnPoints)   //Spawn componet at each spawn point
        {
            Instantiate(component, spawnpoint.transform.position, Quaternion.identity);
        }
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}
}
