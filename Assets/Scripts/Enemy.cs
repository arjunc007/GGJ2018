﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//transform.up = green / y-axis
//transform.right = red / x-axis
//transfor.forward = blue / z-axis

public class Enemy : MonoBehaviour {

	[SerializeField]
	private float health;
	[SerializeField]
	private float patrolSpeed;
	[SerializeField]
	private float attackSpeed;
	[SerializeField]
	private int damage;
	[SerializeField]
	private float patrolLength;
	[SerializeField]
	private float hostileDistance;
	[SerializeField]
	private AudioClip deathClip;

	Animator animator;
	AudioSource enemyAudio;
	ParticleSystem hitParticles;
	private Vector3 velocity;
	private float distTravelled;
	private bool isDead;
	private CapsuleCollider capsuleCollider;

	// Use this for initialization
	void Start () {
		capsuleCollider = GetComponent<CapsuleCollider> ();
		animator = GetComponent<Animator> ();
		hitParticles = GetComponentInChildren<ParticleSystem> ();

		//Randomly select walking direction
		int theta = Random.Range(0, 360);

		transform.rotation = Quaternion.LookRotation (new Vector3(Mathf.Sin(theta), 0, Mathf.Cos(theta)), Vector3.up);

	}

	// Update is called once per frame
	void Update () {

		//Player comes within range, start rushing
		Vector3 target = GameObject.FindWithTag("Player").transform.position;
		if ((transform.position - target).magnitude <= hostileDistance) {
			animator.SetBool("chasing", true);
			Attack (target);
			return;
		}

		animator.SetBool("chasing", false);
		Patrol ();

	}

	//Move pattern
	void Patrol() {

		Vector3 ds = -transform.right * patrolSpeed * Time.deltaTime;

		if (distTravelled >= patrolLength) {
			transform.Rotate(0, 180, 0);
			distTravelled = 0;
		}

		distTravelled += ds.magnitude;

		transform.Translate (ds, Space.World);
	}

	void Attack(Vector3 target)
	{
		if (animator.GetCurrentAnimatorStateInfo (0).IsTag ("Surprise"))
			return;
		
		//yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length + animator.GetCurrentAnimatorStateInfo(0).normalizedTime); 
		target = new Vector3(target.x, transform.position.y, target.z);

		Quaternion rotation90 = Quaternion.Euler (0, 90, 0);
		transform.position = Vector3.MoveTowards (transform.position, target, attackSpeed * Time.deltaTime);
		transform.rotation = Quaternion.LookRotation (rotation90 * (target - transform.position), Vector3.up);
	}

	public void TakeDamage(int amount, Vector3 hitPoint)
	{
		if (isDead)
			return;
		
		health -= amount;

		hitParticles.transform.position = hitPoint;
		hitParticles.Play ();

		if (health <= 0) {
			Death ();
		}
	}

	void OnCollisionEnter(Collision col)
	{
		if (col.gameObject.tag == "Player")
			col.gameObject.GetComponent<Player_Controller> ().TakeDamage (damage);
		else if (col.gameObject.tag == "Enemy") {
			Vector3 colNormal = new Vector3 (col.gameObject.transform.position.x - transform.position.x, 0, col.gameObject.transform.position.z - transform.position.z);

			Quaternion rotation = Quaternion.Euler (0, -90, 0);
			transform.rotation = Quaternion.LookRotation (rotation * Vector3.Reflect (-transform.right, colNormal), Vector3.up);
			col.gameObject.transform.rotation = Quaternion.LookRotation (rotation * Vector3.Reflect (-col.gameObject.transform.right, colNormal), Vector3.up);
		} else {
			transform.Rotate (0, 180, 0);
		}
	}

	void Death()
	{
		isDead = true;
		capsuleCollider.isTrigger = true;

		//Death animation?
		Destroy (gameObject);
	}
}
