using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class RelativeMovement : MonoBehaviour
{
    [SerializeField] private Transform target;
    public float rotSpeed = 15.0f;
    public float moveSpeed = 6.0f;
    public float jumpSpeed = 15.0f;
    public float gravity = -9.8f;
    public float terminalVelocity = -10.0f;
    public float minFall = -1.5f;
    public float pushForce = 3.0f;
    
    private float _vertSpeed;
    
    private CharacterController _charController;
    private ControllerColliderHit _contact;
    
    private Animator _animator;
    
    // Start is called before the first frame update
    void Start()
    {
        _vertSpeed = minFall;
        _charController = GetComponent<CharacterController>();
        _animator = GetComponent<Animator>();
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 movement = Vector3.zero; // starting with a (0, 0, 0) mov. vector
        
        float horInput = Input.GetAxis("Horizontal");
        float vertInput = Input.GetAxis("Vertical");
        if (horInput != 0 || vertInput != 0)
        {
            movement.x = horInput * moveSpeed;
            movement.z = vertInput * moveSpeed;
            movement = Vector3.ClampMagnitude(movement, moveSpeed);
            
            Quaternion tmp = target.rotation; // saving camera's rotation vec.
            target.eulerAngles = new Vector3(0, target.eulerAngles.y, 0); // making a rotation vec. only in X0Z plane for the player
            movement = target.TransformDirection(movement); // from local to global coordinates
            target.rotation = tmp; // ...restoring rotation vec.
            
            // transform.rotation = Quaternion.LookRotation(movement); // player rotates to look in movement vector's direction
            
            Quaternion direction = Quaternion.LookRotation(movement);
            transform.rotation = Quaternion.Lerp(transform.rotation, direction, rotSpeed * Time.deltaTime); // a plain rotation using linear interpolation
        }
        
         _animator.SetFloat("Speed", movement.sqrMagnitude); // animator speed matching
         
        bool hitGround = false;
        RaycastHit hit;
        if (_vertSpeed < 0 && Physics.Raycast(transform.position, Vector3.down, out hit))
        {
            float check = (_charController.height + _charController.radius) / 1.9f;
            hitGround = hit.distance <= check;
        }
       
        // here!!!
        if (hitGround)
        {
            if (Input.GetButtonDown("Jump"))
            {
                _vertSpeed = jumpSpeed;
            }
            else
            {
                _vertSpeed = minFall;
                _animator.SetBool("Jumping", false);
            }
        }
        else
        {
            _vertSpeed += gravity * 5 * Time.deltaTime;
            if (_vertSpeed < terminalVelocity)
            {
                _vertSpeed = terminalVelocity;
            }
            
            if (_contact != null)
            {
                _animator.SetBool("Jumping", true);
            }
            
            if (_charController.isGrounded)
            {
                if (Vector3.Dot(movement, _contact.normal) < 0)
                {
                    movement = _contact.normal * moveSpeed;
                }
                else
                {
                    movement += _contact.normal * moveSpeed;
                }
            }
        }
        movement.y = _vertSpeed;
        
        movement *= Time.deltaTime;
        _charController.Move(movement);
    }
    
    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        _contact = hit;
        
        
        Rigidbody body = hit.collider.attachedRigidbody;
        if (body != null && !body.isKinematic)
        {
            body.velocity = hit.moveDirection * pushForce;
        }
    }
}
