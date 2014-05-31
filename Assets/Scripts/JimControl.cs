using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))] 
public class JimControl : MonoBehaviour
{
    Planet planet;
    SpriteRenderer childSprRenderer;
    Animator anim;
    public GameObject rsBullet;

    bool isGroggy = false;
    bool isMighty = false;
    bool isMoving = false;
    bool isDrain = false;

    int m_nState = 0;

    public float m_fMoveSpeed = 2.0f;
    public float m_fElecDrainAmount = 2.0f;
    public float m_fElectricity = 30.0f;
    public float m_fMaxEletricity = 40.0f;

    float _fTimer = 0;
    float _fFireTimer = 0;
    bool _facingRight = true;

    // Use this for initialization
    void Start()
    {
        m_nState = 0;
        anim = GetComponent<Animator>();
        planet = GameObject.Find("Planet").GetComponent<Planet>();
        childSprRenderer = gameObject.GetComponentInChildren<SpriteRenderer>();
        
        rsBullet = Resources.Load("2wayBullet") as GameObject;                
    }

    // Update is called once per frame
    void Update()
    {
        // Test
        if (Input.GetKeyDown(KeyCode.T)) { Damage(); Debug.Log("T"); }

        _fTimer += Time.deltaTime;

        // Movable state
        if (!isGroggy)
        {
            isMoving = Move();
            if (isMoving)
            {
                _fTimer = 0;
                isDrain = false;
                anim.SetBool("drain", false);
            }
            else
            { // when it doesn't move
                // This is Ready for Drain, not actual gettting electricity.
                if (_fTimer >= 1.0f )
                {
                    anim.SetBool("drain", true);        // start Drain motion, but it doesn't drain any things
                    isDrain = true;
                    _fTimer = 0;
                }
                if (isDrain == true)
                {
                    isDrain = Drain();
                    if( isDrain == false )
                        anim.SetBool("drain", false);
                }
            }
        }       
    }
    //------------------------------------------------
    // Movement
    //------------------------------------------------
    bool Move()
    {
        bool ret = false;
        float angle = 0.0f;
        anim.SetFloat("speed", 0.0f);

        // Turn off Fire Animation
        _fFireTimer += Time.deltaTime;
        if (_fFireTimer > 0.2)                                          // Need Change later
            anim.SetBool("fire", false);

        float inputLR = Input.GetAxis("Horizontal");
        bool inputFire = Input.GetKeyDown(KeyCode.Space);        
        if (inputLR != 0)
        {
            if ((_facingRight == true && inputLR < 0) || (_facingRight == false && inputLR > 0))
                Flip();
            _facingRight = (inputLR > 0) ? true : false;            
            anim.SetFloat("speed", 1.0f);
        }
        if (inputFire)
        {
            anim.SetBool("fire", true);
            Fire(childSprRenderer.transform.position);
            _fFireTimer = 0;
        }
        // it made movement
        if (inputLR != 0 || inputFire == true)
        {
            ret = true;
            angle = -inputLR * m_fMoveSpeed * 10;
            if (angle >= 360)       angle -= 360;
            else if (angle < 0)     angle += 360;
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
    bool Drain()
    {
        float amount = planet.GetElec(m_fElecDrainAmount);
        if (m_fElectricity >= m_fMaxEletricity || amount == 0)
        {
            return false;
        }
        m_fElectricity += amount;
        m_fElectricity = Mathf.Clamp(m_fElectricity, 0, m_fMaxEletricity);
        return true;
    }
    //------------------------------------------------
    // called by other objects
    //------------------------------------------------
    public void Damage()
    {
        if( isGroggy == true || isMighty == true )        
            return;

        anim.SetBool("damage", true);
        anim.SetBool("drain", false);
        isGroggy = true;
        isMighty = true;
        
        StartCoroutine(Twinkle());
    }
    //------------------------------------------------
    // AfterDamageProcess
    //------------------------------------------------
    IEnumerator Twinkle()
    {
        int i = 30;
        while (i > 0)
        {
            yield return new WaitForSeconds( (i > 15)? 0.15f : 0.07f);
            childSprRenderer.enabled = !childSprRenderer.enabled;
            if (i == 15)
            {
                isMighty = true;
                isGroggy = false;
                anim.SetBool("damage", false);
            }
            i--;
        }
        isMighty = false;
        childSprRenderer.enabled = true;
    }
    //------------------------------------------------
    // Fire : Set Load Bullet
    //------------------------------------------------
    void Fire(Vector2 p)
    {
        GameObject obj = ObjectPool.instance.GetGameObject(rsBullet, p*1.5f, this.transform.rotation);
    }

    //------------------------------------------------
    // 
    //------------------------------------------------
    void OnTriggerEnter2D(Collider2D colider)
    {
        if (colider.tag == "Enemy")
            Damage();               
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
