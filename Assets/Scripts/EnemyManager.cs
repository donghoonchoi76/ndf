using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyManager : MonoBehaviour {
    public GameObject[] enemies = new GameObject[5];
    List<float> PhaseDuration = new List<float>();
    float m_fTimer;
    int m_nPhase;

	// Use this for initialization
	void Start () {
        m_nPhase = 0;

        PhaseDuration.Add(15);
        PhaseDuration.Add(5);
        PhaseDuration.Add(15);
        PhaseDuration.Add(5);
        PhaseDuration.Add(15);
        PhaseDuration.Add(5);
        PhaseDuration.Add(0);
        PhaseDuration.Add(15);

        StartCoroutine(RegularGenerator());
	}
	
	// Update is called once per frame
	void Update () {
        m_fTimer += Time.deltaTime;

        if (m_fTimer > PhaseDuration[m_nPhase])
        {
            m_nPhase++;
            m_fTimer = 0;
        }
	}

    IEnumerator RegularGenerator()
    {
        int i = 0;
        do {
            i++;

            switch (m_nPhase)
            {
                case 0:
                    yield return new WaitForSeconds(4);
                    for(int j=0; j<3; j++)
                        GenerateEnemy(enemies[0]);            
                    break;
                case 1:
                    yield return new WaitForSeconds(2);
                    for(int j=0; j<2; j++)
                        GenerateEnemy(enemies[0]);
                    GenerateEnemy(enemies[1]);
                    break;
                case 2:
                    yield return new WaitForSeconds(2);
                    for (int j = 0; j < 2; j++)
                    {
                        GenerateEnemy(enemies[0]);
                        GenerateEnemy(enemies[1]);
                    }
                    break;
                case 3:
                    yield return new WaitForSeconds(1);
                    for (int j = 0; j < 2; j++)
                    {
                        GenerateEnemy(enemies[0]);
                        GenerateEnemy(enemies[1]);
                    }
                    break;
                case 4:
                    yield return new WaitForSeconds(1);
                    for (int j = 0; j < 3; j++)
                    {
                        GenerateEnemy(enemies[0]);
                        GenerateEnemy(enemies[1]);
                    }
                    break;
                default:
                    yield return new WaitForSeconds(1);
                    for (int j = 0; j < 10; j++)
                    {
                        GenerateEnemy(enemies[0]);
                        GenerateEnemy(enemies[1]);
                    }
                    break;
            }            
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
            enemy.GetComponent<EnemyBase>().ChangeSpeed(Random.Range(0.1f, 2.0f));
        }
    }


}
