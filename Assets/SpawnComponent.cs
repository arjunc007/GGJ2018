using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnComponent : MonoBehaviour 
{
    public Rigidbody player;
    public GameObject component;
    public Boolean hasCompoment = false;

	// Use this for initialization
	void Start ()
    { 
		Instantiate(component,this.transform.position, Quaternion.identity); 
	}
	
	// Update is called once per frame
	void Update () 
	{
	}
}
