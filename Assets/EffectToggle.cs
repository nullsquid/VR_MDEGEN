using UnityEngine;
using System.Collections;

public class EffectToggle : MonoBehaviour {
    public ParticleSystem clouds;
    public ParticleSystem distortion;
    public ParticleSystem glow_1;
    public ParticleSystem glow_2;
    bool distortionsOn = true;
	// Use this for initialization

	
	// Update is called once per frame
	void Update () {
        ToggleDistortion();
	}

    void ToggleDistortion()
    {
        if (Input.touchCount > 0)
        {
            if (distortionsOn == true)
            {
                clouds.GetComponent<Renderer>().enabled = false;
                //distortion.enableEmission = false;
                distortion.GetComponent<Renderer>().enabled = false;
                glow_1.GetComponent<Renderer>().enabled = false;
                glow_2.GetComponent<Renderer>().enabled = false;
                distortionsOn = false;
            }
            else if (distortionsOn == false)
            {
                clouds.GetComponent<Renderer>().enabled = true;
                //distortion.enableEmission = false;
                distortion.GetComponent<Renderer>().enabled = true;
                glow_1.GetComponent<Renderer>().enabled = true;
                glow_2.GetComponent<Renderer>().enabled = true;
                distortionsOn = true;
            }
        }
        else if (Input.GetMouseButtonDown(0))
        {
            if (distortionsOn == true)
            {
                clouds.GetComponent<Renderer>().enabled = false;
                //distortion.enableEmission = false;
                distortion.GetComponent<Renderer>().enabled = false;
                glow_1.GetComponent<Renderer>().enabled = false;
                glow_2.GetComponent<Renderer>().enabled = false;
                distortionsOn = false;
            }
            else if (distortionsOn == false)
            {
                clouds.GetComponent<Renderer>().enabled = true;
                //distortion.enableEmission = false;
                distortion.GetComponent<Renderer>().enabled = true;
                glow_1.GetComponent<Renderer>().enabled = true;
                glow_2.GetComponent<Renderer>().enabled = true;
                distortionsOn = true;
            }
        }
    }
}
