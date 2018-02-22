using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerMovement : MonoBehaviour
{

	private bool grounded = false;
	
	private Animator anim;
	
	public Transform groundCheck;
	public Rigidbody2D rb2d;
	public float sidewaysForce = 365f;
	public float jumpForce = 100f;
	public float maxSpeed = 5f;
	public float shootSpawnRate = 1f;
	private float timeToShoot = 0f;
	
	[HideInInspector] public bool facingRight = true;
	[HideInInspector] public bool jump = false;
	[HideInInspector] public bool shoot = false;
	
	// Use this for initialization
	void Start ()
	{
		rb2d = GetComponent<Rigidbody2D>();
		anim = GetComponent<Animator>();
	}

	private void Update()
	{

		grounded = Physics2D.Linecast(transform.position, groundCheck.position, 1 << LayerMask.NameToLayer("Ground"));

		if (Input.GetButtonDown("Jump") && grounded)
		{
			jump = true;
		}

		if (Input.GetButtonDown("Fire1") && Time.time > timeToShoot)
		{
			shoot = true;
		}
	}

	private void FixedUpdate ()
	{

		float horizontal = Input.GetAxis("Horizontal");
		
		anim.SetFloat("Speed", Mathf.Abs(horizontal)); //always ensures positive speed

		if (horizontal * rb2d.velocity.x < maxSpeed)	//move if under max speed
		{
			rb2d.AddForce(Vector2.right * horizontal * sidewaysForce);
		}

		//makes sure only move at max speed if we surpass it
		if (Mathf.Abs(rb2d.velocity.x) > maxSpeed)
		{
			rb2d.velocity = new Vector2(Mathf.Sign(rb2d.velocity.x) * maxSpeed, rb2d.velocity.y);
		}

		if (horizontal > 0 && !facingRight)
		{
			Flip();
		}
		else if (horizontal < 0 && facingRight)
		{
			Flip();
		}
		
		if (grounded)
		{
			anim.SetBool("Grounded", true);
		}
		
		if (jump)
		{
			anim.SetTrigger("Jump");
			GameMaster.PlaySound("Jump");
			rb2d.AddForce(new Vector2(0f, jumpForce));
			anim.SetBool("Grounded", false);
			jump = false;
		}

		if (shoot)
		{
			anim.SetTrigger("Shoot");
			timeToShoot = Time.time + 1 / shootSpawnRate;
			shoot = false;
		}
		
	}

	private void Flip()
	{
		facingRight = !facingRight;
		Vector3 scale = transform.localScale;
		scale.x *= -1;
		transform.localScale = scale;
	}
}
