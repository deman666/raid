
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(CharacterController))]
public class CharacterMovement : MonoBehaviour
{

    Animator animator;
    CharacterController characterController;

    [System.Serializable]
    public class AnimationSettings
    {
        public string verticalVelocityFloat = "Forward";
        public string horizontalVelocityFloat = "Strafe";
        public string groundedBool = "isGrounded";
        public string jumpBool = "isJumping";
    }
    [SerializeField]
    public AnimationSettings animations;

    [System.Serializable]
    public class PhysicsSettings
    {
        public float gravityModifier = 9.81f;
        public float baseGravity = 50f;
        public float resetGravityValue = 1.2f;
    }
    [SerializeField]
    public PhysicsSettings physics;

    [System.Serializable]
    public class MovementSettings
    {
        public float jumpSpeed = 6f;
        public float jumpTime = 0.3f;
        public float speed = 12f;

    }
    [SerializeField]
    public MovementSettings movement;

    bool jumping;
    bool resetGravity;
    float gravity;
    //bool isGrounded = true;

    void Start()
    {
        animator = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();
        SetupAnimator();

        adrianGravity = -9.81f * gravityMutiplier;
    }

    void Update()
    {
        //ApplyGravity();
        //isGrounded = characterController.isGrounded;

        //Movement();

        AdrianMovement();
    }

    bool isGrounded { get { return characterController.isGrounded; } }

    Vector3 adrianVelocity = Vector3.zero;
    bool jumpRequested;

    public float gravityMutiplier = 1f;
    private float adrianGravity;
    void AdrianMovement()
    {
        float velocityY = adrianVelocity.y;

        // read keys for horizontal movement
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        adrianVelocity.x = x;
        adrianVelocity.z = z;

        adrianVelocity = transform.right * x + transform.forward * z;
        adrianVelocity *= movement.speed;


        // control vertical velocity with gravity & contact
        velocityY += adrianGravity * Time.deltaTime;

        if (jumpRequested && characterController.isGrounded)
        {
            velocityY = movement.jumpSpeed;
            jumpRequested = false;
        }
        else if (characterController.isGrounded && adrianVelocity.y < 0f)
        {
            velocityY = 0f;  
        }

        adrianVelocity.y = velocityY;

        characterController.Move(adrianVelocity * Time.deltaTime);

    }
   

    public void Animate(float forward, float strafe)
    {
        // #Adrian temp comment-out
        animator.SetFloat(animations.verticalVelocityFloat, forward);
        animator.SetFloat(animations.horizontalVelocityFloat, strafe);
        animator.SetBool(animations.groundedBool,isGrounded);
        animator.SetBool(animations.jumpBool, jumping);
    }


    //Character  movement
    public void Movement()
    {
        
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
       
        Vector3 move = transform.right * x + transform.forward * z;
        

        move.Normalize();

        characterController.Move(move * movement.speed * Time.deltaTime);

        
    }

    public void Jump()
    {
        //if (isGrounded)
        {
            //adrianVelocity.y = movement.jumpSpeed;
            jumpRequested = true;
        }
        Debug.Log("trying to jump");
        //if (jumping)
        //    return;
        //if (isGrounded)
        //{
        //    jumping = true;
        //    StartCoroutine(StopJump());
        //}
    }

    //IEnumerator StopJump()
    //{
    //    yield return new WaitForSeconds(movement.jumpTime);
    //    jumping = false;

    //}

    //void ApplyGravity()
    //{
    //    if (!characterController.isGrounded)
    //    {
    //        if (!resetGravity)
    //        {
    //            gravity = physics.resetGravityValue;
    //            resetGravity = true;
    //        }
    //        gravity += Time.deltaTime * physics.gravityModifier;
    //    }

    //    else
    //    {
    //        gravity = physics.baseGravity;
    //        resetGravity = false;
    //    }

    //    Vector3 gravityVector = new Vector3();
    //    if (!jumping)
    //    {
    //        if (!characterController.isGrounded)
    //        {
    //            gravityVector.y -= gravity;
    //        }
    //        else
    //        {
    //            gravityVector = Vector3.zero;
    //        }

    //    }
    //    else
    //    {
    //        gravityVector.y = movement.jumpSpeed;
    //    }
    //    characterController.Move(gravityVector * Time.deltaTime);
    //}


     void SetupAnimator()
    {
        Animator wantedAnim = GetComponentsInChildren<Animator>()[1];
        Avatar wantedAvatar = wantedAnim.avatar;

        animator.avatar = wantedAvatar;
        Destroy(wantedAnim);
    }

}
