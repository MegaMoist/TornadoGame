using UnityEngine;
using System.Collections;

public class MultiDestructionIndicator : MonoBehaviour
{
    //public GameManager m_manager;
    public SoundManager sound;
    public int m_initialChildCount;
    public GameObject destroyedBuilding;
    public bool m_destroyed = false;
    public int m_childCount = 0;
    public int sizeCheck;
    public Vacuum[] m_children;
    // Use this for initialization

    
    void Start()
    {
        //if(destroyedBuilding !=null)
        //{
        //    destroyedBuilding.SetActive(false);

        //}

        sound = GameObject.Find("SoundManager").GetComponent<SoundManager>();
    }

    void OnEnable()
    {
        m_children = this.GetComponentsInChildren<Vacuum>();


        m_initialChildCount = this.transform.childCount;
        m_childCount = m_initialChildCount;


    }
    // Update is called once per frame
    void Update()
    {
        m_childCount = this.transform.childCount;

        if (!m_destroyed)
        {

            if (m_childCount <= m_initialChildCount * 0.8f)//if more than 50 percent is destroyed then trigger destruction
            {
                if (destroyedBuilding != null)
                {
                    destroyedBuilding.SetActive(true);
                    sound.PlaySound("BuildingDestroySound");
                }

                //m_manager = GameObject.Find("GameManager").GetComponent<GameManager>();
                // m_manager.m_manMadeObjects--;
                m_destroyed = true;//do only once
            }
        }
        
    }
}
