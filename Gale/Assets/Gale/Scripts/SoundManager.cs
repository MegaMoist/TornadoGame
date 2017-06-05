using UnityEngine;
using System.Collections;

public class SoundManager : MonoBehaviour {

  
    public AudioSource MenuSound;
    public AudioSource MenuMusic;
    public AudioSource T1WindSound;
    public AudioSource T2WindSound;
    public AudioSource T3WindSound;
    
    public AudioSource TierTransition;
   
    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	    
	}

    public void PlaySound(string Sound)
    {
        switch (Sound)
        {
            case "MenuSound":
                MenuSound.Play();
                break;
            case "MenuMusic":
                MenuMusic.Play();
                break;
            case "T1Wind":
                T1WindSound.Play();
                break;
            case "T2Wind":
                MenuSound.Play();
                break;
            case "T3Wind":
                MenuSound.Play();
                break;
            case "TierTransition":
                TierTransition.Play();
                break;
        }

    }


    public void FadeOut(string Sound, float speed)
    {
        bool Fading = true;
        switch (Sound)
        {
            case "MenuSound":
                if (Fading)
                {
                    MenuSound.volume -= speed * Time.deltaTime;
                    if (MenuSound.volume <= 0)
                    {
                        MenuSound.Stop();
                        Fading = false;
                    }
                }
                break;
            case "MenuMusic":
                if (Fading)
                {
                    MenuMusic.volume -= speed * Time.deltaTime;
                    if (MenuMusic.volume <= 0)
                    {
                        MenuMusic.Stop();
                        Fading = false;
                    }
                }
                break;
            case "T1Wind":
                if (Fading)
                {
                    T1WindSound.volume -= speed * Time.deltaTime;
                    if (T1WindSound.volume <= 0)
                    {
                        T1WindSound.Stop();
                        Fading = false;
                    }
                }
                break;
            case "T2Wind":
                if (Fading)
                {
                    T2WindSound.volume -= speed * Time.deltaTime;
                    if (T2WindSound.volume <= 0)
                    {
                        T2WindSound.Stop();
                        Fading = false;
                    }
                }
                break;
            case "T3Wind":
                if (Fading)
                {
                    T2WindSound.volume -= speed * Time.deltaTime;
                    if (T2WindSound.volume <= 0)
                    {
                        T2WindSound.Stop();
                        Fading = false;
                    }
                }
                break;
        }
    }

    public void FadeIn(string Sound, float speed)
    {
        bool Fading = true;
        switch (Sound)
        {
            case "MenuSound":
                if (Fading)
                {
                    MenuSound.volume += speed * Time.deltaTime;
                    if (MenuSound.volume <= 0)
                    {
                       
                        Fading = false;
                    }
                }
                break;
            case "MenuMusic":
                if (Fading)
                {
                    MenuMusic.volume += speed * Time.deltaTime;
                    if (MenuMusic.volume <= 0)
                    {
                       
                        Fading = false;
                    }
                }
                break;
            case "T1Wind":
                if (Fading)
                {
                    
                    T1WindSound.volume += speed * Time.deltaTime;
                    if (T1WindSound.volume <= 0)
                    {
                        
                        Fading = false;
                    }
                }
                break;
            case "T2Wind":
                if (Fading)
                {

                    T2WindSound.volume += speed * Time.deltaTime;
                    if (T2WindSound.volume <= 0)
                    {

                        Fading = false;
                    }
                }
                break;
            case "T3Wind":
                if (Fading)
                {

                    T3WindSound.volume += speed * Time.deltaTime;
                    if (T3WindSound.volume <= 0)
                    {

                        Fading = false;
                    }
                }
                break;
        }
    }

    public void StopSound(string Sound)
    {
        switch (Sound)
        {
            case "MenuSound":
                MenuSound.Stop();
                break;
            case "MenuMusic":
                MenuMusic.Stop();
                break;
            case "T1Wind":
                MenuSound.Stop();
                break;
            case "T2Wind":
                MenuSound.Stop();
                break;
            case "T3Wind":
                MenuSound.Stop();
                break;
        }

    }
}
