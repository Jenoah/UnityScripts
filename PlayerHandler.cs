using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]
public class PlayerHandler : MonoBehaviour
{
    private Rigidbody rb = null;

    [SerializeField]
    private float movementSpeed = 5f;
    [SerializeField]
    private float rotationSpeed = 30f;

    [Header("Crouch settings")]
    [SerializeField]
    private float crouchTime = 1f;
    [SerializeField]
    private float crouchHeight = 1.25f;
    private float defaultCharacterHeight = 0f;
    private CapsuleCollider playerCollider = null;
    private Vector3 userForce = Vector3.zero;
    private float characterHeight = 1;

    [Header("Other")]
    [SerializeField]
    private float jumpHeight = 2f;
    [SerializeField]
    private float groundDistance = 0.2f;
    


    // Use this for initialization
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        playerCollider = GetComponent<CapsuleCollider>();
        defaultCharacterHeight = playerCollider.height;
        characterHeight = playerCollider.height;
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void Update() {
        InitCrouch();
        Jump();
    }

    void Move()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        userForce = new Vector3(horizontal, 0f, vertical);
        userForce = userForce.normalized * movementSpeed * Time.deltaTime;
        userForce = transform.TransformDirection(userForce);
        if (!Input.GetMouseButton(1) && userForce != Vector3.zero)
        {
            float mouseX = Input.GetAxis("Mouse X") * rotationSpeed * Time.deltaTime;

            Quaternion newRotation = Quaternion.Euler(new Vector3(0f, mouseX, 0f));
            //rb.rotation = Quaternion.Euler(0f, rb.rotation.eulerAngles.y, 0f);
            rb.MoveRotation(rb.rotation * newRotation);
            //transform.Rotate();
        }

        rb.MovePosition(rb.position + userForce);
    }

    void Jump(){
        if(Input.GetKeyDown(KeyCode.Space) && isGrounded()){
            print("Jump");
            rb.velocity = new Vector3(rb.velocity.x, jumpHeight, rb.velocity.z);
        }else{
            print("Is grounded = " + isGrounded());
        }
    }

    void InitCrouch(){
        if(Input.GetKeyDown(KeyCode.LeftControl)){
            StopCoroutine(Crouch(true));
            StartCoroutine(Crouch(true));
        }else if(Input.GetKeyUp(KeyCode.LeftControl)){
            StopCoroutine(Crouch(false));
            StartCoroutine(Crouch(false));
        }
    }

    IEnumerator Crouch(bool isCrouching){

        float elapsedTime = 0f;
        characterHeight = playerCollider.height;
        if(isCrouching){
            while(elapsedTime < crouchTime){
                elapsedTime += Time.deltaTime;
                characterHeight = Mathf.Lerp(characterHeight, crouchHeight, elapsedTime / crouchTime);
                playerCollider.height = characterHeight;
                transform.localScale = new Vector3(transform.localScale.x, characterHeight / 2, transform.localScale.z);
                yield return new WaitForEndOfFrame();
            }
        }else{
            while(elapsedTime < crouchTime){
                elapsedTime += Time.deltaTime;
                characterHeight = Mathf.Lerp(characterHeight, defaultCharacterHeight, elapsedTime / crouchTime);
                playerCollider.height = characterHeight;
                transform.localScale = new Vector3(transform.localScale.x, characterHeight / 2, transform.localScale.z);
                yield return new WaitForEndOfFrame();
            }
        }
    }

    private bool isGrounded(){
        Debug.DrawRay(transform.position, Vector3.down * (characterHeight / 2 + groundDistance), Color.cyan, 0.3f);
        if(Physics.Raycast(transform.position, Vector3.down, characterHeight / 2 + groundDistance, LayerMask.NameToLayer("Player"))){
            return true;
        }
        return false;
    }
}
