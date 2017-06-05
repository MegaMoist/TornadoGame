using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerScript : MonoBehaviour
{
    [Tooltip("Tornado Particle Systems")]
    public ParticleSystem[] m_tornadoParticles;

    //[Tooltip("Rain Prefab")]
    //public GameObject RainPrefab;

    [Tooltip("Debug player movement vector (WASD)")]
    public Vector3 movement;
    [Tooltip("Debug player velocity vector")]
    public Vector3 velocity;
    [Tooltip("Number of objects currently in vortex")]
    public int objCount;

    [Tooltip("Number of objects allowed in vortex")]
    public int objLimit;

    [Tooltip("Current Tornado Size")]
    public float m_currentSize = 0;

    [Tooltip("The player collider")]
    public CapsuleCollider thisCollider;




    [Tooltip("Current Tornado Tier")]
    public int m_Current_Tier = 0;//current tier


    [Header("\n")]
    [Header("Current Damage Caused")]
    public float m_damageCaused;

    [Header("\n")]
    [Tooltip("Minimum tornado size for fail state")]
    public float m_playerMinSize = 0;

    [Header("\n")]
    [Tooltip("Maximum tornado size for fail state")]
    public float m_playerMaxSize = 0;
    public bool m_maxSizeAchieved = false;

    [Header("Minimum sizes for each tier")]
    public float m_tier_1_min_Size;
    public float m_tier_2_min_Size;
    public float m_tier_3_min_Size;



    //public float CurrentSpeed;
    private float Acceleration_Factor;

    [Header("Acceleration Factors (Decimal Number from 0 to 1)")]
    public float Tier1_Acceleration_Factor;
    public float Tier2_Acceleration_Factor;
    public float Tier3_Acceleration_Factor;

    [Header("\n")]
    [Header("Maximum Speeds for separate tiers")]
    public float Tier1_Max_Speed;
    public float Tier2_Max_Speed;
    public float Tier3_Max_Speed;

    [Header("\n")]
    [Header("The player boost speeds(multiplies current Acceleration Factor)")]
    public float m_Tier1_Boost_Acceleration;
    public float m_Tier2_Boost_Acceleration;
    public float m_Tier3_Boost_Acceleration;
    public float m_Tier1_Boost_Max_Speed;
    public float m_Tier2_Boost_Max_Speed;
    public float m_Tier3_Boost_Max_Speed;
    private float m_CurrentBoostSpeed;
    private float m_CurrentMaxBoostSpeed;

    [Header("\n")]


    [Header("The player forced decays\n")]
    private Vector3 m_CurrentForcedDecay;
    public Vector3 m_Tier1_ForcedDecay;
    public Vector3 m_Tier2_ForcedDecay;
    public Vector3 m_Tier3_ForcedDecay;

    [Header("\n")]
    [Header("Deceleration Factors(0.0 to 0.99999)")]
    private float Deceleration_Factor;
    private float BoostReturn_Factor;
    public float m_Tier1_Deceleration_Factor;
    public float m_Tier2_Deceleration_Factor;
    public float m_Tier3_Deceleration_Factor;

    [Header("\n")]
    [Header("Boost Return Factor(0.0 to 0.99999) The smaller the faster it decelerates")]
    public float m_Tier1_Boost_Return_Factor;
    public float m_Tier2_Boost_Return_Factor;
    public float m_Tier3_Boost_Return_Factor;
    [Header("\n")]


    [Header("1 in m_randomness chance to change deviation")]

    public int m_Tier1_Rand;
    public int m_Tier2_Rand;
    public int m_Tier3_Rand;

    private int m_randomness;
    private float m_WindStrength;


    [Header("Float from 0.0f to 0.2f")]
    public float m_Tier1_WindStrength;
    public float m_Tier2_WindStrength;
    public float m_Tier3_WindStrength;
    [Header("\n")]


    [Tooltip("Decay Factor for Tier 1")]
    public Vector3 Tier1_Decay_Factor;
    [Tooltip("Decay Factor for Tier 2")]
    public Vector3 Tier2_Decay_Factor;
    [Tooltip("Decay Factor for Tier 3")]
    public Vector3 Tier3_Decay_Factor;

    [Tooltip("Minimum destroy height for objects")]
    public float m_destroyHeight = 10;

    [Header("\n")]
    public float m_pull;
    public float Tier1_Pull_Growth;
    public float Tier2_Pull_Growth;
    public float Tier3_Pull_Growth;
    public float Tier1_Pull_Decay;
    public float Tier2_Pull_Decay;
    public float Tier3_Pull_Decay;
    [Header("\n")]
    public float m_lift;
    public float Tier1_Lift_Growth;
    public float Tier2_Lift_Growth;
    public float Tier3_Lift_Growth;
    public float Tier1_Lift_Decay;
    public float Tier2_Lift_Decay;
    public float Tier3_Lift_Decay;
    [Header("\n")]
    public float m_fling;
    public float Tier1_Fling_Growth;
    public float Tier2_Fling_Growth;
    public float Tier3_Fling_Growth;
    public float Tier1_Fling_Decay;
    public float Tier2_Fling_Decay;
    public float Tier3_Fling_Decay;



    [Header("Minimum Force Factors ")]
    public float Min_Pull;
    public float Min_Lift;
    public float Min_Fling;


    private float m_maxSpeed;



    private Vector3 m_currSpeed;
    [Header("\n")]
    [Header("Current Force Growth Factors")]
    public float m_pullGrowth;
    public float m_liftGrowth;
    public float m_flingGrowth;
    public float m_pullDecay;
    public float m_liftDecay;
    public float m_flingDecay;


    private CharacterController m_charController;
    private Vector3 m_decayFactor;


    private Text m_tornadoSize;

    private ParticleSystem m_currParticleSystem;
    private float[] m_tornadoParticleOriginalSizes;

    private Rigidbody m_playerRigidBody;


    public Vector3 randWindDirection;

    private bool inputDetected = false;

    //bens edits for sound
    bool tier1 = false;
    bool tier2 = false;
    bool tier3 = false;
    public AudioSource tierTransitionSound;
    public float soundCooldown;
    public float soundTimer;



    public float playerRadius;
    void Start()//initialize
    {
        m_currentSize = this.transform.localScale.sqrMagnitude;//m_current size
        m_playerRigidBody = GetComponent<Rigidbody>();
        thisCollider = GetComponent<CapsuleCollider>();
        m_charController = GetComponent<CharacterController>();


        if (m_tornadoParticles != null)
        {
            m_tornadoParticleOriginalSizes = new float[m_tornadoParticles.Length];

            for (int i = 0; i < m_tornadoParticles.Length; i++)
            {

                m_tornadoParticleOriginalSizes[i] = m_tornadoParticles[i].startSize;
                m_tornadoParticles[i].Stop();
            }
        }

    }

    void Update()//called once per frame
    {

        playerRadius = thisCollider.radius * this.transform.localScale.x;
        //if (m_Current_Tier > 1 && RainPrefab != null)
        //{
        //    RainPrefab.SetActive(true);
        //}
        //else
        //{
        //    RainPrefab.SetActive(false);
        //}

        m_currentSize = this.transform.localScale.sqrMagnitude;

        if (m_currentSize >= m_tier_1_min_Size && m_currentSize < m_tier_2_min_Size)
        {
            if(!tier1)
            {
                //dosomet
                //tierTransitionSound.Play();
                tier1 = true;
                tier2 = false;
                tier3 = false;
            }
            m_Current_Tier = 1;
            m_tornadoParticles[1].Stop();
            m_tornadoParticles[2].Stop();
            m_tornadoParticles[0].Play();
            Acceleration_Factor = Tier1_Acceleration_Factor;
            m_maxSpeed = Tier1_Max_Speed;
            m_decayFactor = Tier1_Decay_Factor;
            m_pullGrowth = Tier1_Pull_Growth;
            m_flingGrowth = Tier1_Fling_Growth;
            m_liftGrowth = Tier1_Lift_Growth;
            m_pullDecay = Tier1_Pull_Decay;
            m_flingDecay = Tier1_Fling_Decay;
            m_liftDecay = Tier1_Lift_Decay;
            m_CurrentForcedDecay = m_Tier1_ForcedDecay;
            m_CurrentBoostSpeed = m_Tier1_Boost_Acceleration;
            m_CurrentMaxBoostSpeed = m_Tier1_Boost_Max_Speed;
            m_WindStrength = m_Tier1_WindStrength;
            m_randomness = m_Tier1_Rand;
            Deceleration_Factor = m_Tier1_Deceleration_Factor;
            BoostReturn_Factor = m_Tier1_Boost_Return_Factor;
            //Debug.Log("Tier 1");
        }
        else if (m_currentSize >= m_tier_2_min_Size && m_currentSize < m_tier_3_min_Size)
        {
            if (!tier2)
            {
                //dosomet
                if(soundTimer <= 0)
                {
                    tierTransitionSound.Play();
                }
                tier1 = false;
                tier2 = true;
                tier3 = false;
                soundTimer = soundCooldown;
            }
            soundTimer -= 1 * Time.deltaTime;
            
            m_Current_Tier = 2;
            m_tornadoParticles[0].Stop();
            m_tornadoParticles[2].Stop();
            m_tornadoParticles[1].Play();
            Acceleration_Factor = Tier2_Acceleration_Factor;
            m_maxSpeed = Tier2_Max_Speed;
            m_decayFactor = Tier2_Decay_Factor;
            m_pullGrowth = Tier2_Pull_Growth;
            m_flingGrowth = Tier2_Fling_Growth;
            m_liftGrowth = Tier2_Lift_Growth;
            m_pullDecay = Tier2_Pull_Decay;
            m_flingDecay = Tier2_Fling_Decay;
            m_liftDecay = Tier2_Lift_Decay;
            m_CurrentForcedDecay = m_Tier2_ForcedDecay;
            m_CurrentBoostSpeed = m_Tier2_Boost_Acceleration;
            m_CurrentMaxBoostSpeed = m_Tier2_Boost_Max_Speed;
            m_WindStrength = m_Tier2_WindStrength;
            m_randomness = m_Tier2_Rand;
            Deceleration_Factor = m_Tier2_Deceleration_Factor;
            BoostReturn_Factor = m_Tier2_Boost_Return_Factor;
            //Debug.Log("Tier 2");
        }
        else if (m_currentSize > m_tier_2_min_Size || m_currentSize >= m_tier_3_min_Size)
        {
            if (!tier3)
            {
                if (soundTimer <= 0)
                {
                    tierTransitionSound.Play();
                }
                tier1 = false;
                tier2 = false;
                tier3 = true;
                soundTimer = soundCooldown;
            }
            soundTimer -= 1 * Time.deltaTime;
            m_Current_Tier = 3;
            m_tornadoParticles[0].Stop();
            m_tornadoParticles[1].Stop();

            m_tornadoParticles[2].Play();
            Acceleration_Factor = Tier3_Acceleration_Factor;
            m_maxSpeed = Tier3_Max_Speed;
            m_decayFactor = Tier3_Decay_Factor;
            m_pullGrowth = Tier3_Pull_Growth;
            m_flingGrowth = Tier3_Fling_Growth;
            m_liftGrowth = Tier3_Lift_Growth;
            m_pullDecay = Tier3_Pull_Decay;
            m_flingDecay = Tier3_Fling_Decay;
            m_liftDecay = Tier3_Lift_Decay;
            m_CurrentForcedDecay = m_Tier3_ForcedDecay;
            m_CurrentBoostSpeed = m_Tier3_Boost_Acceleration;
            m_CurrentMaxBoostSpeed = m_Tier3_Boost_Max_Speed;
            m_WindStrength = m_Tier3_WindStrength;
            m_randomness = m_Tier3_Rand;
            Deceleration_Factor = m_Tier3_Deceleration_Factor;
            BoostReturn_Factor = m_Tier3_Boost_Return_Factor;
            //Debug.Log("Tier 3");
        }

        if(soundTimer < 0)
        {
            soundTimer = 0;
        }

        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        for (int i = 0; i < m_tornadoParticles.Length; i++)
        {
            if (m_tornadoParticles[i] != null)
            {

                m_tornadoParticles[i].startSize = m_tornadoParticleOriginalSizes[i] * (1 + this.transform.localScale.sqrMagnitude * 10);
                m_tornadoParticles[i].startLifetime = this.transform.localScale.x + 1.5f;
            }

        }

        movement = transform.right * moveHorizontal + transform.forward * moveVertical;


        //Forced Decay
        if (Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.LeftShift))
        {

            if(m_Current_Tier != 3)
            {
                this.transform.localScale -= m_CurrentForcedDecay;
                m_maxSpeed = m_CurrentMaxBoostSpeed;
            }



        }
        else
        {
            m_CurrentBoostSpeed = 1.0f;
        }



        if (Input.GetKeyUp(KeyCode.Space) && Input.GetKey(KeyCode.LeftShift))
        {
            Deceleration_Factor = BoostReturn_Factor;
        }


        //Movement
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.RightArrow))//WASD
        {

            inputDetected = true;



        }
        else
        {

            inputDetected = false;

        }



        m_currSpeed = m_charController.velocity;

        m_currSpeed.y = 0;
        velocity = m_currSpeed;

        Vector3 lookDir = this.transform.position - Camera.main.transform.position;//camera lookDIr

        lookDir.y = 0;

        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(lookDir), 1);




        if (Random.Range(1, m_randomness) == 1)//chance
        {

            Vector2 randir = Random.insideUnitCircle;
            randWindDirection = new Vector3(randir.x, 0, randir.y);
            randWindDirection.y = 0;



        }









    }

    void FixedUpdate()
    {



        if(inputDetected)
        {
            if (m_currSpeed.sqrMagnitude <= m_maxSpeed)
            {
                m_charController.Move((m_currSpeed + (randWindDirection * m_WindStrength) + (movement * (Acceleration_Factor * m_CurrentBoostSpeed))) * Time.deltaTime);

            }
            else
            {
                m_charController.Move((m_currSpeed) * Deceleration_Factor * Time.deltaTime);
            }
        }
        else
        {
            m_charController.Move((m_currSpeed) * Deceleration_Factor * Time.deltaTime);
        }

        if (this.transform.localScale.sqrMagnitude <= m_playerMinSize)
        {
            //game over   
            // Debug.Log("Game Over");
        }
        else
        {
            this.transform.localScale -= m_decayFactor;//normal over-time decay factor
        }


        if (this.transform.localScale.sqrMagnitude >= m_playerMaxSize)
        {
            m_maxSizeAchieved = true;
        }
        else { m_maxSizeAchieved = false; }

    }



    Vector3 RotatePointAroundPivot(Vector3 point, Vector3 pivot, Vector3 angles)
    {
        Vector3 dir = point - pivot; // get point direction relative to pivot
        dir = Quaternion.Euler(angles) * dir; // rotate it
        point = dir + pivot; // calculate rotated point
        return point; // return it
    }
}
