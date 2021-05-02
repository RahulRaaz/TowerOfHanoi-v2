using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class detectCollision : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        setGravity(true);
    }
    void OnCollisionEnter(Collision disk)
    {
        setGravity(false);
    }

    void setGravity(bool toggle) //set default and gravity and kinematic to disk
    {
        GetComponent<Rigidbody>().useGravity = toggle;
        GetComponent<Rigidbody>().isKinematic = !toggle;
    }
}
