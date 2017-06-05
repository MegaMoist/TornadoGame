using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine.UI;

public class GameStateManager : MonoBehaviour
{
    [System.Serializable]


    public class GameStats
    {
        public Text windDescriptiontext;
        public string tinyWind;
        public int tinyWindSize;
        public string smallWind;
        public int smallWindSize;
        public string mediumWind;
        public int mediumWindSize;
        public string LargeWind;
        public int largeWindSize;
        public string HugeWind;
        public int HugeWindSize;

        public Text gameTime;
    }

    public PlayerPrefs m_playerSaveScore;
    public SoundManager sound;
    public Camera MenuCam;
    public Camera MainCam;
    public Transform[] camPositions;
    public int randomPosNumber;

    public Transform TransitionEndPos;
    public Transform TransitionStartPosition;
    public float smoothing;
    public GameObject player;
    public PlayerScript playerScript;
    public Text TitleImage;
    public Image EndBackdrop;
    public Image LoseBackdrop;
    public Canvas HighScoreGameUI;
    public Canvas StartGameUI;
    public Canvas EndGameUI;
    public Canvas LoseGameUI;
    public Canvas PauseGameUI;
    public Canvas InGameUI;
    public Slider SizeIndicator;

    public Text DamageDisplay;
    public Text HighScoreTime;
    public GameManager game;

    public bool Paused;

    public Image T3Indicator;
    public Image T2Indicator;
    public Image T1Indicator;

    public Gradient colorGradient;    

    public float FadeTimer = 5;
    public float dealthDelay;
    public float dealthDelayCooldown;


    public bool EndTransition;
    public float EndTransitionCooldown;
    public float EndTransitionTimer;
    [System.Serializable]
    public class Score
    {
        public string name;
        public float score;
    }

    public ArrayList scores;
    public float TransitionCooldown;
    public bool Transition;
    public float TransitionTimer;
    public enum GameState
    {
        StartGame,
        InGame,
        EndGame,
        Paused,
        HighScore,
        Lose,
    }

    public GameStats gameStats;

    public GameState gameState;

    // Use this for initialization
    void Start()
    {
        scores = new ArrayList();
        SaveToFile();

        LoadFromFile();
        randomPosNumber = Random.Range(0, 4);
        //camPositions = new Transform[4];
        MenuCam.transform.position = camPositions[randomPosNumber].position;
        MenuCam.transform.rotation = camPositions[randomPosNumber].rotation;
        game = this.GetComponent<GameManager>();
        sound = GameObject.Find("SoundManager").GetComponent<SoundManager>();
       
        player = GameObject.FindGameObjectWithTag("Player");
        playerScript = player.GetComponent<PlayerScript>();
        DamageDisplay = GameObject.Find("DamageCausedDisplay").GetComponent<Text>();
       
        dealthDelay = dealthDelayCooldown;
        SwitchState("Start");
    }

    // Update is called once per frame
    void Update()
    {
        switch (gameState)
        {
            case GameState.StartGame:
                StartGame();
                break;
            case GameState.InGame:
                InGame();
                break;
            case GameState.EndGame:
                EndGame();
                break;
            case GameState.Paused:
                Pause();
                break;
            case GameState.Lose:
                Lose();
                break;
        }
    }

    void UpdateScores()
    {
       
    }
    private void SaveToFile()
    {
        string[] scores = new string[3];

        scores[0] = "Todd 1000";
        scores[1] = "John 200";
        scores[2] = "Trent 800";

        File.WriteAllLines(Application.dataPath + "/../savedata.txt", scores);
    }


    private void LoadFromFile()
    {
        if (File.Exists(Application.dataPath + "/../savedata.txt"))
        {

            string[] scores = File.ReadAllLines(Application.dataPath + "/../savedata.txt");

            foreach(string s in scores)
            {

                string[] tokens = s.Split(' ');

                //check if tokens array actually has two elements
                //Debug.Log("name: " + tokens[0] + " scored " + tokens[1]);

                int scoreNum = 0;

                if( int.TryParse(tokens[1], out scoreNum) )
                {
                 //   Debug.Log("SCORE AS NUMBER: " + scoreNum);
                }
            }
        }


    }
    //Checks for switching to different states and applies values
    public void SwitchState(string state)
    {
        switch (state)
        {
            case "Start":
                Time.timeScale = 1;
                sound.PlaySound("MenuSound");
                sound.PlaySound("MenuMusic");
                sound.StopSound("T1WindSound");
                sound.StopSound("T2WindSound");
                sound.StopSound("T3WindSound");
                player.SetActive(false);
                InGameUI.gameObject.SetActive(false);
                StartGameUI.gameObject.SetActive(true);
                EndGameUI.gameObject.SetActive(false);
                PauseGameUI.gameObject.SetActive(false);
                LoseGameUI.gameObject.SetActive(false);
                MenuCam.gameObject.GetComponent<AudioListener>().enabled = true;
                MainCam.gameObject.GetComponent<AudioListener>().enabled = false;
                MainCam.gameObject.GetComponent<Camera>().enabled = false;
                break;
            //Transition to In Game
            case "InGame":
                MainCam.gameObject.SetActive(true);
                MainCam.gameObject.GetComponent<Camera>().enabled = true;
                MainCam.gameObject.GetComponent<AudioListener>().enabled = true;
                MenuCam.gameObject.GetComponent<AudioListener>().enabled = false;
                InGameUI.gameObject.SetActive(true);
                PauseGameUI.gameObject.SetActive(false);
                LoseGameUI.gameObject.SetActive(false);
                player.SetActive(true);
                Transition = false;
                StartGameUI.gameObject.SetActive(false);
                EndGameUI.gameObject.SetActive(false);
                TransitionTimer = TransitionCooldown;
                gameState = GameState.InGame;
                MenuCam.gameObject.SetActive(false);
                break;
            case "EndGame":
                StartGameUI.gameObject.SetActive(false);
                EndGameUI.gameObject.SetActive(true);
                gameState = GameState.EndGame;
                EndTransition = true;
              
                break;
            case "Lose":
                LoseGameUI.gameObject.SetActive(true);
                gameState = GameState.Lose;
                break;
            case "Pause":
                InGameUI.gameObject.SetActive(false);
                Paused = true;
                PauseGameUI.gameObject.SetActive(true);
                gameState = GameState.Paused;
                break;
           
        }

    }
    void StartGame()
    {
       
        player.SetActive(false);
        if (Input.anyKey && Transition == false)
        {
            Transition = true;
            //game.GrabReferences();
            game.SetupGame();
            sound.PlaySound("T1Wind");
        }
        if (Transition)
        {
            sound.FadeOut("MenuSound", 0.1f);
            sound.FadeOut("MenuMusic", 0.2f);
            sound.FadeIn("T1Wind", 0.2f);
            TitleImage.CrossFadeAlpha(0, FadeTimer, true);
            FadeTimer -= 1 *  Time.deltaTime;
            if(FadeTimer <= 0)
            {
                MenuCam.transform.position = Vector3.Lerp(MenuCam.transform.position, TransitionEndPos.position, smoothing);
                MenuCam.transform.rotation = Quaternion.Lerp(MenuCam.transform.rotation, TransitionEndPos.rotation, smoothing);
                TransitionTimer -= 1 * Time.deltaTime;
            }
            if (TransitionTimer <= 0)
            {
                SwitchState("InGame");
            }
        }
    }

  

    void EndGame()
    {
        if (EndTransition)
        {
            EndBackdrop.CrossFadeAlpha(20f, 3f, true);
            EndTransitionTimer -= Time.deltaTime;
            if (EndTransitionTimer <= 0)
            {
                EndTransition = false;
            }
        }
        if (player.activeSelf == true)
        {
            dealthDelay -= 1 * Time.deltaTime;
            if (dealthDelay <= 0)
            {
                player.SetActive(false);
            }
        }

        
    }
    void InGame()
    {

        DamageDisplay.text = "$" + playerScript.m_damageCaused.ToString();
       
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SwitchState("Pause");
        }
      
    }
    void Pause()
    {
        AudioListener audio = MainCam.GetComponent<AudioListener>();
        AudioListener.pause = true;
        Time.timeScale = 0;
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            AudioListener.pause = false;
            SwitchState("InGame");
            Time.timeScale = 1;
        }

    }

    void Lose()
    {
        if (EndTransition)
        {
            LoseBackdrop.CrossFadeAlpha(20f, 3f, true);
            EndTransitionTimer -= Time.deltaTime;
            if (EndTransitionTimer <= 0)
            {
                EndTransition = false;
            }
        }
        if (player.activeSelf == true)
        {
            dealthDelay -= 1 * Time.deltaTime;
            if (dealthDelay <= 0)
            {
                player.SetActive(false);
            }
        }
    }
}
