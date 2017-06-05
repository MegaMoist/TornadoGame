using UnityEngine;
using System.Collections;

public class Lightning : MonoBehaviour
{
    private  float m_interval;
    private float m_fadeTime;
    private Vector3[] m_path;

    private Light m_light;
    private LineRenderer m_bolt;
    private Vector3 m_randPos;
    private PlayerScript m_playerScript;
    // Use this for initialization
    void Start()
    {
        m_playerScript = GetComponent<PlayerScript>();
        m_interval = 5;
        m_fadeTime = 0.6f;
        m_randPos = new Vector3();
        m_path = new Vector3[50];
        m_bolt = GetComponent<LineRenderer>();
        m_light = GetComponentInChildren<Light>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(m_bolt == null || m_light == null || m_playerScript.m_Current_Tier == 1)//only activate lightning after tier 1
        {
            return;
        }
        m_interval -= Time.deltaTime;
        if (m_interval < 0)
        {
            m_bolt.enabled = true;
            m_path[0] = this.transform.position;
            m_path[0].y = 50;
            m_randPos = m_path[0];
            for (int i = 1; i < m_path.Length; i++)
            {
                m_randPos.x += Random.Range(-2.5f, 2.5f);
                m_randPos.z += Random.Range(-2.5f, 2.5f);

                m_randPos.y -= 1f;

                m_path[i] = m_randPos;
            }

            m_bolt.SetPositions(m_path);
            m_light.transform.position = m_randPos;
            m_light.enabled = true;
            m_interval = 5;//reset
        }
        else
        {
            m_fadeTime -= Time.deltaTime;

            if (m_fadeTime < 0)
            {
                m_light.enabled = false;
                m_bolt.enabled = false;
                m_fadeTime = 0.6f;
            }

           
        }



       
    }
}
