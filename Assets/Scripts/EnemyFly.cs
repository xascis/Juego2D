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

        // sonidos
        var audioSource = GetComponents<AudioSource>();
        enemyDestroyed = audioSource[0];

    }
    
    // Update is called once per frame
    void Update () {
        if (myTransform.position != positionList[_current]) {
            myTransform.position = Vector3.MoveTowards(myTransform.position, positionList[_current], speed * Time.deltaTime);
        }
        else
        {
            // gira al acabar el recorrido
            Vector3 currentRotation = myTransform.eulerAngles;
            currentRotation.y += 180;
            myTransform.eulerAngles = currentRotation;

            _current = (_current + 1) % positionList.Count;
        }

    }

    private void OnTriggerEnter2D(Collider2D collider){
        if (collider.gameObject.tag == "Shoes")
        {
            // collision.gameObject.myBody.AddForce(Vector2.up * 100f);

            if (GameManager.musicSettings){
                enemyDestroyed.Play();
            }

            myBody.bodyType = RigidbodyType2D.Dynamic;
            gameObject.GetComponent<Collider2D>().enabled = false;
            myAnimator.SetBool("Dead", true);
            Destroy(gameObject, 3f);
        }
    }
}
