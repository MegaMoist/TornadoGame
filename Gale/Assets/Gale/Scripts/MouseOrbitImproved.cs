using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[AddComponentMenu("Camera-Control/Mouse Orbit with zoom")]
public class MouseOrbitImproved : MonoBehaviour
{


    public Toggle FreeCamToggle;

    public Transform target;
    public float distance = 5.0f;
    public float xSpeed = 120.0f;
    public float ySpeed = 120.0f;

    public float camera_Angle;

    public float yMinLimit = -20f;
    public float yMaxLimit = 80f;

    public float distanceMin = .5f;
    public float distanceMax = 15f;

    public float zoom_factor;

    private new Rigidbody rigidbody;

    private Transform originalPos;

    Quaternion rotation;
    float x = 0.0f;
    float y = 0.0f;

    // Use this for initialization
    void Start()
    {
        Vector3 angles = transform.eulerAngles;
        x = angles.y;
        y = angles.x;

        rigidbody = GetComponent<Rigidbody>();

        // Make the rigid body not change rotation
        if (rigidbody != null)
        {
            rigidbody.freezeRotation = true;
        }
    }

    void Awake()
    {
        originalPos = this.transform;
    }

    void LateUpdate()
    {

        if (target)
        {
            
            if (FreeCamToggle.isOn)
            {

                
                if ( (Time.timeScale == 0 && Input.GetMouseButton(1)) || Time.timeScale == 1)
                {
                    //nothing
                   
                    x += Input.GetAxis("Mouse X") * xSpeed * distance * 0.02f;
                    y -= Input.GetAxis("Mouse Y") * ySpeed * 0.02f;
                    rotation = Quaternion.Euler(y, x, 0); distanceMin = Mathf.Lerp(distanceMin, (target.transform.localScale.x * zoom_factor * 0.7f), Time.deltaTime);

                }
                
                distance = distanceMin;
                distance = Mathf.Clamp(distance, distanceMin, distanceMax);
                //free rotation

            }
            else
            {
                rotation = Quaternion.Euler(camera_Angle, 180, 0);//revert back to original cam
                distanceMin = Mathf.Lerp(distanceMin, (target.transform.localScale.x * zoom_factor), Time.deltaTime);
                distance = distanceMin;
                distance = Mathf.Clamp(distance, distanceMin, distanceMax);
                
            }

            y = ClampAngle(y, yMinLimit, yMaxLimit);



            Vector3 negDistance = Vector3.back * distance;
            Vector3 position = rotation * negDistance + target.position;

            transform.rotation = rotation;
            transform.position = position;
        }


    }

    public void OnToggleOff()
    {

        rotation = Quaternion.Euler(camera_Angle, 180, 0);//revert back to original cam
    }

    public static float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360F)
            angle += 360F;
        if (angle > 360F)
            angle -= 360F;
        return Mathf.Clamp(angle, min, max);
    }
}