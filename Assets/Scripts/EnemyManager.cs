using UnityEngine;
using System.Collections;

public class EnemyManager : MonoBehaviour {
    public GameObject[] enemies = new GameObject[5];
    float m_fTimer;
    int m_nPhase;

	// Use this for initialization
	void Start () {
        m_nPhase = 0;

        StartCoroutine(RegularGenerator());
	}
	
	// Update is called once per frame
	void Update () {
        m_fTimer += Time.deltaTime;      

	}

    IEnumerator RegularGenerator()
    {
        int i = 0;
        do {
            i++;
            yield return new WaitForSeconds(4);
            for(int j=0; j<i; j++)
                GenerateEnemy(enemies[0]);
        } while (i>0);
    }


    void GenerateEnemy(GameObject obj)
    {
        int i = 0;
        for (i = 0; i < enemies.Length; i++)
        {
            if (obj == enemies[i])
                break;
        }
        if (i < enemies.Length)
        {
            float dist = Random.Range(9.0f, 14.0f);
            int angle = Random.Range(0, 91);
            Vector2 p = new Vector2(Mathf.Cos(angle) * dist,Mathf.Sin(angle) * dist);
            p.x = (Random.Range(0,2) == 0) ? p.x : -p.x;
            p.y = (Random.Range(0,2) == 0) ? p.y : -p.y;

            Vector2 target_pos = new Vector2(0, 0);
            Vector2 target_dir = target_pos - p;
            Vector2 forward = Vector2.up;
            GameObject enemy = ObjectPool.instance.GetGameObject(obj, p, Quaternion.FromToRotation(forward, target_dir));
            enemy.GetComponent<Enemy0>().ChangeSpeed(Random.Range(0.1f, 2.0f));
        }
    }


}
