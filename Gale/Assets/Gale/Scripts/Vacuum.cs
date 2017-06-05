using UnityEngine;
using System.Collections;
using UnityEngine.UI;
[System.Serializable]
public class Vacuum : MonoBehaviour
{
    public int ID;
    public int maxID;
    public int MaxPickupChance;
    public int MinPickupChance;
    public int minID;
    private int m_objLimit;
    public float m_cost = 0;
    public int Tier;
    public bool m_pickedUp;
    public bool inTornado;
    public bool dontCountAsSmall;
    public Vector3 m_growthFactor;//individual growth factor
    public AudioSource PickupSound;
    public bool hasPickupSound;

    public bool m_destroyed = false;

    private Rigidbody m_rigidBody;//rigidbody
    public GameObject m_player;//player
    public GameManager m_gameManagerRef;


    public float m_pull;//pull factor
    public float m_lift;//lift factor
    public float m_fling;//fling factor

    public float m_SizeCheck;

    public bool hasBeenPickedUp = false;//check to see if object has passed through vortex

    private Collider m_thisCollider;
    public PlayerScript m_playerScript;

    private float m_randFling;

    private float m_objectHeight;


    private Vector3 Pull;
    private Vector3 Fling;
    private Vector3 Lift;
    private float distSqrd;
    public bool NegativeGrowthObject;


    Vector3 objPos;
    Vector3 playerPos;
    Vector3 distanceFromPlayer;

    /////////////////////////////////////////////////////////////////////////////////Vince
    public int m_suicideRandomRange = 0;//not used yet (not publicly available yet)
    public bool suicidePiece = false;
    /////////////////////////////////////////////////////////////////////////////////Vince


        /// <summary>
        /// Sticky pieces stay in the tornado
        /// </summary>
    public bool stickyPiece = false;

    // Use this for initialization
    void Start()
    {
        if (this.GetComponent<AudioSource>())
        {
            PickupSound = this.GetComponent<AudioSource>();
        }
        else if (!NegativeGrowthObject)
        {
            PickupSound = GameObject.FindGameObjectWithTag("PickupSound").GetComponent<AudioSource>();
        }
        else
        {
            PickupSound = GameObject.FindGameObjectWithTag("BadPickupSound").GetComponent<AudioSource>();
        }


        ID = Random.Range(minID, maxID);

        if(GetComponentInParent<MultiDestructionIndicator>() != null)///if part of multi destruct randomly select for destruction
        {
            if (Random.Range(1, 3) == 1) // (THINNING THE HERD) ---- 1 in 2 pieces are toggled to destroy immediately after growing (ADJUST RANGE OF RANDOMNESS TO WHATEVER)
            {
                suicidePiece = true;
            }
            else { suicidePiece = false; }
        }

        if (Random.Range(1, 10) == 1) // 1 in 30 chance to be a stickypiece
        {
            stickyPiece = true;
        }
        else
        {
            stickyPiece = false;
        }

        m_randFling = Random.Range(1.0f, 2.0f);
        m_thisCollider = GetComponent<Collider>();
        m_thisCollider.enabled = true;


        m_rigidBody = GetComponent<Rigidbody>();

        m_rigidBody.isKinematic = true;//unkinematic


    }

    void Awake()
    {
        if (this.GetComponent<AudioSource>())
        {
            PickupSound = this.GetComponent<AudioSource>();
        }
        else if(!NegativeGrowthObject)
        {
            PickupSound = GameObject.FindGameObjectWithTag("PickupSound").GetComponent<AudioSource>();
        }
        else
        {
            PickupSound = GameObject.FindGameObjectWithTag("BadPickupSound").GetComponent<AudioSource>();
        }
        m_gameManagerRef = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        m_player = GameObject.FindGameObjectWithTag("Player");
        if (m_player != null)
        {
            m_playerScript = m_player.GetComponent<PlayerScript>();
        }


        if (m_playerScript != null)
        {

            switch (Tier)
            {
                case 1:
                    if (m_SizeCheck < m_playerScript.m_tier_1_min_Size)
                    {
                        m_SizeCheck = m_playerScript.m_tier_1_min_Size;
                    }
                    break;

                case 2:
                    if (m_SizeCheck < m_playerScript.m_tier_2_min_Size)
                    {
                        m_SizeCheck = m_playerScript.m_tier_2_min_Size;
                    }

                    break;

                case 3:
                    if (m_SizeCheck < m_playerScript.m_tier_3_min_Size)
                    {
                        m_SizeCheck = m_playerScript.m_tier_3_min_Size;
                    }

                    break;
                default:
                    //Do Nothing 
                    break;
            }

        }

    }

    void OnEnable()
    {


    }

    bool NullCheck()
    {


        if (m_player == null || m_playerScript == null)
        {
            // Debug.Log("Player Not Found");
            return true;
        }
        else
        {
            return false;
        }
    }
    // Update is called once per frame
    void Update()
    {
        PlayerCollisionCheck();

        if (inTornado)
        {


            if (NullCheck())
            {
                return;
            }
            if (m_player.transform.localScale.sqrMagnitude >= m_SizeCheck)//current Check
            {
                if (!hasBeenPickedUp)//grow only once
                {
                    if (hasPickupSound)
                    {
                        PickupSound.Stop();
                    }
                    if (m_playerScript.m_maxSizeAchieved == false)//if not at max size
                    {
                        Grow();
                    }
                    if (m_gameManagerRef != null)
                    {
                        m_gameManagerRef.vacuumableObjects.Add(this);//Add self to vacuumableObjects in game manager
                    }
                    if(hasPickupSound)
                    {
                        PickupSound.Play();
                    }
                        
                    
                   
                    hasBeenPickedUp = true;
                        
                }

                if (m_playerScript.objCount > m_playerScript.objLimit)//if obj limit reach start culling objects
                {
                    m_destroyed = true;
                    GameObject.Destroy(this.gameObject);

                    return;
                }
                /////////////////////////////////////////////////////////////////////////////////Vince
                if (hasBeenPickedUp && suicidePiece == true)//if toggled for destruction (thinning the herd)
                {
                    m_destroyed = true;
                    GameObject.Destroy(this.gameObject);

                    return;
                }
                /////////////////////////////////////////////////////////////////////////////////Vince



                Vector3 temp = m_playerScript.transform.position;


                temp.y = this.transform.position.y;
                Pull = (temp - this.transform.position);


                Fling = Pull;
                Fling.x = Pull.z;//find normal vector
                Fling.z = -Pull.x;
                Fling.y = 0;
                Fling.Normalize();

                distSqrd = Pull.sqrMagnitude;



                //if distance is far pull strength is weaker

                //if distance is far fling is constant


                Pull.Normalize();
                Pull.y = 0;

                Lift = Vector3.up;

                if (distSqrd < 1.0f)
                {
                    distSqrd = 1.0f;
                }

                /////////////////////////////PHYSICS CHUNK START
                m_rigidBody.velocity = (4 * m_fling / (2 * Mathf.PI * distSqrd) * m_fling * m_randFling * (Fling + ((m_objectHeight / m_playerScript.m_destroyHeight) * Pull))) / (m_rigidBody.mass);//centripetal velocity with mass dampener
                                                          
                if( !stickyPiece)
                {
                    m_rigidBody.AddForce((Lift * m_lift) / distSqrd );
                }

               

                m_rigidBody.velocity = m_rigidBody.velocity * Time.deltaTime;
                m_rigidBody.AddRelativeTorque((Vector3.left + Vector3.forward) * m_fling * 10 * m_randFling);


                /////////////////////////////PHYSICS CHUNK END
            }

        }
        if (NullCheck())
        {
            return;
        }
        m_pull = m_playerScript.m_pull;
        m_lift = m_playerScript.m_lift;
        m_fling = m_playerScript.m_fling;

        m_objLimit = m_playerScript.objLimit;
        m_objectHeight = this.transform.position.y;

        if ((transform.position.y > m_playerScript.m_destroyHeight || transform.position.y < -1)) //destroy once pass min destroy height
        {
            m_destroyed = true;
            GameObject.Destroy(this.gameObject);

            return;
        }

        if (!inTornado && hasBeenPickedUp && m_rigidBody.velocity.sqrMagnitude < 1.0f ) // not in tornado AND not moving AND is close to ground then kinematic (A BETTER WAY TO SOLVE THIS IS TO GET A GROUND PLANE COLLIDER AND CHECK IF COLLIDING WITH GROUND IN ONCOLLIDE ENTER. THIS WILL SAVE ALOT OF PHYSICS CALCULATION PER CYCLE)
        {

            m_destroyed = true;
            GameObject.Destroy(this.gameObject);
            return;
        }

    }





    void OnDestroy()//on destroy decrement objCOunt in playerScript
    {
        if(inTornado)
        {
            m_playerScript.objCount--;
        }
        
    }


   



    void Grow()
    {
        
        m_playerScript.m_damageCaused += m_cost;//update damage cost by addind this items cost to the playerscript



        if(m_playerScript.m_Current_Tier == 3 && this.Tier == 1)//if two levels below
        {
            //dont grow
        }
        else
        {


            m_player.transform.localScale += m_growthFactor;//increase the transform


            //Scale tornado power according to scale
            if (m_growthFactor.x < 0 && m_growthFactor.y < 0 && m_growthFactor.z < 0)
            {//decay
                m_playerScript.m_pull -= m_playerScript.m_pullDecay * m_player.transform.localScale.x;
                m_playerScript.m_lift -= m_playerScript.m_liftDecay * m_player.transform.localScale.x;
                m_playerScript.m_fling -= m_playerScript.m_flingDecay * m_player.transform.localScale.x;
                if (m_pull <= 0)
                {
                    m_playerScript.m_pull = m_playerScript.Min_Pull;
                }
                if (m_lift <= 0)
                {
                    m_playerScript.m_lift = m_playerScript.Min_Lift;
                }
                if (m_fling <= 0)
                {
                    m_playerScript.m_fling = m_playerScript.Min_Fling;
                }

            }
            else
            {//grow
                m_playerScript.m_pull += m_playerScript.m_pullGrowth * m_player.transform.localScale.x;
                m_playerScript.m_lift += m_playerScript.m_liftGrowth * m_player.transform.localScale.x;
                m_playerScript.m_fling += m_playerScript.m_flingGrowth * m_player.transform.localScale.x;
            }

        }



      
    }

     void PlayerCollisionCheck()
    {

        if(m_playerScript.thisCollider == null)
        {
            return;
        }
         objPos = this.transform.position;
         playerPos = m_player.transform.position;

        objPos.y = 0;
        playerPos.y = 0;


        distanceFromPlayer = objPos - playerPos;

        float distanceSqrd = distanceFromPlayer.sqrMagnitude;

        float playerRadius = m_playerScript.playerRadius;


        if (!inTornado && distanceSqrd <= playerRadius * playerRadius) //if within player radius
        {
            if (m_player.transform.localScale.sqrMagnitude >= m_SizeCheck && this.gameObject != null && !inTornado)
            {
                ID = Random.Range(minID, maxID);
                m_rigidBody.isKinematic = false;
                m_rigidBody.detectCollisions = false;
                ++m_playerScript.objCount;
                inTornado = true;

            }

        }
        else
        if (inTornado && distanceSqrd >= playerRadius * playerRadius)
        {

            if (m_player.transform.localScale.sqrMagnitude >= m_SizeCheck && this.gameObject != null && inTornado)
            {
                m_rigidBody.isKinematic = false;
                m_rigidBody.detectCollisions = true;

                --m_playerScript.objCount;
                inTornado = false;

            }

        }

        
    }

}
