using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private CharacterController character;
    public HealthBar healthBar;
    public FireBreathBar fireBar;
    public GameObject valuePrefab;

    // Player attributes
    public DragonAttributesSO dragonAtt;
    public int health = 999;
    int maxHealth;
    public float flyEnergy = 999;
    public int basicDamage = 999;
    public int fireDamage = 999;

    // Player movement attributes
    public float _speed = 5f;
    public float _turnSmoothTime = 0.1f;
    float _turnSmoothVelocity;

    public float _gravity = -9.81f;
    public float _jumpHeight = 5f;

    // Grounded check
    public bool _isGrounded = true;
    Transform groundCheck;
    public float groundDistance = 0.4f;
    Vector3 gravityVelocity;
    public LayerMask groundMask;
    
    public Transform cam;

    // Attack prefabs
    public Transform basicAttackPosition;
    public float basicAttackRange = 2f;
    public float basicAttackRate = 2f;
    float basicAttackTime = 0f;
    public GameObject fireAttackPrefab;
    public float fireAttackRate = 0.1f;
    float fireAttackTime = 0f;

    // Animator and animations IDs
    private Animator animator;
    private int _animIDSpeed;
    private int _animIDGrounded;
    private int _animIDJump;
    private int _animIDFreeFall;
    private int _animIDBasicAttack;
    private int _animIDFireAttack;
    private int _animIDHit;
    private int _animIDDead;

    public FinishScreen finishSc;

    void Awake()
    {
        animator = GetComponent<Animator>();
        groundCheck = GetComponent<Transform>();
        character = GetComponent<CharacterController>();
    }

    void Start()
    {
        health = dragonAtt.health;
        maxHealth = health;
        flyEnergy = dragonAtt.flyEnergy;
        basicDamage = dragonAtt.basicDamage;
        fireDamage = dragonAtt.fireDamage;

        healthBar.SetMaxHealth(maxHealth);
        fireBar.skillRate = fireAttackRate;

        Cursor.lockState = CursorLockMode.Locked;
        AssignAnimationIDs();
    }

    void Update()
    {
        JumpAndGravity();
        GroundedCheck();
        Move();
        Attack();
    }

    void AssignAnimationIDs()
    {
        _animIDSpeed = Animator.StringToHash("Speed");
        _animIDGrounded = Animator.StringToHash("Grounded");
        _animIDJump = Animator.StringToHash("Jump");
        _animIDFreeFall = Animator.StringToHash("FreeFall");
        _animIDBasicAttack = Animator.StringToHash("BasicAttack");
        _animIDFireAttack = Animator.StringToHash("FireAttack");
        _animIDHit = Animator.StringToHash("Hit");
        _animIDDead = Animator.StringToHash("Dead");
    }

    void JumpAndGravity()
    {
        // Check for grounded
        if (_isGrounded)
        {
            animator.SetBool(_animIDJump, false);
            animator.SetBool(_animIDFreeFall, false);

            // Limit fall speed
            if (gravityVelocity.y < 0.0f)
            {
                gravityVelocity.y = -2f;
            }

            // Jump
            if (Input.GetButtonDown("Jump"))
            {
                gravityVelocity.y = Mathf.Sqrt(_jumpHeight * -2f * _gravity);
                animator.SetBool(_animIDJump, true);
            }
        }
        else
        {
            animator.SetBool(_animIDFreeFall, true);
        }

        // Apply gravity
        gravityVelocity.y += _gravity * Time.deltaTime;
        character.Move(gravityVelocity * Time.deltaTime);
    }

    void GroundedCheck()
    {
        _isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        animator.SetBool(_animIDGrounded, _isGrounded);
    }

    void Move()
    {
        // Receive Input
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(horizontal, 0f, vertical).normalized;

        // Move the player
        if (movement.magnitude > 0)
        {
            float targetAngle = Mathf.Atan2(movement.x, movement.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref _turnSmoothVelocity, _turnSmoothTime);
            
            transform.rotation = Quaternion.Euler(0f, angle, 0f);
            Vector3 moveForward = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            character.Move(moveForward.normalized * _speed * Time.deltaTime);
        }

        // Animate
        float velocity = movement.normalized.magnitude;

        animator.SetFloat("Velocity", velocity, 0.1f, Time.deltaTime);
    }

    void Attack()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            if (Time.time >= basicAttackTime)
            {
                Collider[] enemies = Physics.OverlapSphere(basicAttackPosition.position, basicAttackRange);

                foreach (Collider enemy in enemies)
                {
                    if (enemy.tag == "Enemy")
                    {
                        EnemyStats enemyStats = enemy.gameObject.GetComponent<EnemyStats>();
                        enemyStats.GetHit(basicDamage);
                    }
                }
                animator.SetTrigger(_animIDBasicAttack);

                basicAttackTime = Time.time + 1f / basicAttackRate;
            }
        }

        if (Input.GetButtonDown("Fire2"))
        {
            if (Time.time >= fireAttackTime)
            {
                GameObject fireAttackObj = Instantiate(fireAttackPrefab, basicAttackPosition.position, Quaternion.identity);
                fireAttackObj.transform.parent = transform;
                fireAttackObj.GetComponent<FireBreathAttack>().damage = fireDamage;

                animator.SetTrigger(_animIDFireAttack);

                fireAttackTime = Time.time + 1f / fireAttackRate;

                fireBar.SetCooldown();
            }
            // Instantiate fire

            // Play fire attack animation
        }
    }

    // Function to gain or lose HealthPoints
    public void SetHealth(int hp)
    {
        health += hp;

        GameObject value = Instantiate(valuePrefab,
        new Vector3(transform.position.x, transform.position.y + 4f,
        transform.position.z), transform.rotation);

        if (hp < 0)
        {
            animator.SetTrigger(_animIDHit);

            HPCheck();

            value.GetComponent<DamageShow>().SetInfo(hp, Color.red);
        }
        else
        {
            if (health > maxHealth)
            {
                health = maxHealth;
            }

            value.GetComponent<DamageShow>().SetInfo(hp, Color.green);
        }

        healthBar.SetHealth(health);
    }


    // Check health on every update
    // When the health points reach 0 the dragon dies and the game is over
    void HPCheck()
    {
        if (health <= 0)
        {
            animator.SetTrigger(_animIDDead);
            finishSc.FinishGame();
            character.enabled = false;
            GetComponent<Collider>().enabled = false;
            GetComponent<PlayerController>().enabled = false;
        }
    }

    void OnDrawGizmosSelected()
    {
        if (basicAttackPosition == null)
        {
            return;
        }

        Gizmos.DrawWireSphere(basicAttackPosition.position, basicAttackRange);
    }
}
