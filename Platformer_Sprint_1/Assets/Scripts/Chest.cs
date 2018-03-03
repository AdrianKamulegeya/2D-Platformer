using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{

	public bool opened = false;
	public bool empty = false;
	private Animator anim; 
	
	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void OpenChest()
	{
		opened = true;
		anim.SetBool("opened", true);
	}

	public void EmptyChest()
	{
		if (opened == true)
		{
			anim.SetBool("empty", true);
			empty = true;
		}
	}
}
