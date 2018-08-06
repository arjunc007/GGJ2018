using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    private Player_Controller pc;

	public float rotationSpeed;
	public int value;

    private void Start()
    {
        GameObject[] gos = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject go in gos)
        {
            if (go.name != "Graphics")
            {
                pc = go.GetComponent<Player_Controller>();
            }
        }
    }

	void Update()
	{
		transform.Rotate (0, rotationSpeed * Time.deltaTime, 0);
    }


    private void OnTriggerStay(Collider col)
    {
        if (col.gameObject.tag == "Player")
        {
            if (this.tag == "Component")
            {
                pc.hasComponent = true;
                pc.componentName = this.gameObject.name;
                pc.inRadar1 = false;
                pc.inRadar2 = false;
                pc.inRadar3 = false;
                Destroy(this.gameObject);
            }
            else if (this.tag == "Health")
            {
                pc.health += this.gameObject.GetComponent<Item>().value;
                Destroy(this.gameObject);
            }
            else if (this.tag == "Tank")
            {
                pc.oxygen += this.gameObject.GetComponent<Item>().value;
                Destroy(this.gameObject);
            }
        }
        if (this.tag == "Component")
        {
            switch (col.gameObject.name)
            {
                case "Radar_1":
                    pc.inRadar1 = true;
                    break;
                case "Radar_2":
                    pc.inRadar2 = true;
                    break;
                case "Radar_3":
                    pc.inRadar3 = true;
                    break;
                default:
                    break;
            }
        }
    }

    private void OnTriggerExit(Collider col)
    {
        if (this.tag == "Component")
        {
            switch (col.gameObject.name)
            {
                case "Radar_1":
                    pc.inRadar1 = false;
                    break;
                case "Radar_2":
                    pc.inRadar2 = false;
                    break;
                case "Radar_3":
                    pc.inRadar3 = false;
                    break;
                default:
                    break;
            }
        }
    }
}
