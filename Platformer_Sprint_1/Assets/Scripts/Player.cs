using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

	
	[System.Serializable]
	public class PlayerStats
	{
		public int health = 100;
	}

	public PlayerStats stats = new PlayerStats();
	private PowerUp powers;
	private PlayerMovement movement;
	public int fallBounndary = -20;
	public float powerUpTimeLeft = 10f;

	private string previousPower = "";

	void Awake()
	{
		if (powers == null)
		{
			powers = GameObject.FindGameObjectWithTag("Power Up").GetComponent<PowerUp>();
		}

		if (movement == null)
		{
			movement = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
		}
	}

	void Update()
	{
		if (powers.currentPower != "")	//if currently using a power i.e. touched a gem
		{
			if (powers.currentPower != previousPower)
			{
				previousPower = powers.currentPower;
				ResetTimer();
			}
			
			powerUpTimeLeft -= Time.deltaTime;
			if (powerUpTimeLeft < 0)
			{
				powers.currentPower = "";
				ResetTimer();
			}
		}
		
		if (transform.position.y < fallBounndary)
		{
			DamageTaken(100);
		}
	}
	
	public void DamageTaken(int health)
	{
		stats.health -= health;
		if (stats.health <= 0)
		{
			Debug.Log("KILL PLAYER");
			GameMaster.KillPlayer(this);
		}
	}

	void ResetTimer()
	{
		powerUpTimeLeft = 10f;
	}

	private void OnCollisionEnter2D(Collision2D col)
	{
		switch (col.gameObject.tag)
		{
			case "Fire Gem":
				powers.currentPower = "Fire";
				GameMaster.PlaySound("PowerUp");
				Destroy(col.gameObject);
				return;
			case "Ice Gem":
				powers.currentPower = "Ice";
				GameMaster.PlaySound("PowerUp");
				Destroy(col.gameObject);
				return;
			case "Double Jump Gem":
				powers.currentPower = "Double Jump";
				GameMaster.PlaySound("PowerUp");
				Destroy(col.gameObject);
				return;
			case "Slow Gem":
				powers.currentPower = "Slow";
				GameMaster.PlaySound("PowerUp");
				//TODO: Slow down enemies
				Destroy(col.gameObject);
				return;
			case "Chest":
				//TODO: Do some sort of chest animation
				//TODO: Spawn some item - don't know what yet
				return;
		}
	}
	
}
