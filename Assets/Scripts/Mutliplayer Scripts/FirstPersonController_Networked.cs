using UnityEngine;
using System.Collections;

public class FirstPersonController_Networked : MonoBehaviour
{
    [Header("Cinemachine")]
    [Tooltip("The follow target set in the Cinemachine Virtual Camera that the camera will follow")]
    public GameObject CinemachineCameraTarget;
    [Tooltip("How far in degrees can you move the camera up")]
    public float TopClamp = 90.0f;
    [Tooltip("How far in degrees can you move the camera down")]
    public float BottomClamp = -90.0f;
    [Header("Player")]
    [Tooltip("Move speed of the character in m/s")]
    public float MoveSpeed = 4.0f;
    [Tooltip("Sprint speed of the character in m/s")]
    public float SprintSpeed = 6.0f;
    [Tooltip("Rotation speed of the character")]
    public float RotationSpeed = 1.0f;
    [Tooltip("Acceleration and deceleration")]
    public float SpeedChangeRate = 10.0f;
    private const float _threshold = 0.01f;

    [Space(10)]
    [Tooltip("The height the player can jump")]
    public float JumpHeight = 1.2f;
    [Tooltip("The character uses its own gravity value. The engine default is -9.81f")]
    public float Gravity = -15.0f;
    public bool canDoubleJump;
    private float _cinemachineTargetPitch;
    private float _speed;
    private float _rotationVelocity;
    private float _verticalVelocity;
    private float _terminalVelocity = 53.0f;
    [Space(10)]
    [Tooltip("Time required to pass before being able to jump again. Set to 0f to instantly jump again")]
    public float JumpTimeout = 0.1f;
    [Tooltip("Time required to pass before entering the fall state. Useful for walking down stairs")]
    public float FallTimeout = 0.15f;
    private float _jumpTimeoutDelta;
    private float _fallTimeoutDelta;
    [Header("Dash")]
    [SerializeField] private bool canDash = true;
    [SerializeField] private float dashSpeed;
    [SerializeField] private float dashTime;
    [SerializeField] private float dashCooldown = 1.8f;

    [Header("Player Grounded")]
    [Tooltip("Useful for rough ground")]
    public float GroundedOffset = -0.14f;
    [Tooltip("The radius of the grounded check. Should match the radius of the CharacterController")]
    public float GroundedRadius = 0.5f;
    [Tooltip("What layers the character uses as ground")]
    public LayerMask GroundLayers;


    protected void CameraRotation(Vector3 _input)
    {
        // if there is an input
        if (_input.sqrMagnitude >= _threshold)
        {
            //Don't multiply mouse input by Time.deltaTime

            _cinemachineTargetPitch += _input.y * RotationSpeed * Time.deltaTime;
            _rotationVelocity = _input.x * RotationSpeed * Time.deltaTime;

            // clamp our pitch rotation
            _cinemachineTargetPitch = ClampAngle(_cinemachineTargetPitch, BottomClamp, TopClamp);

            // Update Cinemachine camera target pitch
            CinemachineCameraTarget.transform.localRotation = Quaternion.Euler(_cinemachineTargetPitch, 0.0f, 0.0f);

            // rotate the player left and right
            transform.Rotate(Vector3.up * _rotationVelocity);
        }
    }
    private static float ClampAngle(float lfAngle, float lfMin, float lfMax)
    {
        if (lfAngle < -360f) lfAngle += 360f;
        if (lfAngle > 360f) lfAngle -= 360f;
        return Mathf.Clamp(lfAngle, lfMin, lfMax);
    }

    public void Move(CharacterController _controller, Vector2 _input, bool IsSprinting = false)
    {

        // set target speed based on move speed, sprint speed and if sprint is pressed
        float targetSpeed = IsSprinting ? SprintSpeed : MoveSpeed;
        // a simplistic acceleration and deceleration designed to be easy to remove, replace, or iterate upon

        // note: Vector2's == operator uses approximation so is not floating point error prone, and is cheaper than magnitude
        // if there is no input, set the target speed to 0
        if (_input == Vector2.zero) targetSpeed = 0.0f;

        // a reference to the players current horizontal velocity
        float currentHorizontalSpeed = new Vector3(_controller.velocity.x, 0.0f, _controller.velocity.z).magnitude;

        float speedOffset = 0.1f;
        float inputMagnitude = _input.magnitude;

        // accelerate or decelerate to target speed
        if (currentHorizontalSpeed < targetSpeed - speedOffset || currentHorizontalSpeed > targetSpeed + speedOffset)
        {

            // creates curved result rather than a linear one giving a more organic speed change
            // note T in Lerp is clamped, so we don't need to clamp our speed
            _speed = Mathf.Lerp(currentHorizontalSpeed, targetSpeed * inputMagnitude, Time.deltaTime * SpeedChangeRate);

            // round speed to 3 decimal places
            _speed = Mathf.Round(_speed * 1000f) / 1000f;
        }
        else
        {
            _speed = targetSpeed;
        }

        // normalise input direction
        Vector3 inputDirection = new Vector3(_input.x, 0.0f, _input.y).normalized;

        // note: Vector2's != operator uses approximation so is not floating point error prone, and is cheaper than magnitude
        // if there is a move input rotate player when the player is moving
        if (_input != Vector2.zero)
        {
            // move
            if (GroundedCheck())
            {
                // if (IsSprinting)
                // _playerController.PlayRunAudio();
                // _playerController.PlayWalkAudio();
            }
            inputDirection = transform.right * _input.x + transform.forward * _input.y;
        }

        // if (canDash )
        // {
        //     StartCoroutine(Dash());
        // }
        // move the player
    }

    // private IEnumerator Dash()
    // {
    //     canDash = false;
    //     var tempCD = 0f;
    //     while (tempCD <= dashDuration)
    //     {
    //         tempCD += Time.deltaTime;
    //         yield return null;
    //     }
    //     StartCoroutine(ResetDash());
    // }
    private IEnumerator ResetDash()
    {
        var tempCD = 0f;
        while (tempCD <= dashCooldown)
        {
            tempCD += Time.deltaTime;
            yield return null;
        }
        canDash = true;

        yield return new WaitUntil(() => canDash);

    }
    private void JumpAndGravity(bool JumpInput)
    {
        if (GroundedCheck())
        {
            // reset the fall timeout timer
            _fallTimeoutDelta = FallTimeout;
            // stop our velocity dropping infinitely when grounded
            if (_verticalVelocity < 0.0f)
            {

                _verticalVelocity = -2f;
            }
            // Jump
            if (JumpInput && _jumpTimeoutDelta <= 0.0f)
            {
                canDoubleJump = true;
                // _playerController.PlayJumpAudio(jumpSFXClip, transform.position, 1f);


                // the square root of H * -2 * G = how much velocity needed to reach desired height
                _verticalVelocity = Mathf.Sqrt(JumpHeight * -2f * Gravity);
            }
            // jump timeout
            if (_jumpTimeoutDelta >= 0.0f)
            {
                _jumpTimeoutDelta -= Time.deltaTime;
            }
        }
        else if (!GroundedCheck() && canDoubleJump)
        {

            if (JumpInput && _jumpTimeoutDelta <= 0.0f)
            {
                // _playerController.PlayJumpAudio(jumpSFXClip, transform.position, 1f);

                // the square root of H * -2 * G = how much velocity needed to reach desired height
                _verticalVelocity = Mathf.Sqrt(JumpHeight / 2 * -2f * Gravity) * 2;
                canDoubleJump = false;
            }
        }
        {
            // reset the jump timeout timer
            _jumpTimeoutDelta = JumpTimeout;

            // fall timeout
            if (_fallTimeoutDelta >= 0.0f)
            {

                _fallTimeoutDelta -= Time.deltaTime;
            }

            // if we are not grounded, do not jump


        }
        // apply gravity over time if under terminal (multiply by delta time twice to linearly speed up over time)
        if (_verticalVelocity < _terminalVelocity)
        {
            _verticalVelocity += Gravity * Time.deltaTime;
        }
    }

    private bool GroundedCheck()
    {
        Vector3 spherePosition = new Vector3(transform.position.x, transform.position.y - GroundedOffset, transform.position.z);
        return Physics.CheckSphere(spherePosition, GroundedRadius, GroundLayers, QueryTriggerInteraction.Ignore);
    }
}