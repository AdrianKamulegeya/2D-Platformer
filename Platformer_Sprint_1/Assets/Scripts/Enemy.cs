using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

	public EnemyStats stats = new EnemyStats();
	
	[System.Serializable]
	public class EnemyStats
	{
		public int health = 100;
	}
	
	public void damageTaken(int damage)
	{
		stats.health -= damage;
		if (stats.health <= 0)
		{
			GameMaster.KillEnemy(this);
		}
	}
}
