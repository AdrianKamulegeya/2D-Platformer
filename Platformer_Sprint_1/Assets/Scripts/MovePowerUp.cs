using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePowerUp : MonoBehaviour
{

	public int moveSpeed = 20;
	public Transform player;

	private Vector3 direction;
	
	// Update is called once per frame

	void Awake()
	{
		player = GameObject.FindGameObjectWithTag("Player").transform;
		if (player.localScale.x > 0)
		{
			direction = Vector3.right;
		}
		else if (player.localScale.x < 0)
		{
			direction = Vector3.left;
		}
	}
	
	void Update () {
		transform.Translate(direction * Time.deltaTime * moveSpeed);
		Destroy(gameObject, 1);
	}
}
