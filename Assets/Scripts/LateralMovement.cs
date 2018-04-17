using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LateralMovement : MonoBehaviour
{
    public float destinationX;
    public float destinationY;
    public float destinationZ;
    public float time;

    // Use this for initialization
    void Start()
    {
        var destination = new Vector3();
        destination.Set(gameObject.transform.position.x + destinationX, gameObject.transform.position.y + destinationY, gameObject.transform.position.z + destinationZ);

        iTween.MoveTo(gameObject, iTween.Hash("position", destination, "time", time, "easetype", "easeInOutSine", "looptype", "pingPong"));
    }
}
