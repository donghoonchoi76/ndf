﻿using UnityEngine;
using System.Collections;

public class Enemy0 : EnemyBase {

    void OnEnable()
    {
        float speed = 1.0f;
        rigidbody2D.velocity = transform.up * speed;
    }
    public override void ChangeSpeed(float spd)
    {
        rigidbody2D.velocity = transform.up * spd;
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Bullet")
        {
            float damage = col.GetComponent<Bullet>().m_fPower;
            ObjectPool.instance.ReleaseGameObject(col.gameObject);
            //Destroy(col.gameObject);
            HP -= damage;
            if (HP < 0)
                ObjectPool.instance.ReleaseGameObject(this.gameObject);
        }
        if (col.tag == "Player")
        {

        }
        if (col.tag == "Planet")
        {
            col.SendMessage("Damage", Power, SendMessageOptions.DontRequireReceiver);
            ObjectPool.instance.ReleaseGameObject(this.gameObject);
        }
    }
}
