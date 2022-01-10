using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cubeScript : MonoBehaviour
{
	public GameObject cube;
	public float speed=20.0f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
		cube.transform.Rotate(speed*Time.deltaTime*Vector3.up);
        
    }
}
