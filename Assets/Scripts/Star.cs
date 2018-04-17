using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Star : MonoBehaviour {

    public static int starsLeft = 0; // static: variable global, solo existe una y se puede acceder a ella

	// Use this for initialization
	void Start () {
        starsLeft++;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
