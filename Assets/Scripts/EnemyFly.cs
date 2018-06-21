using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFly : MonoBehaviour {

    public float speed;
    public float distance;
    private float initialPosition;
    private float lastPosition;
    private bool turn;
    private bool flying;
    // public LayerMask groundMask;
    private float myWidth, myHeight;
    private Transform myTransform;

    private Animator myAnimator;
    private Rigidbody2D myBody;

    private AudioSource enemyDestroyed;


    // Use this for initialization
    void Start () {
        myAnimator = GetComponent<Animator>();
        myBody = GetComponent<Rigidbody2D>();
        myTransform = transform;
        SpriteRenderer mySpriteRenderer = GetComponent<SpriteRenderer>();
        myWidth = mySpriteRenderer.bounds.extents.x;
        myHeight = mySpriteRenderer.bounds.extents.y;
        
        initialPosition = myTransform.position.x - distance;
        lastPosition = distance + myTransform.position.x;
        flying = true;
        turn = true;
    }
    
    // Update is called once per frame
    void Update () {
        print(initialPosition);

        Vector2 lineCastPosition = myTransform.position - myTransform.right * myWidth + Vector3.up * myHeight;
        Debug.DrawLine(lineCastPosition, lineCastPosition + Vector2.down);

        if (lineCastPosition.x < initialPosition || lineCastPosition.x > lastPosition)
        {
            Vector3 currentRotation = myTransform.eulerAngles;
            currentRotation.y += 180;
            myTransform.eulerAngles = currentRotation;
        }

        if (flying) {
            Vector2 myVelocity = myBody.velocity;
            myVelocity.x = - myTransform.right.x * speed;
            myBody.velocity = myVelocity;
        }
        
    }
}
