using UnityEngine;
using System.Collections;

public class Enemy0 : MonoBehaviour {

    public float HP = 100;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Bullet")
        {
            float damage = col.GetComponent<Bullet>().m_fPower;
            Destroy(col.gameObject);
            HP -= damage;
            if (HP < 0)
                Destroy(this.gameObject);            
        }
    }
}
