using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallaxing : MonoBehaviour
{
    public Transform[] backgrounds; // Array of backgrounds and foregrounds for parallaxing
    private float[] parallaxing;    // Value of the camera's movement in moving the backgrounds by 
    public float parallaxingAmount = 1f; // Smoothness of the parallax, must be >0

    private Transform cam;
    private Vector3 previousCamPos;


    //Called before Start()
    void Awake()
    {
        cam = Camera.main.transform;
    }
    // Start is called before the first frame update
    void Start()
    {
        previousCamPos = cam.position;

        parallaxing = new float[backgrounds.Length];

        for (int i = 0; i < backgrounds.Length; i++)
        {
            parallaxing[i] = backgrounds[i].position.z * -1;

        }
    }

    // Update is called once per frame
    void Update()
    {
        // Each background
        for (int i = 0; i < backgrounds.Length; i++)
        {
            // Parallaxing in the opposite of the camera movement
            float parallax = (previousCamPos.x - cam.position.x) * parallaxing[i];

            // Target Position = Current Position + Parallax
            float backgroundTargetPosX = backgrounds[i].position.x + parallax;

            // Target Position = Background Position with Target's x Position
            Vector3 backgroundTargetPos = new Vector3(backgroundTargetPosX, backgrounds[i].position.y, backgrounds[i].position.z);

            // Fading between current and target position - lerp
            backgrounds[i].position = Vector3.Lerp(backgrounds[i].position, backgroundTargetPos, parallaxingAmount * Time.deltaTime);

        }
        // previousCamPos is set to the camera's position at the end of the frame
        previousCamPos = cam.position;
    }
}
