using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

	public EnemyStats stats = new EnemyStats();
	public LayerMask whatToHit;
	public string weakTo = "Fireball";
	[HideInInspector] public bool facingRight = true;
	
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

	private void FixedUpdate()
	{	
		Vector2 startPosition = new Vector2(transform.position.x, transform.position.y);
		Vector2 endPositionLeft = new Vector2(transform.position.x - 1, transform.position.y);
		Vector2 endPositionRight = new Vector2(transform.position.x + 1, transform.position.y);
		
		RaycastHit2D leftHit = Physics2D.Raycast(startPosition, endPositionLeft - startPosition, 100, whatToHit);
		RaycastHit2D rightHit = Physics2D.Raycast(startPosition, endPositionRight - startPosition, 100, whatToHit);
		
		
		if (rightHit.collider != null && !facingRight)
		{
			Flip();
		}
		else if (leftHit.collider != null && facingRight)
		{
			Flip();
		}
	}
	
	private void Flip()
	{
		facingRight = !facingRight;
		Vector3 scale = transform.localScale;
		scale.x *= -1;
		transform.localScale = scale;
	}
	
	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.gameObject.CompareTag(weakTo))
		{
			Debug.Log("HIT ENEMY");
			PowerUp powerUp = FindObjectOfType<PowerUp>();
			damageTaken(powerUp.damage);
			Destroy(other.gameObject);
		}
	}

	private void OnCollisionEnter2D(Collision2D other)
	{
		if (!other.gameObject.CompareTag("Player"))
		{
			if (!other.gameObject.CompareTag("Obstacle"))
			{
				if (!other.gameObject.CompareTag("Chest"))
				{
					Physics2D.IgnoreCollision(other.gameObject.GetComponent<Collider2D>(), GetComponent<Collider2D>());
				}
			}
		}
	}
}
