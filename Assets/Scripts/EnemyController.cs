using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour {

	public GameObject[] enemyTypes;
	public Transform[] enemySpawnPoints;

	List<GameObject> enemies = new List<GameObject>();

	int maxEnemies = 20;
	float timer;

	// Use this for initialization
	void Start () {

		foreach (Transform transform in enemySpawnPoints) {
			if(Random.Range(0, 100) > 20)
				Instantiate (enemyTypes [1], new Vector3(transform.position.x, enemyTypes[1].transform.position.y, transform.position.z), Quaternion.identity);
			else
				Instantiate (enemyTypes [0], new Vector3(transform.position.x, enemyTypes[0].transform.position.y, transform.position.z), Quaternion.identity);
		}
		enemies.AddRange(GameObject.FindGameObjectsWithTag ("Enemy"));
	}
	
	// Update is called once per frame
	void Update () {
		timer += Time.deltaTime;

		if (timer > 10f && enemies.Count < maxEnemies) {
			if(Random.Range(0, 100) > 20)
				Instantiate (enemyTypes [1], new Vector3(transform.position.x, enemyTypes[1].transform.position.y, transform.position.z), Quaternion.identity);
			else
				Instantiate (enemyTypes [0], new Vector3(transform.position.x, enemyTypes[0].transform.position.y, transform.position.z), Quaternion.identity);
			timer = 0f;
		}
	}
}
