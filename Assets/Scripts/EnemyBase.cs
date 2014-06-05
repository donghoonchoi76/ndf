using UnityEngine;
using System.Collections;

public class EnemyBase : MonoBehaviour {
    public float HP = 100;
    public float Power = 100;

    public virtual void ChangeSpeed(float spd)
    {
    }
}
