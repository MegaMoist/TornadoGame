using UnityEngine;
using System.Collections;

public class CreditsScroll : MonoBehaviour
{
    public float CreditsScrollSpeed;

    private float yPos;
    Vector3 originalPos;

    private RectTransform thisTransform;
    // Use this for initialization
    void Start()
    {
       // originalPos = this.transform.position;
        thisTransform = GetComponent<RectTransform>();
    }

    void Awake()
    {
        originalPos = this.transform.position;
        yPos = 0;
    }




    // Update is called once per frame
    void LateUpdate()
    {
        if(this.transform.position.y >= thisTransform.sizeDelta.y + 800)
        {
            this.transform.position = originalPos;
            yPos = 0;
        }
        else
        {
            this.transform.Translate(0, CreditsScrollSpeed, 0);
            yPos += CreditsScrollSpeed;
        }

    }



    void OnDisable()
    {
        this.transform.position = originalPos;
    }
}
