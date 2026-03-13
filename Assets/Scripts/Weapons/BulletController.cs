using System;
using Unity.Mathematics;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    private Rigidbody rb;

    public LayerMask enemiesLayerMask;

    public float damage;
    [SerializeField] private float maxLifeTime = 3f;
    
    public bool isBullet;
    [Header("Bounce Settings")] 
    public bool useGravity, explodeOnCollision;
    [Range(0,1)]public float _bounciness;
    [SerializeField] private float maxBounces, currentBounces;
    [SerializeField] private AudioClip bounceSound;
    [SerializeField] private float explosionRadius, explosionForce;
    [SerializeField] private AudioClip explosionSound;
    [SerializeField] private GameObject explosionGameObject;
    [SerializeField] private GameObject hitImpact;
    private PhysicsMaterial _bounceMaterial;

  
    private void Start()
    {
        Setup();
    }
    
    private void Setup()
    {
        rb = GetComponent<Rigidbody>();
        if (!isBullet)
        {
            _bounceMaterial = new PhysicsMaterial
            {
                bounciness = _bounciness,
                bounceCombine = PhysicsMaterialCombine.Maximum,
                frictionCombine = PhysicsMaterialCombine.Minimum
            };
            GetComponent<SphereCollider>().material = _bounceMaterial;
        }
        rb.useGravity = useGravity;
    }

    private void Update()
    {
        maxLifeTime -= Time.deltaTime;  
        if (!isBullet)
        {
            if(currentBounces > maxBounces) Explode();
        
            if(maxLifeTime <= 0) Explode();
        }
        else
        {
            if(maxLifeTime <= 0) BulletImpact();
        }
    }

    private GameObject spawnedGO;
    private void Explode()
    {

        if (explosionGameObject != null && spawnedGO == null)
        {
             spawnedGO = Instantiate(explosionGameObject, transform.position, Quaternion.identity);
        }
        Collider[] enemies = Physics.OverlapSphere(transform.position, explosionRadius, enemiesLayerMask);
        foreach (var e in enemies)
        {

            if (e.GetComponent<Rigidbody>() != null)
            {
                var eRb = e.GetComponent<Rigidbody>();
                eRb.AddExplosionForce(explosionForce, transform.position, explosionRadius);
            }
            
        }
        
        Destroy(gameObject, 0.05f);
    }

    private void BulletImpact()
    {
        Destroy(gameObject,0.05f);
    }
    private void OnCollisionEnter(Collision other)
    {
        if (isBullet)
        {
            if (other.collider.CompareTag("Enemy"))
            {
                if(hitImpact != null) Instantiate(hitImpact, other.contacts[0].point, Quaternion.identity);
                if(other.collider.TryGetComponent<EnemyController>(out EnemyController e) )
                   e.TakeDamage(damage);
            }
            Destroy(gameObject);
        }
        else
        {
            currentBounces++;
            // Instantiate(explosionGameObject, transform.position, Quaternion.identity);
            if(other.collider.CompareTag("Enemy") && explodeOnCollision) Explode();
        }
       
    }

}