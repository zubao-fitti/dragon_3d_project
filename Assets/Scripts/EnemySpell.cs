using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpell : MonoBehaviour
{
    public GameObject player;
    public int _damage = 10;
    public float _speed = 10f;
    Rigidbody _rigidbody;
    public float _lifeTime = 5f;
    public GameObject hitEffectPrefab;

    // Start is called before the first frame update
    void Start()
    {
        transform.LookAt(player.transform.position, Vector3.up);
        _rigidbody = GetComponent<Rigidbody>();
        _rigidbody.velocity = transform.forward * _speed;    
    }

    // Update is called once per frame
    void Update()
    {
        if (_lifeTime <= 0.0f)
        {
            Destroy(gameObject);
        }
        _lifeTime -= Time.deltaTime;
    }

    void OnTriggerEnter (Collider col)
    {
        if (col.gameObject == player)
        {
            PlayerController playerCt = col.gameObject.GetComponent<PlayerController>();
            playerCt.SetHealth(-_damage);
            Instantiate(hitEffectPrefab, transform.position, transform.rotation);
            Destroy(gameObject);
        }
    }
}
