using System.Collections;
using Interfaces;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;
public class EnemyController : MonoBehaviour, IDamageable
{
    private Transform _player;
    private NavMeshAgent _agent;
    private Animator _animator;
    
    
    [SerializeField] private float _speed;
    [SerializeField] private float _attackRange = 2f;
    [SerializeField] private float _attackDelay = 1.2f;
    [SerializeField] private bool _isAttacking;


    [SerializeField] private float maxHealth = 100f;
    private float currentHealth;
    private void Start()
    {
        
        InitializeEnemy();
        
    }

    private void Update()
    {
        ChasePlayer();
        Attack();
    }

    private void InitializeEnemy()
    {
        _agent = GetComponent<NavMeshAgent>();
        _player = GameObject.FindGameObjectWithTag("Player").transform;
        _animator = GetComponentInChildren<Animator>();
        
        _agent.speed = _speed;
        _agent.stoppingDistance = _attackRange - .2f;
        
        currentHealth = maxHealth;
    }


    private void ChasePlayer()
    {
        if (_player == null || _isAttacking) return;
        _agent.isStopped = false;

        var DistanceToPlayer= Vector3.Distance(_player.position, transform.position);
        if (DistanceToPlayer <= _attackRange)
        {        
            _isAttacking = true;
            StartCoroutine(Attack());
        }
        _animator.SetFloat("Speed", _agent.velocity.magnitude);
        _agent.SetDestination(_player.position);
    }


    private IEnumerator Attack()
    {
        yield return new WaitForSeconds(_attackDelay);
        _animator.SetBool("Attack", _isAttacking);
        _animator.SetInteger("Attack Number", Random.Range(0,3));
        _isAttacking = false;
    }


    public void TakeDamage(float Damage)
    {
        currentHealth -= Damage;
        _agent.isStopped = true;
        if (currentHealth <= 0)
        {
            _animator.SetBool("Dead", true);
            
        }
        _animator.SetTrigger("TakeDamage");
    }

    public void Destroy()
    {
        throw new System.NotImplementedException();
    }
}