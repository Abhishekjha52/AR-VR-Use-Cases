using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;

public class vbscript : MonoBehaviour
{
	public GameObject robot;
	public GameObject vbtn;
	public float speed=20.0f;
    // Start is called before the first frame update
    void Start()
    {
		vbtn=GameObject.Find("VirtualButton");
		vbtn.GetComponent<VirtualButtonBehaviour>().RegisterOnButtonPressed(OnButtonPressed);
		vbtn.GetComponent<VirtualButtonBehaviour>().RegisterOnButtonReleased(OnButtonReleased);
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
	
	public void OnButtonPressed(VirtualButtonBehaviour vb)
	{
		Debug.Log("Virtual Button Pressed");
		rotateRobot();
	}
	
	public void OnButtonReleased(VirtualButtonBehaviour vb)
	{
		Debug.Log("Virtual Button Released");
		rotateRobot();
	}
	
	void rotateRobot()
	{
		robot.transform.Rotate(speed*Time.deltaTime*Vector3.up);
	}
	
	
		
}
