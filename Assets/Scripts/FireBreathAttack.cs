using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBreathAttack : MonoBehaviour
{
    public int damage;
    public float lifeTime = 3f;
    public float attackRange = 5f;
    public float attackRate = 1f;
    float attackTime = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (lifeTime <= 0.0f)
        {
            Destroy(gameObject);
        }

        lifeTime -= Time.deltaTime;

        if (Time.time >= attackTime)
        {
            Collider[] enemies = Physics.OverlapSphere(transform.position, attackRange);

            foreach (Collider enemy in enemies)
            {
                if (enemy.tag == "Enemy")
                {
                    EnemyStats enemyStats = enemy.gameObject.GetComponent<EnemyStats>();
                    enemyStats.GetHit(damage);
                }
            }

            attackTime = Time.time + 1f / attackRate;
        }
    }
}
