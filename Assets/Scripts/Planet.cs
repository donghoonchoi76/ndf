using UnityEngine;
using System.Collections;

public class Planet : MonoBehaviour {
    public const int MAX_SLOT = 20;
    public float fHP = 100.0f;
    public float fElec = 300.0f;
    public float fSpeed = 20.0f;

	// Use this for initialization
	void Start () {

        iTween.RotateBy(gameObject, iTween.Hash("z", 90, "speed", fSpeed, "loopType", "loop", "easeType", "linear"));
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
