using UnityEngine;
using System.Collections;

public class Planet : MonoBehaviour 
{
    public const int MAX_SLOT = 20;

    public float fMaxElecRatioRecoveryMode = 0.2f;
    public float fHP = 100.0f;
    public float fMaxElec = 300.0f;
    public float fElec = 300.0f;
    public float fElecChargeSpeed = 5.0f; //per second
    public float fRotateSpeed = 20.0f;

    protected bool bRecoveryMode = false; // in this time, Jim cannot charge elec from planet.

	// Use this for initialization
	void Start () 
    {

        iTween.RotateBy(gameObject, iTween.Hash("z", 90, "speed", fRotateSpeed, "loopType", "loop", "easeType", "linear"));
	}
	
	// Update is called once per frame
	void Update () 
    {
        UpdateElec();
	}

    void UpdateElec()
    {
        if (fElec <= 0.0f)
        {
            bRecoveryMode = true;
        }
        else if (bRecoveryMode)
        {
            float fElecRatio = fElec / fMaxElec;
            if (fElecRatio >= fMaxElecRatioRecoveryMode) bRecoveryMode = false;
        }

        if (fElec < fMaxElec) fElec += (fElecChargeSpeed * Time.deltaTime);
        else fElec = fMaxElec;
    }

    public float GetElect()
    {
        if (bRecoveryMode) return 0.0f;
        return fElec;
    }
}
