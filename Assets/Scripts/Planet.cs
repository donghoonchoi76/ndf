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
    public float fRotateSpeed = 40.0f;
    public float fRotateAccel = 20.0f;

    protected bool bRecoveryMode = false; // in this time, Jim cannot charge elec from planet.
    protected float fCurrRotateSpeed = 100.0f;
    protected bool bAlive = true;


	// Use this for initialization
	void Start () 
    {
        fCurrRotateSpeed = fRotateSpeed;
	}
	
	// Update is called once per frame
	void Update () 
    {
        if(bAlive) UpdateElec();
        UpdateRotation();
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

    void UpdateRotation()
    {
        float fTargetSpeed = fRotateSpeed;
        if (fElec >= 450.0f) fTargetSpeed *= 1.2f;
        else if (fElec >= 300.0f) fTargetSpeed *= 1.0f;
        else if (fElec >= 150.0f) fTargetSpeed *= 0.75f;
        else fTargetSpeed *= 0.5f;

        if (!bAlive) fTargetSpeed = 0.0f;

        if(fTargetSpeed - fCurrRotateSpeed > 0.0f)
        {
            fCurrRotateSpeed += (fRotateAccel * Time.deltaTime);
            if(fCurrRotateSpeed > fTargetSpeed) fCurrRotateSpeed = fTargetSpeed;
        }
        else if(fTargetSpeed - fCurrRotateSpeed < 0.0f)
        {
            fCurrRotateSpeed -= (fRotateAccel * Time.deltaTime);
            if(fCurrRotateSpeed < fTargetSpeed) fCurrRotateSpeed = fTargetSpeed;
        }

        transform.Rotate(0.0f, 0.0f, (fCurrRotateSpeed * Time.deltaTime));
    }

    // 
    //  float GetElect()...
    //  requireAmount : how much requested electricity to the planet..
    //  return value : how much actually planet can gives.
    //  This method reduces electricity of planet automatically by return value.
    //
    public float GetElec(float requiredAmount)
    {
        if (bRecoveryMode) return 0.0f;

        if (fElec >= requiredAmount) fElec -= requiredAmount;
        else
        {
            requiredAmount = fElec;
            fElec = 0.0f;
        }

        return requiredAmount;
    }

    void Damage(float damage)
    {
        fHP -= damage;
        if (fHP <= 0.0f)
        {
            fHP = 0.0f;
            bAlive = false;
        }
    }
}
