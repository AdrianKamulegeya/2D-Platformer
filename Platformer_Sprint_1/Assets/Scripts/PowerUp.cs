 using System.Collections;
using System.Collections.Generic;
 using System.ComponentModel;
 using UnityEngine;
 using UnityEngine.UI;

public class PowerUp : MonoBehaviour {

	public float fireRate = 0f;
	public int damage = 10;
	public LayerMask whatToHit;
	public float effectSpawnRate = 10;
	
	public Transform fireballPrefab;
	public Transform iceballPrefab;
	
	private float timeToFire = 0;
	private float timeToSpawnEffect = 0;
	private float initialJumpForce = 300f;
	private PlayerMovement movement;
	
	private Transform firePoint;
	private Transform player;

	public string currentPower = "";

	void Awake()
	{
		player = GameObject.FindGameObjectWithTag("Player").transform;
		firePoint = transform.Find("FirePoint");
		if (firePoint == null)
		{
			Debug.LogError("Welp. Where's the FirePoint?");
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (fireRate == 0)
		{
			if (Input.GetButtonDown("Fire1") && currentPower != "")
			{
				//TODO: add animation for shooting
				Shoot();
			}
		}
		else
		{
			if (Input.GetButton("Fire1") && Time.time > timeToFire)
			{
				//TODO: add animation for shooting
				timeToFire = Time.time + 1 / fireRate;
				Shoot();
			}
		}

		if (Input.GetButtonDown("Jump"))
		{
			SetDoubleJumpEffect();
		}
	}

	void Shoot()
	{
		Debug.Log("FIRE");
		float direction = 1;
		if (player.localScale.x > 0)
		{
			direction *= 1;
		}
		else if (player.localScale.x < 0)
		{
			direction *= -1;
		}
		
		Vector2 endPosition = new Vector2(firePoint.position.x + direction, firePoint.position.y);
		Vector2 firePointPosition = new Vector2(firePoint.position.x, firePoint.position.y);
		RaycastHit2D hit = Physics2D.Raycast(firePointPosition, endPosition - firePointPosition, 100, whatToHit);

		Debug.DrawLine(firePointPosition, (endPosition-firePointPosition) * 100, Color.blue);
		
		if (hit.collider != null)
		{
			Debug.DrawLine(firePointPosition, hit.point, Color.red);
			Debug.Log("We hit " + hit.collider.name + " and did " + damage + " damage");
		}
		
		if (Time.time > timeToSpawnEffect)
		{
			Effect();
			timeToSpawnEffect = Time.time + 1 / effectSpawnRate;
		} 
		
	}

	void Effect()
	{
		if (currentPower == "Fire")
		{
			GameMaster.PlaySound("Fireball");
			Instantiate(fireballPrefab, firePoint.position, firePoint.rotation);
		}
		else if (currentPower == "Ice")
		{
			GameMaster.PlaySound("Ice");
			Instantiate(iceballPrefab, firePoint.position, firePoint.rotation);
		}
	}

	void SetDoubleJumpEffect()
	{
		movement = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
		if (currentPower == "Double Jump")
		{
			movement.jumpForce = initialJumpForce * 2;
		}
		else if (currentPower == "")
		{
			movement.jumpForce = initialJumpForce;
		}
	}

}
