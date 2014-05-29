using UnityEngine;
using System.Collections;

public class JimControl : MonoBehaviour {
    enum STATE
    {
        IDLE,
        WALK,
        SHOOT,
        WALK_SHOOT,
        RECHARGE,
        DAMAGE,
    };

    float m_fAngle = 0;
    float m_fMoveSpeed = 2.0f;
    
	// Use this for initialization
	void Start () {
        //Animator anim;
        //anim = GetComponent<Animator>();
        //anim.SetFloat("speed", Mathf.Abs(input));
	}
	
	// Update is called once per frame
	void Update () {
        Move();        
        
	}

    void Move()
    {
        int key = 0;

        if (Input.GetKey(KeyCode.LeftArrow))
            key = -1;
        if (Input.GetKey(KeyCode.RightArrow))
            key = 1;

        if (key != 0)
        {
            m_fAngle += -key * m_fMoveSpeed;
            if (m_fAngle >= 360)
                m_fAngle -= 360;
            if (m_fAngle < 0)
                m_fAngle += 360;
        }
        Quaternion qt = Quaternion.identity;
        qt.eulerAngles = new Vector3(0, 0, m_fAngle);
        transform.rotation = Quaternion.Slerp(transform.rotation, qt, 5.0f);
    }
}
