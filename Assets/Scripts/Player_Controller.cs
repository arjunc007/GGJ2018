using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Controller : MonoBehaviour
{
    //Radar Variables
    public bool inRadar1, inRadar2, inRadar3, beeping = false;
    private AudioSource radar;

    //Movement Variables
    [SerializeField]
    private float moveSpeed;
    private float minSpeed = 3f;
    private float maxSpeed = 5f;

    [SerializeField]
    private float jumpSpeed = 5f;

    private bool grounded;
    private Rigidbody rb;

    //Skills Variables - Teleporting
    [SerializeField]
    private GameObject teleportParticles;
    private GameObject home;
    private bool teleporting;

    //Sandy - Holding Component Variables
    public bool hasComponent;
    public string componentName;

    //Arjun - Health And Combat Variables
    public float health;
	[SerializeField]
	private const int damage = 20;
	public float oxygen;
	public float timeBetweenBullets = 0.15f;
	bool isDead;

	Animator animator;
	float timer;
	private float range = 100f;
	int shootableMask;
	private Ray shootRay;
	RaycastHit shootHit;
	LineRenderer gunLine;
	public float effectsDisplayTime = 0.2f;

    private void Awake()
    {
        //Base Setting For Teleportation
        home = GameObject.FindGameObjectWithTag("Home");

        //Sound Setting
        GameObject[] sfxs = GameObject.FindGameObjectsWithTag("SFX");
        foreach (GameObject go in sfxs)
        {
            if (go.name == "Radar_Sound")
            {
                radar = go.GetComponent<AudioSource>();
            }
        }
    }

    private void Start ()
    {

		animator = GetComponent<Animator> ();
        //Shooting
		shootableMask = LayerMask.GetMask ("Shootable");
		gunLine = GetComponent<LineRenderer> ();
        rb = this.GetComponent<Rigidbody>();
		isDead = false;
	}
	
	private void Update ()
    {
        //Game Loop Essentially
		if (!isDead) {
			timer += Time.deltaTime;

            //Component Effect
            if (hasComponent)
            {
                moveSpeed = minSpeed;
                inRadar1 = inRadar2 = inRadar3 = false;
            }
            else
            {
                moveSpeed = maxSpeed;
            }

            //Radar
            if (inRadar1 && !beeping && inRadar2 && inRadar3)
                StartCoroutine(Beep(0.5f));
            else if (inRadar2 && !beeping && inRadar3 && !inRadar1)
                StartCoroutine(Beep(1f));
            else if (inRadar3 && !beeping && !inRadar1 && !inRadar2)
                StartCoroutine(Beep(1.5f));

            //Input
            if (Input.GetKey (KeyCode.W) && !teleporting) {
				this.gameObject.transform.position += gameObject.transform.forward * (moveSpeed * Time.deltaTime);
				animator.SetTrigger ("walking");

			} //Movement
			if (Input.GetKey (KeyCode.S) && !teleporting) {
				this.gameObject.transform.position -= gameObject.transform.forward * (moveSpeed * Time.deltaTime);
				animator.SetTrigger ("walking");
			} //Movement
			if (Input.GetKey (KeyCode.A) && !teleporting) {
				this.gameObject.transform.Rotate (0, -5, 0);
			} //Rotation
			if (Input.GetKey (KeyCode.D) && !teleporting) {
				this.gameObject.transform.Rotate (0, 5, 0);
			} //Rotation
			if (Input.GetKeyDown (KeyCode.Space) && grounded && !teleporting) {
				rb.velocity += jumpSpeed * Vector3.up;
			} //Jumping
			if (Input.GetKeyDown (KeyCode.LeftControl)) {
				Shoot ();
			} //Shooting Controls
            if (Input.GetKeyDown(KeyCode.F) && !teleporting && grounded && !hasComponent)
            {
                teleporting = true;
                StartCoroutine(Teleport());
            } //Teleporting

            //Shooting
			if (timer >= timeBetweenBullets * effectsDisplayTime) {
				DisableEffects ();
			}

			if (timer >= 1f) {
				timer = 0;
				//oxygen--;
			}

			if (oxygen <= 0)
				isDead = true;
		}
    }

    //Radar Beeping
    IEnumerator Beep(float f)
    {
        beeping = true;
        if (f == 1.5f)
        {
            radar.PlayOneShot(radar.clip, 0.1f);
            Debug.Log("Beep");
        }
        else if (f == 1f)
        {
            radar.PlayOneShot(radar.clip, 0.1f);
            Debug.Log("Loud Beep");
        }
        else if (f == 0.5f)
        {
            radar.PlayOneShot(radar.clip, 0.1f);
            Debug.Log("Loudest Beep");
        }
        yield return new WaitForSeconds(f);
        beeping = false;
    }

    //Teleporting
    IEnumerator Teleport()
    {
        Instantiate(teleportParticles, new Vector3(this.gameObject.transform.position.x, this.gameObject.transform.position.y - 2f, this.gameObject.transform.position.z), new Quaternion(0, 0, 0, 0));
        yield return new WaitForSeconds(3f);
        this.gameObject.transform.position = home.transform.position;
        teleporting = false;
        GameObject go = GameObject.FindGameObjectWithTag("Teleport");
        Destroy(go);
    }

    void DisableEffects ()
	{
		gunLine.enabled = false;
	}

    //Grounded Check
    private void OnCollisionStay(Collision col)
    {
        if (col.gameObject.tag == "Ground")
        {
            grounded = true;
        }
    }

    //Grounded Check
    private void OnCollisionExit(Collision col)
    {
        if (col.gameObject.tag == "Ground")
        {
            grounded = false;
        }
    }

    //Sandy-Collider for components ~~~~~~~~~~ MOVED ELSEWHERE
    /*private void OnTriggerEnter(Collider other)
    {
        if (this.tag == "Player")
        {
            if (other.tag == "Component")
            {
                hasComponent = true;
                Destroy(other.gameObject);
            }
            else if (other.tag == "Health")
            {
                health += other.gameObject.GetComponent<Item>().value;
                Destroy(other.gameObject);
            }
            else if (other.tag == "Item")
            {
                oxygen += other.gameObject.GetComponent<Item>().value;
                Destroy(other.gameObject);
            }
        }
    }*/

    //Shooting
	void Shoot()
	{
		timer = 0f;
		gunLine.enabled = true;
		gunLine.SetPosition (0, transform.position);

		shootRay.origin = transform.position;
		shootRay.direction = transform.forward;

		if (Physics.Raycast (shootRay, out shootHit, range, shootableMask)) {
			Enemy enemy = shootHit.collider.GetComponent<Enemy> ();

			if (enemy != null) {
				enemy.TakeDamage (damage, shootHit.point);
			}

			gunLine.SetPosition (1, shootHit.point);
		} else {
			gunLine.SetPosition (1, shootRay.origin + shootRay.direction * range);
		}
	}

    //Combat
	public void TakeDamage(int damage)
	{
		health -= damage;

		if (health <= 0)
			Death ();
	}

    //Death
	void Death()
	{
		isDead = true;
        Application.LoadLevel("Menu");
    }
}
