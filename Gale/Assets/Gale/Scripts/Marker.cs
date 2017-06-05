using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Marker : MonoBehaviour {

    public RectTransform sliderRect;
    private RectTransform markerRect;
	// Use this for initialization
    [ExecuteInEditMode]
	void Start () {
        markerRect = GetComponent<RectTransform>();
	}
	
    [ExecuteInEditMode]
	// Update is called once per frame
	void Update () {

        markerRect.anchoredPosition = new Vector2(sliderRect.rect.x * -2.0f, markerRect.rect.y);
	}
}
