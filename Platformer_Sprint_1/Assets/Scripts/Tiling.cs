using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]

public class Tiling : MonoBehaviour
{

	public int offsetX = 2;				//offset so we don't get wierd errors
	public bool hasARightBuddy = false;		//check if we need to instantiate stuff
	public bool hasALeftBuddy = false;

	public bool reverseScale = false;		//used if object not tilable
	
	private float spriteWidth = 0f;		//width of texture

	private Camera cam;
	private Transform myTransform;

	void Awake()
	{
		cam = Camera.main;
		myTransform = transform;
	}
	
	// Use this for initialization
	void Start () {
		SpriteRenderer sRenderer = GetComponent<SpriteRenderer>();
		spriteWidth = sRenderer.sprite.bounds.size.x;
	}
	
	// Update is called once per frame
	void Update () {
		
		if (hasALeftBuddy == false || hasARightBuddy == false)
		{
			//calculate camera extend, 1/2 the width, of what camera can see in world co-ordinates
			float camHorizontalExtend = cam.orthographicSize * Screen.width / Screen.height;

			//calculate x position where camera can see edge of sprite
			float edgeVisiblePositionRight = (myTransform.position.x + spriteWidth / 2) - camHorizontalExtend;
			float edgeVisiblePositionLeft = (myTransform.position.x - spriteWidth / 2) + camHorizontalExtend;
	
			//checking if we can see edge of element
			if (cam.transform.position.x >= edgeVisiblePositionRight - offsetX && !hasARightBuddy)
			{
				MakeNewBuddy(1);
				hasARightBuddy = true;
			}else if (cam.transform.position.x <= edgeVisiblePositionLeft + offsetX && !hasALeftBuddy)
			{
				MakeNewBuddy(-1);
				hasALeftBuddy = true;
			}
		}

	}


	/*
	 *  creates buddy on side required
	 */
	
	void MakeNewBuddy(int direction)
	{
		Vector3 newPosition = new Vector3(myTransform.position.x + spriteWidth * direction,
			myTransform.position.y,myTransform.position.z);
		
		Transform newBuddy =  Instantiate(myTransform, newPosition,myTransform.rotation);

		
		 //if not tilable, reverse x size to get rid of seams
		if (reverseScale == true)
		{
			newBuddy.localScale = new Vector3(newBuddy.localScale.x * -1, newBuddy.localScale.y, newBuddy.localScale.z);
		}

		newBuddy.parent = myTransform.parent;

		if (direction > 0)
		{
			newBuddy.GetComponent<Tiling>().hasALeftBuddy = true;
		}
		else
		{
			newBuddy.GetComponent<Tiling>().hasARightBuddy = true;
		}
	}

}
