using System.Collections;
using StarterAssets;
using UnityEngine;

public class GunController : MonoBehaviour
{
    private Camera _mainCamera;
    private Conductor _conductor;
    private StarterAssetsInputs _input;

    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private GameObject secondaryPrefab;
    
    [SerializeField] private Transform barrelTransform;

    private float beatInput;
    [Range(0.1f, .3f)] [SerializeField] private float inputDelay = .15f;
    public bool IsOnBeat { get; private set; }

    [SerializeField] private float shootForce, upwardForce;
    [SerializeField] private float secondaryForce;

    [SerializeField] private float timeBetweenShots, spread, timeBetweenShooting;

    [SerializeField] private int bulletPerShot;

    private bool isShooting, isReadyToShoot;
    public bool allowInvoke;


    public GameObject MuzzleFlashVFX;

    // public AudioClip shootSFX;
    private void Awake()
    {
        if (transform.parent != null)
        {
            _input = GetComponentInParent<StarterAssetsInputs>();

        }
    }
    private IEnumerator Start()
    {
        _conductor = Conductor.Instance;
        Initialize(_input);

        yield return new WaitUntil(() => _input != null);
    }
    
    
    private void Update()
    {
        if (_input == null) return;
        
        beatInput += Time.deltaTime;
        InputHandler();
    }

    public void Initialize(StarterAssetsInputs input)
    {
        _mainCamera = Camera.main;
        isReadyToShoot = true;
        beatInput = _conductor.secondsPerBeat;
        _input = input;
    }
    private void InputHandler()
    {
        if (_input.attack)
        {
            isShooting = _input.attack;
            
            if (isShooting && isReadyToShoot)
            {
                if (beatInput >= _conductor.secondsPerBeat - inputDelay  && beatInput <= _conductor.secondsPerBeat + inputDelay)
                {
 
                    IsOnBeat = true;
                }
                else
                {
                    IsOnBeat = false;
                }

                Shoot();
            }
        }

        if (_input.secondaryAttack  && _conductor.currentStreak >= 3)
        {
            isShooting = _input.secondaryAttack;
            if (isShooting && isReadyToShoot)
            {            
                _conductor.currentStreak -= 3;
                ShootSecondary();
            }
        }
    }

    
    private void Shoot()
    {
        isReadyToShoot = false;
        if(IsOnBeat) EventManager.instance.playerEvents.OnBeatInputPressed();
        else EventManager.instance.playerEvents.OnBeatInputMissed();
        if(MuzzleFlashVFX != null)
            Instantiate(MuzzleFlashVFX, barrelTransform.position, Quaternion.identity);
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

        
        if (allowInvoke)
        {
            Invoke("ResetShot", timeBetweenShots);
            allowInvoke = false;
        }
        
    }
    private void ShootSecondary()
    {
        isReadyToShoot = false;
        if(MuzzleFlashVFX != null)
            Instantiate(MuzzleFlashVFX, barrelTransform.position, Quaternion.identity);
        Ray ray = _mainCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;
        
        var targetPosition = Physics.Raycast(ray, out hit) ? hit.point : ray.GetPoint(75);

        var x = Random.Range(-spread, spread);
        var y = Random.Range(-spread, spread);
        
        var directionWithoutSpread = targetPosition - barrelTransform.position;
        
        Vector3 directionWithSpread = directionWithoutSpread + new Vector3(x, y, 0);
        
        var spawnedPrefab = Instantiate(secondaryPrefab, barrelTransform.position, Quaternion.identity);
        spawnedPrefab.transform.forward = directionWithSpread.normalized;
        
        spawnedPrefab.GetComponentInChildren<Rigidbody>().AddForce(directionWithSpread.normalized * secondaryForce, ForceMode.Impulse);
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
        _input.secondaryAttack= false;
        beatInput = 0;
    }

}