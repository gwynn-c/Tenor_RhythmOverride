
using System.Collections;
using StarterAssets;
using Unity.VisualScripting;
using UnityEngine;

public class ThrowableController : MonoBehaviour
{
    private Camera _mainCamera;
    private StarterAssetsInputs _input;
    private Conductor _conductor;
    private PlayerUIController _uiController;
    [SerializeReference]private Transform throwableTransform;
    [SerializeReference] private GameObject throwablePrefab;
    [SerializeReference] private float throwForce;
    [SerializeReference] private float verticalThrowForce;
    [SerializeReference] private float timeBetweenThrows;

    private bool isReadyToThrow, isThrowing;
    private bool allowInvoke;
    


    private void Awake()
    {
        _input = GetComponent<StarterAssetsInputs>();
        _uiController = GetComponent<PlayerUIController>();
    }

    private IEnumerator Start()
    {
        _conductor = Conductor.Instance;
        Initialize(_input);

        yield return new WaitUntil(() => _input != null);
    }

    private void Initialize(StarterAssetsInputs input)
    {
        _mainCamera = Camera.main;
        isReadyToThrow = true;
        _input = input;    
        allowInvoke = true;
    }

    private void Update()
    {
        if (_input.specialAttack)
        {
            isThrowing = _input.specialAttack;
            if (isThrowing && isReadyToThrow)
            {            
                Throw();
            }
        }
    }
    
    private void Throw()
    {
        isReadyToThrow = false;
        
        Ray ray = _mainCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;
        _uiController.ThrowableCooldownFill.fillAmount = 0;
        var targetPosition = Physics.Raycast(ray, out hit) ? hit.point : ray.GetPoint(75);

      
        var directionWithoutSpread = targetPosition - throwableTransform.position;

        
        var spawnedPrefab = Instantiate(throwablePrefab, throwableTransform.position, Quaternion.identity);
        spawnedPrefab.transform.forward = directionWithoutSpread.normalized;
        
        spawnedPrefab.GetComponentInChildren<Rigidbody>().AddForce(directionWithoutSpread.normalized * throwForce, ForceMode.Impulse);
        spawnedPrefab.GetComponentInChildren<Rigidbody>().AddForce(_mainCamera.transform.up * verticalThrowForce, ForceMode.Impulse);

        StartCoroutine(nameof(ResetThrow));
    }
    
    
    private IEnumerator ResetThrow()
    {
        var tempCD = 0f;
        while (tempCD <= timeBetweenThrows)
        {
            tempCD += Time.deltaTime;
            _uiController.ThrowableCooldownFill.fillAmount = tempCD/timeBetweenThrows;
            yield return null;
        }
        isReadyToThrow = true;
        _input.specialAttack= false;
        yield return new WaitUntil(() => isReadyToThrow);
    }

}

