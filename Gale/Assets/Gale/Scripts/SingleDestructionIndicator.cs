using UnityEngine;
using System.Collections;

public class SingleDestructionIndicator : MonoBehaviour
{
    //public GameManager m_manager;
    public Vacuum m_vacuumScript;
    public bool m_destroyed = false;
    // Use this for initialization

     void Awake()
    {
        //m_manager = GameObject.Find("GameManager").GetComponent<GameManager>();
        m_vacuumScript = this.GetComponent<Vacuum>();
    }
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        if(!m_destroyed && (m_vacuumScript.hasBeenPickedUp||m_vacuumScript.gameObject == null))
        {
           
            m_destroyed = true;//do only once
        }

    }

    void OnDestroy()
    {
        if(!m_destroyed)
        {
            
            m_destroyed = true;
        }
        
    }
}
