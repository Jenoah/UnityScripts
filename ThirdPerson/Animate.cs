using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class Animate : MonoBehaviour
{
    [SerializeField]
    private Animator anim = null;
    [SerializeField]
    private string verticalAxis = "Vertical";
    [SerializeField]
    private string horizontalAxis = "Horizontal";
    private bool isGrounded = true;
    [SerializeField]
    private float fallingGroundDistance = 1.5f;
    private float animMultiplier = 1f;
    
    [Header("Inverse Kinematics (Feet)"), Space(10)]
    [SerializeField]
    private bool useIK = true;
    private float leftFootIKWeight = 1f;
    private float rightFootIKWeight = 1f;
    private Transform leftFootBone = null;
    private Transform rightFootBone = null;
    [SerializeField]
    private float maxFootDistance = 4f;
    [SerializeField]
    private Vector3 footOffset = new Vector3(0f, 0.08f, 0f);

    // Update is called once per frame
    void Update()
    {
        //Sets the movement direction to the corresponding value
        float vertical = Input.GetAxis(verticalAxis) * animMultiplier;
        float horizontal = Input.GetAxis(horizontalAxis) * animMultiplier;
        anim.SetFloat("Vertical", vertical);
        anim.SetFloat("Horizontal", horizontal);

        //Checks if the player is grounded and sets the grounded state to the corresponding value
        if (Physics.Raycast(transform.position + new Vector3(0f, 0.01f, 0f), Vector3.down, fallingGroundDistance, LayerMask.NameToLayer("Player")))
        {
            anim.SetBool("isGrounded", true);
        }
        else
        {
            anim.SetBool("isGrounded", false);
        }
    }

    //Sets the run animation
    public void SetRun(float speed)
    {
        animMultiplier = speed;
    }

    //Plays the jump animation
    public void Jump()
    {
        anim.SetTrigger("Jump");
    }

    //Sets the players crouching state
    public void SetCrouched(bool isCrouching)
    {
        anim.SetBool("isCrouching", isCrouching);
    }

    //Sets the players grounded state
    public void SetGrounded(bool isGrounded)
    {
        this.isGrounded = isGrounded;

    }

    void OnAnimatorIK()
    {
        if (anim)
        {
            if (useIK)
            {

        //Checking whether or not the bones are known to the script
                if (leftFootBone == null)
                {
                    leftFootBone = anim.GetBoneTransform(HumanBodyBones.LeftFoot);
                }
                if (rightFootBone == null)
                {
                    rightFootBone = anim.GetBoneTransform(HumanBodyBones.RightFoot);
                }

        //Setting the weight for the IK position and rotation
                anim.SetIKPositionWeight(AvatarIKGoal.LeftFoot, leftFootIKWeight);
                anim.SetIKPositionWeight(AvatarIKGoal.RightFoot, rightFootIKWeight);
                anim.SetIKRotationWeight(AvatarIKGoal.LeftFoot, leftFootIKWeight);
                anim.SetIKRotationWeight(AvatarIKGoal.RightFoot, rightFootIKWeight);

            //IK for left foot
                RaycastHit leftFootHit;
                if (Physics.Raycast(leftFootBone.position + Vector3.up, Vector3.down, out leftFootHit, maxFootDistance, LayerMask.NameToLayer("Player")))
                {
                    anim.SetIKPosition(AvatarIKGoal.LeftFoot, leftFootHit.point + footOffset);
                    Quaternion prefferedFootRotation = Quaternion.LookRotation(Vector3.ProjectOnPlane(transform.forward, leftFootHit.normal), leftFootHit.normal);
                    anim.SetIKRotation(AvatarIKGoal.LeftFoot, prefferedFootRotation);
                }

            //IK for right foot
                RaycastHit rightFootHit;
                if (Physics.Raycast(rightFootBone.position + Vector3.up, Vector3.down, out rightFootHit, maxFootDistance, LayerMask.NameToLayer("Player")))
                {
                    anim.SetIKPosition(AvatarIKGoal.RightFoot, rightFootHit.point + footOffset);
                    Quaternion prefferedFootRotation = Quaternion.LookRotation(Vector3.ProjectOnPlane(transform.forward, rightFootHit.normal), rightFootHit.normal);
                    anim.SetIKRotation(AvatarIKGoal.RightFoot, prefferedFootRotation);
                }
            }
        }
    }

}
