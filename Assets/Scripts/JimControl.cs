using UnityEngine;
using System.Collections;

public class JimControl : MonoBehaviour {

    int facing = 0;
    Animator anim;

	// Use this for initialization
	void Start () {
        anim = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
        float input = Input.GetAxis("Horizontal");

        facing = 0;
        if (input > 0)
            facing = 1;
        else if (input < 0)
            facing = -1;

        anim.SetFloat("speed", Mathf.Abs(input));
	}   
}
