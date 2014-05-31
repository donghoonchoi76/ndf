﻿using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour {

    public float m_fPower = 10.0f;
    public Vector2 m_Velocity;
    public float speed;
	
    // when bullet is enable, the function is called
    void OnEnable()
    {
        speed = 4.0f;
        rigidbody2D.velocity = transform.position.normalized * speed;
    }
    void Update()
    {
        if (transform.position.magnitude > 10.0f)
            SelfRelease();
    }
    //
    void OnTriggerEner2D(Collider2D other)
    {
        // Call Bullet destroy Particle, with the same parent if it is need something
        Debug.Log("Called Bullet OnTriggerEnter2D");

        // Destroy
        Invoke("SelfRelease", 1);
    }
    // Self Destroy 
    private void SelfRelease()
    {
        ObjectPool.instance.ReleaseGameObject(gameObject);
    }
}