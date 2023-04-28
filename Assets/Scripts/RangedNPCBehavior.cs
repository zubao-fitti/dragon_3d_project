using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedNPCBehavior : MonoBehaviour
{
    EnemyStats enemyStats;

    // Movement attributes
    Rigidbody _rigidbody;
    public float _walkSpeed = 2f;
    public float _runSpeed = 5f;

    // Combat attributes
    public GameObject spellPrefab;
    public Transform spellPosition;
    public int damage = 10;
    public float _atkRate = 1f;
    float _atkTime = 0f;

    // Detection and path attributes
    private GameObject player;
    bool _outOfWay = false;
    public float _detectionRange = 20f;
    public float _patrolTime = 5f;
    float _currentPatrolTime;
    Vector3 _originPoint;
    float _distanceToPlayer;

    // Animation attributes
    private Animator animator;
    private int _animIDSpeed;
    private int _animIDAttack;

    void Awake()
    {
        enemyStats = GetComponent<EnemyStats>();
        animator = GetComponent<Animator>();
        _rigidbody = GetComponent<Rigidbody>();
    }

    // Start is called before the first frame update
    void Start()
    {
        damage = enemyStats.damage;
        _originPoint = transform.position;
        AssignAnimationIDs();
        player = GameObject.FindWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        _distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);

        if (FindPlayer())
        {
            CastSpell();
            _outOfWay = true;
        }
        else
        {
            if (!_outOfWay)
                Patrol();
            else
                Wayback();
        }
    }

    void AssignAnimationIDs()
    {
        _animIDSpeed = Animator.StringToHash("Speed");
        _animIDAttack = Animator.StringToHash("Attack");
    }

    bool FindPlayer()
    {
        if (_distanceToPlayer < _detectionRange)
            return true;
        else
            return false;
    }

    // Patrol behavior
    void Patrol()
    {
        // Walk for some time and rotate
        if (_currentPatrolTime < _patrolTime)
        {
            _rigidbody.MovePosition(transform.position + transform.forward * _walkSpeed * Time.deltaTime);
            _currentPatrolTime += Time.deltaTime;
        }
        else
        {
            transform.Rotate(0.0f, 90f, 0.0f);
            _currentPatrolTime = 0.0f;
        }

        animator.SetFloat(_animIDSpeed, _walkSpeed, 0.1f, Time.deltaTime);
    }

    void Wayback()
    {
        float distanceToOrigin = Vector3.Distance(_originPoint, transform.position);
        if (distanceToOrigin > 0.2f)
        {
            transform.LookAt(_originPoint, Vector3.up);
            _rigidbody.MovePosition(transform.position + transform.forward * _walkSpeed * Time.deltaTime);

            animator.SetFloat(_animIDSpeed, _walkSpeed, 0.1f, Time.deltaTime);
        }
        else
        {
            transform.rotation = Quaternion.Euler(0f, 180f, 0f);
            _currentPatrolTime = 0.0f;
            _outOfWay = false;
        }
    }

    void CastSpell()
    {
        Vector3 playerPos = new Vector3(player.transform.position.x, 0, player.transform.position.z);
        transform.LookAt(playerPos, Vector3.up);

        if (Time.time >= _atkTime)
        {
            _atkTime = Time.time + 1f / _atkRate;

            GameObject spellObj = Instantiate(spellPrefab, spellPosition.position, Quaternion.identity);
            EnemySpell spell = spellObj.GetComponent<EnemySpell>();
            spell._damage = damage;
            spell.player = player;

            animator.SetTrigger(_animIDAttack);
        }

        GetSafe();
    }

    void GetSafe()
    {
        if (_distanceToPlayer < (_detectionRange / 2f))
        {
            transform.Rotate(0, 180, 0);
            _rigidbody.MovePosition(transform.position + transform.forward * _runSpeed * Time.deltaTime);

            animator.SetFloat(_animIDSpeed, _runSpeed, 0.1f, Time.deltaTime);
        }
        else{
            animator.SetFloat(_animIDSpeed, 0.0f, 0.1f, Time.deltaTime);
        }
    }
}
