using UnityEngine;
using System.Collections;


public class JimControl : MonoBehaviour
{
    Planet planet;

    const int GROGGY = 0x80;
    const int IDLE = 0;
    const int RUN = 1;
    const int DRAIN = 2;    

    int m_nState = 0;
    float m_fMoveSpeed = 2.0f;
    public float m_fElecDrainSpeed = 2.0f;
    public float m_fElectricity = 30.0f;
    public float m_fMaxEletricity = 40.0f;

    float _fTimer = 0;
    bool _facingRight = true;
    Animator anim;

    // Use this for initialization
    void Start()
    {
        m_nState = 0;        
        anim = GetComponent<Animator>();
        planet = GameObject.Find("Planet").GetComponent<Planet>();
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
                    anim.SetBool("drain", true);
                }
                if (m_nState == DRAIN)
                {
                    if (_fTimer >= 1.0f)
                    {                        
                        DrainProcess();
                        _fTimer = 0.0f;
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
        float angle = 0.0f;

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
            //m_fAngle += -key * m_fMoveSpeed;            
            angle = -key * m_fMoveSpeed * 10;

            if (angle >= 360)
                angle -= 360;
            if (angle < 0)
                angle += 360;
        }
        Quaternion qt = Quaternion.identity;
        qt.eulerAngles = new Vector3(0, 0, angle);
        qt.eulerAngles += transform.rotation.eulerAngles;
        transform.rotation = Quaternion.Slerp(transform.rotation, qt, 10.0f * Time.deltaTime);        
        return ret;
    }
    //------------------------------------------------
    // Drain Electricity
    //------------------------------------------------
    int DrainProcess()
    {
        if (planet.GetElect() > 0 && m_fElectricity <= m_fMaxEletricity)
        {
            planet.fElec -= m_fElecDrainSpeed;
            m_fElectricity += m_fElecDrainSpeed;
            m_fElectricity = Mathf.Clamp(m_fElectricity, 0, m_fMaxEletricity);
            Debug.Log(m_fElectricity);
        }
        
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
