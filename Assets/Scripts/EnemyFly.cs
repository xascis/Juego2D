using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFly : MonoBehaviour {

    public float speed;
    public float distance;
    private List<Vector3> positionList;
    private int _current = 0;
    private bool flying;

    // public LayerMask groundMask;
    private float myWidth, myHeight;
    private Transform myTransform;

    private Animator myAnimator;
    private Rigidbody2D myBody;

    private AudioSource enemyDestroyed;


    // Use this for initialization
    void Start ()
    {
        myAnimator = GetComponent<Animator>();
        myBody = GetComponent<Rigidbody2D>();
        myTransform = transform;
        SpriteRenderer mySpriteRenderer = GetComponent<SpriteRenderer>();
        myWidth = mySpriteRenderer.bounds.extents.x;
        myHeight = mySpriteRenderer.bounds.extents.y;

        positionList = new List<Vector3>();
        positionList.Add(new Vector2(myTransform.position.x - distance, myTransform.position.y));
        positionList.Add(new Vector2(myTransform.position.x + distance, myTransform.position.y));
        flying = true;

    }
    
    // Update is called once per frame
    void Update () {
//        print(initialPosition);

//        Vector2 lineCastPosition = myTransform.position - myTransform.right * myWidth + Vector3.up * myHeight;
//        Debug.DrawLine(lineCastPosition, lineCastPosition + Vector2.down);

        // gira
//        if (lineCastPosition.x < initialPosition || lineCastPosition.x > lastPosition)
//        {
//            Vector3 currentRotation = myTransform.eulerAngles;
//            currentRotation.y += 180;
//            myTransform.eulerAngles = currentRotation;
//        }
        // está moviendose
        if (myTransform.position != positionList[_current]) {
            myTransform.position = Vector3.MoveTowards(myTransform.position, positionList[_current], speed * Time.deltaTime);
        }
        else
        {
            Vector3 currentRotation = myTransform.eulerAngles;
            currentRotation.y += 180;
            myTransform.eulerAngles = currentRotation;

            _current = (_current + 1) % positionList.Count;


            print(currentRotation);
        }

    }
}
