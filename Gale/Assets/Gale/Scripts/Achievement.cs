using UnityEngine;
using System.Collections;

public class Achievement : MonoBehaviour {

    public int score;
    public string achievementName;
    public string achievementDescription;
    public bool earned;

    public Achievement(string a_name, int a_score, string desc)
    {
        achievementName = a_name;
        achievementDescription = desc;
        score = a_score;
        earned = false;
    }
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    
}
