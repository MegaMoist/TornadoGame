using UnityEngine;
using System.Collections;

public class Pickup : MonoBehaviour
{

    public Transform playerTransform;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (playerTransform != null)
        {
            this.transform.position = playerTransform.position;
        }

    }
}
