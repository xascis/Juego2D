using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySlime : MonoBehaviour
{
    public float speed;
    public LayerMask groundMask;
    public LayerMask objectGroundMask;
    private float myWidth, myHeight;
    private Transform myTransform;

    private Animator myAnimator;
    private Rigidbody2D myBody;

    private AudioSource enemyDestroyed;

    void Start()
    {
        myAnimator = GetComponent<Animator>();
        myBody = GetComponent<Rigidbody2D>();
        myTransform = transform;
        SpriteRenderer mySpriteRenderer = GetComponent<SpriteRenderer>();
        myWidth = mySpriteRenderer.bounds.extents.x;
        myHeight = mySpriteRenderer.bounds.extents.y;

        // sonidos para cada situacíon
        var audioSource = GetComponents<AudioSource>();
        enemyDestroyed = audioSource[0];
    }

    void Update()
    {
        Vector2 lineCastPosition = myTransform.position - myTransform.right * myWidth + Vector3.up * myHeight;
        // dibuja una linea para saber si esta en el suelo
        Debug.DrawLine(lineCastPosition, lineCastPosition + Vector2.down);
        bool isGrounded = Physics2D.Linecast(lineCastPosition, lineCastPosition + Vector2.down, groundMask);
        // dibuja una linea para saber si delante tiene un obstáculo
        Debug.DrawLine(lineCastPosition,lineCastPosition - toVector2(myTransform.right) * 0.1f);
        bool isBlocked = Physics2D.Linecast(lineCastPosition, lineCastPosition - toVector2(myTransform.right) * 0.1f, groundMask);
        bool isObject = Physics2D.Linecast(lineCastPosition, lineCastPosition - toVector2(myTransform.right) * 0.1f, objectGroundMask);

        // da la vuelta al llegar a un obstaculo o se acaba el suelo
        if (!isGrounded || isBlocked || isObject)
        {
            Vector3 currentRotation = myTransform.eulerAngles;
            currentRotation.y += 180;
            myTransform.eulerAngles = currentRotation;
        }
        // Always move foward
        Vector2 myVelocity = myBody.velocity;
        myVelocity.x = - myTransform.right.x * speed;
        myBody.velocity = myVelocity;
    }

    private void OnTriggerEnter2D(Collider2D collider){
        if (collider.gameObject.tag == "Shoes" || collider.gameObject.tag == "Fireball")
        {
            if (GameManager.musicSettings){
                enemyDestroyed.Play();
            }

            myBody.bodyType = RigidbodyType2D.Static;
            gameObject.GetComponent<Collider2D>().enabled = false;
            myAnimator.SetTrigger("Dead");
            Destroy(gameObject, 2f);
        }
    }

    public Vector2 toVector2(Vector3 vector3)
    {
        return new Vector2(vector3.x, vector3.y);
    }
}