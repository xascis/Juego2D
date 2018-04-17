using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowDamping : MonoBehaviour {

    //public GameObject target;
    public float damping = 2f;
    Vector3 offset;
    private Transform character;
    private float startY;
    private Character characterScript;

    // Use this for initialization
    void Start () {
        character = GameObject.Find("Character").transform; // busca el personaje en la escena

        offset = transform.position - character.transform.position; // entre la posicion del personaje y la de la camara

        startY = Camera.main.WorldToScreenPoint(character.position).y; // la posicion del personaje la transforma en coordenadas de pantalla, cuando el personaje salta la camara no se mueve hacia arriba, peros si salta a un lugar mas alto se mueve la camara hacia arriba

        characterScript = character.GetComponent<Character>(); // hace referencia al script character
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private void LateUpdate()
    {
        Vector3 desiredPosition = character.transform.position + offset;

        Vector3 position = Vector3.Lerp(transform.position, (characterScript.grounded || Camera.main.WorldToScreenPoint(character.position).y < startY) ? desiredPosition : new Vector3(desiredPosition.x, transform.position.y,desiredPosition.z), Time.deltaTime * damping);

        transform.position = position;
    }
}
