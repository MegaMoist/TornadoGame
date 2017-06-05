using UnityEngine;
using System.Collections;

public class Rotator : MonoBehaviour
{
    public float x;
    public float y;
    public float z;


    // Use this for initialization
    void Start()
    {
       
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        this.transform.Rotate(x,y,z);
        
    }
}
