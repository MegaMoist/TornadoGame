using UnityEngine;
using System.Collections;

public class ModelToggle : MonoBehaviour
{
    //private MeshCollider mc;
    public GameObject PhysModel;
    public GameObject FaceModel;
    private GameObject player;
    private PlayerScript playerScript;
    private bool toggle = false;

    public bool m_destroyed;
    public bool nature;
    // Use this for initialization
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        if (PhysModel == null)
        {
            return;
        }
       for (int i = 0; i < PhysModel.transform.childCount; i++)
       {
           PhysModel.transform.GetChild(i).gameObject.SetActive(false);
       }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        
        if (player != null && !toggle)
        {
            playerScript = player.GetComponent<PlayerScript>();
        }
        else
        {
            return;
        }


        Vector3 distance = this.transform.position - player.transform.position;

        if (distance.sqrMagnitude < 1.01f*player.GetComponent<CapsuleCollider>().radius)
        {

            toggle = true;
 
        }
    }



    void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("Player"))
        {
            if(PhysModel != null)
            {
                PhysModel.SetActive(true);
            }
            else
            {
                return;
            }
            for (int i = 0; i < PhysModel.transform.childCount; i++)
            {
                PhysModel.transform.GetChild(i).gameObject.SetActive(true);

                
            }
            //Destroy(FaceModel.gameObject);

            FaceModel.gameObject.SetActive(false);//disable face model

        }
    }


}
