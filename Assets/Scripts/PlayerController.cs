using System.Collections.Generic;
using System.Runtime.CompilerServices;
using StarterAssets;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerController : MonoBehaviour
{
    //Components
    Conductor _conductor;
    private StarterAssetsInputs _input;
    private Camera _mainCamera;

    //Gun References
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform barrelTransform;

    [SerializeField] private float shootForce, upwardForce;

    [SerializeField] private float timeBetweenShots, spread, timeBetweenShooting;

    [SerializeField] private int bulletPerShot;

    private bool isShooting, isReadyToShoot;

    public bool allowInvoke;


    private float beatInput;
    [Range(0.1f, .3f)] [SerializeField] private float inputDelay = .15f;

    public bool isOnBeat { get; private set; }


    public AudioSource _gunAudioSource;
    public AudioClip shootSFX, _readyToShootSFX;

    public GameObject GroundSlamVFX;
    [SerializeField] private float slamRadius = 5f;
    void Start()
    {
        _conductor = Conductor.Instance;
        _mainCamera = Camera.main;
        _input = GetComponent<StarterAssetsInputs>();

        beatInput = _conductor.secondsPerBeat;
        isReadyToShoot = true;
    }


    // Update is called once per frame
    private void Update()
    {
        if (beatInput > _conductor.secondsPerBeat)
        {
            beatInput = 0;
        }
        beatInput += Time.deltaTime;
        InputHandler();
    }
    
    private void InputHandler()
    {
        if (_input.attack)
        {
            isShooting = _input.attack;
            if ( beatInput <= _conductor.secondsPerBeat && beatInput > _conductor.secondsPerBeat - inputDelay)
            {
                isOnBeat = true;
            }
            else
            {
                isOnBeat = false;
            }
            
            if (isShooting && isReadyToShoot)
            {
                Shoot();
            }
        }
 
    }
    private void Shoot()
    {
        isReadyToShoot = false;
        
        Ray ray = _mainCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;
        
        var targetPosition = Physics.Raycast(ray, out hit) ? hit.point : ray.GetPoint(75);

        var x = Random.Range(-spread, spread);
        var y = Random.Range(-spread, spread);
        
        var directionWithoutSpread = targetPosition - barrelTransform.position;
        
        Vector3 directionWithSpread = directionWithoutSpread + new Vector3(x, y, 0);
        
        var spawnedPrefab = Instantiate(bulletPrefab, barrelTransform.position, Quaternion.identity);
        spawnedPrefab.transform.forward = directionWithSpread.normalized;
        
        spawnedPrefab.GetComponentInChildren<Rigidbody>().AddForce(directionWithSpread.normalized * shootForce, ForceMode.Impulse);
        spawnedPrefab.GetComponentInChildren<Rigidbody>().AddForce(_mainCamera.transform.up * upwardForce, ForceMode.Impulse);


        if (allowInvoke)
        {
            Invoke("ResetShot", timeBetweenShots);
            allowInvoke = false;
        }
        
    }

    private void ResetShot()
    {
        isReadyToShoot = true;
        allowInvoke = true;
        _input.attack = false;
        beatInput = 0;
    }

    
    public void GroundSlam(float offset)
    {
        //The Transform position of where the player's feet landed in relation to the player controller offset
        var slamPosition = new Vector3(transform.position.x, transform.position.y - offset, transform.position.z);
        //Colliders of all within the overlap sphere of the ground slam
        var checkSphereSlammable = Physics.OverlapSphere(slamPosition, slamRadius);
        //Check if an entity if within direct radius or further from the slam
        foreach (var slammedEntity in checkSphereSlammable)
        {
            if (slammedEntity.TryGetComponent<Interactable_GroundSlam>(out var slammed))
            {
                Debug.Log(slammed);
                var distanceFromPlayer = Vector3.Distance(transform.position, slammed.GetTransform().position);
                if (distanceFromPlayer >= 1.5f)
                {
                    slammed.WithinSlamRadius(distanceFromPlayer);
                }
                else
                {
                    slammed.DirectSlam();
                }
            }
            
        }
        // Instantiate(GroundSlamVFX, slamPosition, Quaternion.identity);
    }
}
