using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public int random;
    public int AmountToWin;
    public GameObject WindVane;
    public SoundManager sound;
    public Transform AnimHolder1;
    public Transform AnimHolder2;
    public Transform AnimHolder3;
    public int AnimHolderNumber;
    public GameStateManager state;
    public List<SingleDestructionIndicator> singleManMade;
    public List<MultiDestructionIndicator> multiManMade;
    public List<Vacuum> vacuumableObjects;

    Vector3 deviation;
    bool inAnimation = false;

    public float CurrentHighScore = 0;

    private PlayerScript m_playerScript;
    public int smallerObjCount = 0;
    public int m_manMadeObjects = 0; // number of man made objects to be destroyed

    AchievementManager achievements;
    public string LevelName;
    private static float m_damageCount;//count of damage
    private Text damageCost;//text display for damage cost

    private float UpdateTimer;
    private float UpdateCooldown;


    public float currTime = 0;
    private List<GameObject> itemsToDelete;
    // Use this for initialization

    void Start()
    {
        deviation = new Vector3();
        m_playerScript = state.playerScript;
        singleManMade = new List<SingleDestructionIndicator>();
        multiManMade = new List<MultiDestructionIndicator>();
        sound = GameObject.Find("SoundManager").GetComponent<SoundManager>();
        state = this.GetComponent<GameStateManager>();
        UpdateCooldown = 1;


        SingleDestructionIndicator[] sdis = GameObject.FindObjectsOfType<SingleDestructionIndicator>();
        MultiDestructionIndicator[] mdis = GameObject.FindObjectsOfType<MultiDestructionIndicator>();
        Vacuum[] vo = GameObject.FindObjectsOfType<Vacuum>();
        singleManMade.AddRange(sdis);
        multiManMade.AddRange(mdis);
        vacuumableObjects.AddRange(vo);

    }


    public void SetupGame()
    {

            m_manMadeObjects = singleManMade.Count + multiManMade.Count;

    }
    public void LoadLevel(string level)
    {
        switch (level)
        {
            case "Game":
               SceneManager.LoadScene(LevelName);
               break;
        }
    }

    public void Quit()
    {
        Application.Quit();
    }

    void FixedUpdate()
    {
       
    }
    // Update is called once per frame
    void Update()
    {
        if( state.gameState == GameStateManager.GameState.InGame)
        {
            currTime += Time.deltaTime; //Update Timer
        }


        smallerObjCount = 0;
        foreach(Vacuum VacuumScript in vacuumableObjects)
        {
            AnimHolderNumber = Random.Range(0, 3); /// make random number from 0 to 3
            if (VacuumScript.ID > VacuumScript.MinPickupChance && VacuumScript.ID < VacuumScript.MaxPickupChance && VacuumScript != null && VacuumScript.hasBeenPickedUp && VacuumScript.m_pickedUp == false && VacuumScript.GetComponent<BoxCollider>())
            {
                //VacuumScript.gameObject.AddComponent<Rotator>();
                //VacuumScript.GetComponent<Rotator>().x = Random.Range(0, 10);
               // VacuumScript.GetComponent<Rotator>().y = Random.Range(0, 10);
                //VacuumScript.GetComponent<Rotator>().z = Random.Range(0, 10);
               // VacuumScript.GetComponent<Rotator>().speed = Random.Range(10, 45);
                VacuumScript.GetComponent<BoxCollider>().enabled = false;
                VacuumScript.dontCountAsSmall = true;
                VacuumScript.GetComponent<Rigidbody>().useGravity = false;
                VacuumScript.stickyPiece = true;
                // VacuumScript.GetComponent<Rigidbody>().isKinematic = true;
                // VacuumScript.gameObject.transform.Rotate(Vector3.left, Time.deltaTime * 10);
                if (AnimHolderNumber == 0)
                {
                    // VacuumScript.gameObject.transform.position;

                    
                    VacuumScript.gameObject.transform.SetParent(AnimHolder1);


                }
                else if (AnimHolderNumber == 1)
                {
                    // VacuumScript.gameObject.transform.position = new Vector3(AnimHolder2.position.x + Random.Range(-0.1f, 0.1f), AnimHolder2.position.y + Random.Range(-0.1f, 0.1f), AnimHolder2.position.z + Random.Range(-0.5f, 0.5f));
                    VacuumScript.gameObject.transform.SetParent(AnimHolder2);
                   
                }
                else if (AnimHolderNumber == 2)
                {
                    //VacuumScript.gameObject.transform.position = new Vector3(AnimHolder3.position.x + Random.Range(-0.1f, 0.1f), AnimHolder3.position.y + Random.Range(-0.1f, 0.1f), AnimHolder3.position.z + Random.Range(-0.5f, 0.5f));
                    VacuumScript.gameObject.transform.SetParent(AnimHolder3);
              

                }
               
                VacuumScript.m_pickedUp = true;
                VacuumScript.enabled = false;
            }




            if (VacuumScript != null && m_playerScript != null && VacuumScript.m_SizeCheck < m_playerScript.m_currentSize && !VacuumScript.hasBeenPickedUp && VacuumScript.m_growthFactor.x >=0)//if object is smaller than player put
            {
                smallerObjCount++;
            }
        }

        //delete objects

        foreach( SingleDestructionIndicator sdi in singleManMade.ToArray())//single destroy
        {
            if (sdi.m_destroyed)//if marked for destroyed
            {
                singleManMade.Remove(sdi);
                //Destroy(sdi);
                //GameObject.Destroy(sdi.gameObject);
                --m_manMadeObjects;
            }
        }

        foreach (MultiDestructionIndicator mdi in multiManMade.ToArray())//multi destroy
        {

            if (mdi.m_destroyed)//if marked for destroyed
            {
                multiManMade.Remove(mdi);
                //Destroy(mdi);
                //GameObject.Destroy(mdi.gameObject);
                --m_manMadeObjects;
            }
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////
        foreach (Vacuum vacScript in vacuumableObjects.ToArray())//UPDATE vacuumableObjects list
        {
            if (vacScript.m_destroyed)//if marked for destroyed
            {
                vacuumableObjects.Remove(vacScript);
               // Destroy(vacScript.gameObject);
                Destroy(vacScript);

            }
        }
        //////////////////////////////////////////////////////////////////////////////////////////////////////

        if (m_manMadeObjects > 1 && smallerObjCount == 0 && state.gameState == GameStateManager.GameState.InGame &&  m_playerScript != null)
        {
            state.SwitchState("Lose");
            m_playerScript.enabled = false;
            
        }



        if(state.playerScript.randWindDirection != Vector3.zero)
        {
            WindVane.transform.rotation = Quaternion.Slerp(WindVane.transform.rotation, Quaternion.LookRotation(state.playerScript.randWindDirection), Time.deltaTime * 10.0f);
        }
        
        if (sound == null)
        {
            return;//dont do anything
        }

        UpdateTimer -= Time.deltaTime;
        if(UpdateTimer <= 0)
        {
            TierCheck();
            UpdateTimer = UpdateCooldown;
        }




        
    }




    public void TierCheck()
    {
        
        random = Random.Range(0, 7);

        if (state.playerScript.m_Current_Tier == 1 && state.gameState == GameStateManager.GameState.InGame)
        {

            sound.FadeOut("T3Wind", 0.3f);
            sound.FadeOut("T2Wind", 0.3f);
            sound.FadeIn("T1Wind", 0.3f);
           
        }
        if (state.playerScript.m_Current_Tier == 2)
        {

            sound.FadeOut("T1Wind", 0.3f);
            sound.FadeIn("T2Wind", 0.3f);
            sound.FadeOut("T3Wind", 0.3f);
          
        }
        if (state.playerScript.m_Current_Tier == 3)
        {

            sound.FadeOut("T1Wind", 0.3f);
            sound.FadeOut("T2Wind", 0.3f);
            sound.FadeIn("T3Wind", 0.3f);
            
        }
        if (state.playerScript.transform.localScale.sqrMagnitude <= state.playerScript.m_playerMinSize || state.playerScript.transform.localScale.x <= 0)
        {
            state.SwitchState("Lose");
        }
        if (state.gameState == GameStateManager.GameState.InGame && m_manMadeObjects <= AmountToWin)
        {
            state.SwitchState("EndGame");
        }
    }
}
