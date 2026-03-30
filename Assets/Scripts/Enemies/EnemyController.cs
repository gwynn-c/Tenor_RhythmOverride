using System.Collections;
using Interfaces;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;
public class EnemyController : MonoBehaviour, IDamageable
{
    private Transform _player;
    private NavMeshAgent _agent;
    private Animator _animator;

    [SerializeField] private bool IsRanged;
    [SerializeField] private float _speed;
    [SerializeField] private float _attackRange = 2f;
    [SerializeField] private float _attackDelay = 1.2f;
    [SerializeField] private bool _isAttacking;


    [SerializeField] private float maxHealth = 100f;
    private float currentHealth;

    public GameObject HealthBarGO;
    public float offset = 3.5f;
    [SerializeField] private GameObject deathVFX;
    [SerializeField] private bool allowInvoke;
    private bool isReadyToShoot;
    [SerializeField] private float timeBetweenShots;
    [SerializeField] private float bulletSpeed;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform barrelTransform;
    public bool isDead;

    private void Start()
    {
        
        InitializeEnemy();
        
    }

    private void Update()
    {
        ChasePlayer();
        Attack();
        transform.LookAt(_player);

    }

    private void InitializeEnemy()
    {
        _agent = GetComponent<NavMeshAgent>();
        _player = GameObject.FindGameObjectWithTag("Player").transform;
        _animator = GetComponentInChildren<Animator>();
        
        _agent.speed = _speed;
        _agent.stoppingDistance = _attackRange - .2f;

        if (IsRanged) _attackDelay = timeBetweenShots;
        isReadyToShoot = true;
        currentHealth = maxHealth;
    }


    private void ChasePlayer()
    {
        if (_player == null || _isAttacking ) return;
        _agent.isStopped = false;

        var DistanceToPlayer= Vector3.Distance(_player.position, transform.position);
        if (DistanceToPlayer <= _attackRange)
        {        
            if(!IsRanged)
                _isAttacking = true;
            StartCoroutine(Attack());
        }
        
        _animator.SetFloat("Speed", _agent.velocity.magnitude);
        _agent.SetDestination(_player.position);
    }


    private IEnumerator Attack()
    {
        
        yield return new WaitForSeconds(_attackDelay);

        if(!IsRanged)
        {
            _animator.SetBool("Attack", _isAttacking);
            _animator.SetInteger("Attack Number", Random.Range(0, 3));
            _isAttacking = false;
        }
        else if(IsRanged && isReadyToShoot)
        {
            Shoot();
        }
    }


    public void TakeDamage(float Damage)
    {
        currentHealth -= Damage;
        _agent.isStopped = true;
        _agent.velocity = Vector3.zero;
        if (currentHealth <= 0)
        {
            Death();
            
        }
        _animator.SetTrigger("TakeDamage");
    }

    public void Death()
    {
        EventManager.instance.playerEvents.OnEnemyKilled();

        isDead = true;
        _animator.CrossFade("Death", .1f);
        _agent.isStopped = true;
        GetComponent<Rigidbody>().linearVelocity = Vector3.zero;
        GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        if (deathVFX != null)
        {

            var temp = Instantiate(deathVFX, new Vector3(transform.position.x, transform.position.y + 10, transform.position.z), Quaternion.identity);
            temp.transform.SetParent(transform);
        }
        EventManager.instance.playerEvents.OnEnemyKilled();

        this.enabled = false;
        Destroy(gameObject, 5f);
    }
    
    private void Shoot()
    {
        isReadyToShoot = false;
        var directionWithoutSpread = _player.position - barrelTransform.position;
        
        
        var spawnedPrefab = Instantiate(bulletPrefab, barrelTransform.position, Quaternion.identity);
        spawnedPrefab.transform.forward = directionWithoutSpread.normalized;
        
        spawnedPrefab.GetComponentInChildren<Rigidbody>().AddForce(directionWithoutSpread.normalized * bulletSpeed, ForceMode.Impulse);

        
        if (allowInvoke)
        {
            Invoke("ResetShot", timeBetweenShots);
            allowInvoke = false;
        }
        
    }
    
    private void ResetShot()
    {
        isReadyToShoot = true;
        _isAttacking = false;

        allowInvoke = true;
    }
}