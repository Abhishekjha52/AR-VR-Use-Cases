using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class cardScript : MonoBehaviour
{
	public Button fButton,insButton;
	
	string fb_profile="https://www.facebook.com/Abhishek jha";
	string ins_profile="https://www.instagram.com/abhishek_jha_052";
    // Start is called before the first frame update
    void Start()
    {
		Button fbtn=fButton.GetComponent<Button>();
		fbtn.onClick.AddListener(fButtonCall);
		
		Button insbtn=insButton.GetComponent<Button>();
		insbtn.onClick.AddListener(insButtonCall);
		
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
	
	public void fButtonCall()
	{
		Application.OpenURL(fb_profile);
	}
	
	public void insButtonCall()
	{
		Application.OpenURL(ins_profile);
	}
}
