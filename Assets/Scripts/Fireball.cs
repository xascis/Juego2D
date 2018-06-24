using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : MonoBehaviour
{
	public static int fireballNumber = 0;

	// Use this for initialization
	void Start ()
	{
		fireballNumber++;
	}
	
	// Update is called once per frame
	void Update () {

	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.gameObject.tag == "Enemy" || other.gameObject.layer == LayerMask.NameToLayer("Ground") || other.gameObject.tag == "Void")
		{
			fireballNumber--;
			Destroy(gameObject);
		}
	}
}
