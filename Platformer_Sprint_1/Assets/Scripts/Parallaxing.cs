using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Parallaxing : MonoBehaviour
{

	public Transform[] backgrounds; 		// list of all backgrounds/foregrounds to be parallaxed
	private float[] parallaxScales;			// proportion of camera movement to move backgrounds by
	public float smoothing = 1f;			//How smooth parallax will be. Must be > 0.

	private Transform cam;					//ref. to main camera transform
	private Vector3 previousCamPos;			//store position of camera in previous frame.

	//called before Start(). Great for references.
	void Awake()
	{
		cam = Camera.main.transform;
	}
	
	
	// Use this for initialization
	void Start ()
	{
		previousCamPos = cam.position;		//store current frame

		parallaxScales = new float[backgrounds.Length];
	
		//assigning corresponding parallaxScales
		for (int i = 0; i < backgrounds.Length; i++)
		{
			parallaxScales[i] = backgrounds[i].position.z * -1;
		}
	}
	
	// Update is called once per frame
	void Update ()
	{
		float parallax;
		float backgroundTargetPosX;
		Vector3 backgroundTargetPos;
		
		for (int i = 0; i < backgrounds.Length; i++)
		{
			//parallax is opposite of camera movement
			parallax = (previousCamPos.x - cam.position.x) * parallaxScales[i];
			
			//set target X position
			backgroundTargetPosX = backgrounds[i].position.x + parallax; 
			
			//create target position
			backgroundTargetPos = new Vector3(backgroundTargetPosX,backgrounds[i].position.y,backgrounds[i].position.z);
			
			//fade between target Pos and current Pos using Lerp
			backgrounds[i].position = Vector3.Lerp (backgrounds[i].position, backgroundTargetPos, smoothing * Time.deltaTime);
		}

		//set previousCamPos to camera position at the end of the frame
		previousCamPos = cam.position;

	}
}
