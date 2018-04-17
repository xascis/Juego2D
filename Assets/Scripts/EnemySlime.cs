using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySlime : MonoBehaviour {
    public float time;
    public float distanceX = 1;

    private Animator animator;
    private Rigidbody2D rigidbody2D;

    public AudioSource enemyDestroyed;
    public AudioSource characterDamaged;

    public float directionX = 0;

    void Start () {
        animator = GetComponent<Animator>();
        rigidbody2D = GetComponent<Rigidbody2D>();

        // sonidos para cada situacíon
        var audioSource = GetComponents<AudioSource>();
        enemyDestroyed = audioSource[0];
        characterDamaged = audioSource[1];

        // movimiento del enemigo
        directionX = gameObject.transform.position.x;

        var destination = new Vector3();
        destination.Set(gameObject.transform.position.x + distanceX, gameObject.transform.position.y, gameObject.transform.position.z);

        iTween.MoveTo(gameObject, iTween.Hash("position", destination, "time", time, "easetype", "easeInOutSine", "looptype", "pingPong"));
    }
	
	void Update () {
        
        // girar la imagen según la dirección
        directionX = directionX - gameObject.transform.position.x;

        if (directionX < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
        else
        {
            transform.localScale = new Vector3(1, 1, 1);
        }

        directionX = gameObject.transform.position.x;
    }


    public void OnTriggerEnter2D(Collider2D collision)
    {
        // elimina el enemigo si le cae encima
        if (collision.gameObject.tag == "Character"
            && Math.Abs(gameObject.transform.position.x - collision.gameObject.transform.position.x) < 0.3
            && gameObject.transform.position.y - collision.gameObject.transform.position.y < 0)
        {
            collision.gameObject.GetComponent<Rigidbody2D>().AddForce(Vector2.up * 380f);

            if (GameManager.musicSettings)
                enemyDestroyed.Play();

            gameObject.GetComponent<Collider2D>().enabled = false;
            animator.SetBool("Dead", true);
            Destroy(gameObject, 1.5f);
        }

        // dañar jugador si toca el enemigo
        if (collision.gameObject.tag == "Character" 
            && Math.Abs(gameObject.transform.position.x - collision.gameObject.transform.position.x) >= 0.3)
        {
            if (gameObject.transform.position.x > collision.gameObject.transform.position.x)
            {
                collision.gameObject.GetComponent<Rigidbody2D>().AddForce(Vector2.left * 200f);
            }
            else
            {
                collision.gameObject.GetComponent<Rigidbody2D>().AddForce(Vector2.right * 200f);
            }

            if (GameManager.musicSettings)
                characterDamaged.Play();
            GameManager.currentNumberHearth--;

        }
    }

}
