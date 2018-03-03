using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMaster : MonoBehaviour
{

	public static GameMaster gm;
	public Transform playerPrefab;
	public Transform spawnPoint;
	public float spawnDelay = 2f;
	
	private static AudioSource[] audio;
	
	static Dictionary<string, int> audioFiles = new Dictionary<string, int>();
	
	void Awake()
	{
		audio = GetComponents<AudioSource>();
		audioFiles = new Dictionary<string, int>();
		InitialiseAudioFiles();
		PlaySound("Background");
	}
	
	void Start()
	{
		if (gm == null)
		{
			gm = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameMaster>();
		}
	}
	
	
	public static void KillPlayer(Player player)
	{
		Destroy(player.gameObject);
		gm.StartCoroutine(gm.RespawnPlayer());
	}
	
	public static void KillEnemy(Enemy enemy)
	{
		Destroy(enemy.gameObject);	
	}

	public IEnumerator RespawnPlayer()
	{
		PlaySound("Death");
		yield return new WaitForSeconds(spawnDelay);
		Instantiate(playerPrefab, spawnPoint.position, spawnPoint.rotation);
	}

	public static void PlaySound(string sound)
	{
		int number = audioFiles[sound];
		audio[number].Play();
	}

	private void InitialiseAudioFiles()
	{
		audioFiles["Background"] = 0;
		audioFiles["Death"] = 1;
		audioFiles["Jump"] = 2;
		audioFiles["Fireball"] = 3;
		audioFiles["Ice"] = 4;
		audioFiles["PowerUp"] = 5;
		audioFiles["Open Chest"] = 6;
		audioFiles["Empty Chest"] = 7;
	}

}
