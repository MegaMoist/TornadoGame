using UnityEngine;
using System.Collections;

public class MeshCombiner : MonoBehaviour
{
    private MeshCollider mc;
    public GameObject FaceModel;

    // Use this for initialization

    void Start()
    {
        mc = FaceModel.GetComponent<MeshCollider>();
        CombineMeshes();
    }
    

    void Update()
    {


    }

    // Update is called once per frame
    public void CombineMeshes()//combines all meshes present
    {
        Quaternion oldRot = transform.rotation;
        Vector3 oldPos = transform.position;

        transform.rotation = Quaternion.identity;
        transform.position = Vector3.zero;

        MeshFilter[] filters = GetComponentsInChildren<MeshFilter>();

        Mesh finalMesh = new Mesh();

        CombineInstance[] combiners = new CombineInstance[filters.Length];

        for (int i = 0; i < filters.Length; i++)
        {
            combiners[i].subMeshIndex = 0;
            combiners[i].mesh = filters[i].sharedMesh;
            combiners[i].transform = filters[i].transform.localToWorldMatrix;
        }

        finalMesh.CombineMeshes(combiners);
        FaceModel.GetComponent<MeshFilter>().sharedMesh = finalMesh;
        mc.sharedMesh = finalMesh;
        transform.rotation = oldRot;
        transform.position = oldPos;
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }
    }
}
