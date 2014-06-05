using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))] 
public class DroneControl : MonoBehaviour
{
    enum eDroneState { followJim };

    public float fDelayToMove = 0.5f;
    public float fMoveSpeed = 80.0f;
    public float fMoveAccel = 20.0f;

    JimControl jim;
    eDroneState eState = eDroneState.followJim;
    
    bool _facingRight = true;
    float fMoveStartTime = 0.0f;
    float fCurrSpeed = 0.0f;

    // Use this for initialization
    void Start()
    {
        jim = GameObject.Find("Jim").GetComponent<JimControl>();
        fMoveStartTime = Time.time + fDelayToMove;
        eState = eDroneState.followJim;
    }

    // Update is called once per frame
    void Update()
    {  
        switch (eState)
        {
            case eDroneState.followJim:
                UpdateFollowJim();
                break;
        }
    }

    void UpdateFollowJim()
    {
        float diff = jim.GetAngle() + 360.0f - transform.rotation.eulerAngles.z;
        if (diff > 360.0f) diff -= 360.0f;

        if (diff != 0.0f && Time.time > fMoveStartTime)
        {
            fCurrSpeed += (fMoveAccel * Time.deltaTime);
            if(fCurrSpeed > fMoveSpeed) fCurrSpeed = fMoveSpeed;

            float fMoveDelta = fCurrSpeed * Time.deltaTime;
            if (diff > 180.0f)
            {
                if (fMoveDelta > (360.0f - diff))
                {
                    fCurrSpeed = 0.0f;
                    fMoveStartTime = Time.time + fDelayToMove;
                    fMoveDelta = 360.0f - diff;
                }
                fMoveDelta = -fMoveDelta;
            }
            else if (diff < fMoveDelta)
            {
                fCurrSpeed = 0.0f;
                fMoveStartTime = Time.time + fDelayToMove;
                fMoveDelta = diff;
            }
            
            transform.rotation = Quaternion.AngleAxis(transform.rotation.eulerAngles.z + fMoveDelta, new Vector3(0,0,1));
        }
    }
  
    //------------------------------------------------
    // Flip  Animation
    //------------------------------------------------
    void Flip()
    {
        _facingRight = !_facingRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }
    
}
