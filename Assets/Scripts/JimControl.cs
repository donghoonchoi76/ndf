using UnityEngine;
using System.Collections;


public class JimControl : MonoBehaviour
{
    const int GROGGY = 0x80;
    const int IDLE = 0;
    const int RUN = 1;
    const int DRAIN = 2;    

    int m_nState = 0;

    float m_fAngle = 0;
    float m_fMoveSpeed = 2.0f;

    float m_fElectricity = 30.0f;
    float m_fMaxEletricity = 40.0f;

    float _fTimer = 0;
    bool _facingRight = true;
    Animator anim;

    // Use this for initialization
    void Start()
    {
        m_nState = 0;        
        anim = GetComponent<Animator>();        
    }

    // Update is called once per frame
    void Update()
    {
        _fTimer += Time.deltaTime;

        // Moveable State
        if ((m_nState & 0x80) != 0x80)
        {
            if (Move())
            {
                _fTimer = 0;
            }
            else
            {
                if (_fTimer >= 1.0f)
                {
                    m_nState = DRAIN;
                }
                if (m_nState == DRAIN)
                {
                    if (_fTimer >= 1.0f)
                    {
                        DrainProcess();
                    }                    
                }
            }                        
        }
       
        if (_fTimer > 1.0)
        {

        }
    }
    //------------------------------------------------
    // Movement
    //------------------------------------------------
    bool Move()
    {
        bool ret = false;
        int key = 0;
        anim.SetFloat("speed", 0.0f);
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            if (_facingRight == true)
                Flip();

            _facingRight = false;
            key = -1;
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            if (_facingRight == false)
                Flip();

            _facingRight = true;
            key = 1;
        }

        if (key != 0)
        {
            ret = true;
            anim.SetFloat("speed", 1.0f);

            m_fAngle += -key * m_fMoveSpeed;

            if (m_fAngle >= 360)
                m_fAngle -= 360;
            if (m_fAngle < 0)
                m_fAngle += 360;
        }
        Quaternion qt = Quaternion.identity;
        qt.eulerAngles = new Vector3(0, 0, m_fAngle);
        transform.rotation = Quaternion.Slerp(transform.rotation, qt, 10.0f * Time.deltaTime);
        return ret;
    }
    //------------------------------------------------
    // Drain Electricity
    //------------------------------------------------
    int DrainProcess()
    {
        Debug.Log("+1 energy");
        return 1;
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
