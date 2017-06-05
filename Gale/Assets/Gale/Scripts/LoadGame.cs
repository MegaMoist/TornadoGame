using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class LoadGame : MonoBehaviour {
    public bool Done = false;

    public float buffer = 5;
    public string LevelName;

    // Use this for initialization
    void Start () {
        SceneManager.LoadSceneAsync(LevelName);
    }
	
	// Update is called once per frame
	void Update () {
        buffer -= 1 * Time.deltaTime;
       
        if(buffer <= 0)
        {
            Done = true;
        }

	}
}
