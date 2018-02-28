using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

	
	[System.Serializable]
	public class PlayerStats
	{
		public int health = 3;
	}

	public PlayerStats stats = new PlayerStats();
	private PowerUp powers;
	private PlayerMovement movement;
	public int fallBounndary = -20;
	public float powerUpTimeLeft = 10f;
	
	private string previousPower = "";
	private bool chestTriggerEntered = false;
	private Chest currentChest;

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
		
		if (transform.position.y < fallBounndary)
		{
			DamageTaken(100);
		}

		if (Input.GetButtonDown("Submit") && chestTriggerEntered)
		{
				if (!currentChest.opened)
				{
					Debug.Log("OPEN");
					GameMaster.PlaySound("Open Chest");
					currentChest.OpenChest();
				}
				else if (!currentChest.empty)
				{
					Debug.Log("EMPTY");
					//GameMaster.PlaySound("Empty Chest");
					currentChest.EmptyChest();
				}
				
			//TODO: Do some sort of chest animation
			//TODO: Spawn some item - don't know what yet

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
				Destroy(col.gameObject);
				return;
			case "Enemy":
				powers.currentPower = "";
				//TODO: Add hurt sound
				DamageTaken(1);
				return;
		}
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		chestTriggerEntered = true;
		currentChest = other.gameObject.GetComponent<Chest>();
	}

	private void OnTriggerExit2D(Collider2D other)
	{
		chestTriggerEntered = false;
	}
}
