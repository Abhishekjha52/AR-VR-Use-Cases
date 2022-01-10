using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class sofaScript : MonoBehaviour
{
    public GameObject model;
    public Camera ar_camera;
    public ARRaycastManager raycastManager;

    public List<ARRaycastHit> hits = new List<ARRaycastHit>();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0)) {
            Ray ray=ar_camera.ScreenPointToRay(Input.mousePosition);
            if(raycastManager.Raycast(ray,hits)) {
                Pose pose=hits[0].pose;
                Instantiate(model,pose.position,pose.rotation);
            }
        }
    }
}

