using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : MonoBehaviour
{   
    public GameObject valuePrefab;

    public NPCAttributesSO npcAtt;
    public int health;
    public int damage;

    private Animator animator;
    private int _animIDHit = 0;
    private int _animIDDead = 0;

    bool destroyProcedure = false;
    public float timeToDestroy = 3f;
    public GameObject crystalPrefab;
    public GameObject particlePrefab;

    void Awake()
    {
        if (GetComponent<Animator>() != null)
        {
            animator = GetComponent<Animator>();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        if (animator != null)
        {
            _animIDHit = Animator.StringToHash("Hit");
            _animIDDead = Animator.StringToHash("Dead");
        }

        health = npcAtt.health;
        damage = npcAtt.damage;
    }

    void Update()
    {
        if (destroyProcedure)
        {
            if (timeToDestroy <= 0.0f){
                SpawnCrystal();
                SpawnParticle();
                Destroy(gameObject);
            }
            else
            {
                timeToDestroy -= Time.deltaTime;
            }
        }

        // Too avoid NPCs falling from map
        // if (transform.position.y < 0)
        //     transform.Translate(new Vector3(transform.position.y, 0, transform.position.z));
    }

    public void GetHit(int damage)
    {
        health -= damage;

        GameObject value = Instantiate(valuePrefab,
        new Vector3(transform.position.x + 3f, transform.position.y + 5f,
        transform.position.z), Quaternion.Euler(transform.rotation.x, transform.rotation.y * 180f, transform.rotation.z));

        value.GetComponent<DamageShow>().SetInfo(damage, Color.yellow);

        if (animator != null)
        {
            animator.SetTrigger(_animIDHit);
        }

        if (health <= 0)
        {
            Death();
        }
    }

    void Death()
    {
        if (animator != null)
        {
            animator.SetTrigger(_animIDDead);
        }

        MonoBehaviour[] comps = GetComponents<MonoBehaviour>();
        foreach(MonoBehaviour c in comps)
        {
            c.enabled = false;
        }

        GetComponent<EnemyStats>().enabled = true;
        GetComponent<Collider>().enabled = false;
        Destroy(GetComponent<Rigidbody>());
        destroyProcedure = true;
    }

    void SpawnCrystal()
    {
        if (Random.value > 0.5f)
        {
            Instantiate(crystalPrefab, new Vector3(transform.position.x,
            transform.position.y + 1f, transform.position.z), transform.rotation);
        }
    }

    void SpawnParticle()
    {
        Instantiate(particlePrefab, transform.position, transform.rotation);
    }
}
