using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoes : MonoBehaviour
{
    public Rigidbody2D characterBody;

    // Use this for initialization
    void Start () {
        
    }
    
    // Update is called once per frame
    void Update () {
        
    }

    private void OnTriggerEnter2D(Collider2D collider){
        if (collider.gameObject.tag == "Enemy")
        {
            print("shoes collider");
            characterBody.AddForce(Vector2.up * 400f);

        }
    }
}
