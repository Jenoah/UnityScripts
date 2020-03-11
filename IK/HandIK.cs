using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandIK : MonoBehaviour
{
    [Header("Anchors")]
    [SerializeField]
    private Transform leftHandAnchor = null;
    [SerializeField]
    private Transform rightHandAchor = null;

    [Header("Offsets")]
    [SerializeField]
    private Vector3 leftHandPositionOffset = Vector3.zero;
    [SerializeField]
    private Vector3 leftHandRotationOffset = Vector3.zero;
    [SerializeField]
    private Vector3 rightHandPositionOffset = Vector3.zero;
    [SerializeField]
    private Vector3 rightHandRotationOffset = Vector3.zero;

    private Animator anim = null;
    // Start is called before the first frame update
    void Start()
    {
        if(anim == null)
        {
            anim = GetComponent<Animator>();
        }
    }

    private void OnAnimatorIK(int layerIndex)
    {
        if(anim != null)
        {
            //Position Weight
            anim.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1f);
            anim.SetIKPositionWeight(AvatarIKGoal.RightHand, 1f);

            //Rotation Weight
            anim.SetIKRotationWeight(AvatarIKGoal.LeftHand, 1f);
            anim.SetIKRotationWeight(AvatarIKGoal.RightHand, 1f);

            anim.SetIKPosition(AvatarIKGoal.LeftHand, leftHandAnchor.position + leftHandPositionOffset);
            anim.SetIKPosition(AvatarIKGoal.RightHand, rightHandAchor.position + rightHandPositionOffset);

            anim.SetIKRotation(AvatarIKGoal.LeftHand, leftHandAnchor.rotation * Quaternion.Euler(leftHandRotationOffset));
            anim.SetIKRotation(AvatarIKGoal.RightHand, rightHandAchor.rotation * Quaternion.Euler(rightHandRotationOffset));
        }
    }
}
