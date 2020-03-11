using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootIK : MonoBehaviour
{
    private Transform leftFootAnchor = null;
    private Transform rightFootAchor = null;

    private Vector3 leftCurrentPosition = Vector3.zero;
    private Vector3 rightCurrentPosition = Vector3.zero;

    private Quaternion leftCurrentRotation = Quaternion.identity;
    private Quaternion rightCurrentRotation = Quaternion.identity;

    private AudioManager audioManager = null;

    private Animator anim = null;

    [Header("Home transforms")]
    [SerializeField]
    private Transform leftHomeTransform = null;
    [SerializeField]
    private Transform rightHomeTransform = null;

    [Header("Step settings")]
    [SerializeField]
    private float stepAtDistance = 0.8f;
    [SerializeField]
    private float stepDuration = 0.5f;
    [SerializeField]
    private float stepOvershootFraction = 0.2f;

    [SerializeField]
    private bool leftIsMoving = false;
    [SerializeField]
    private bool rightIsMoving = false;

    // Start is called before the first frame update
    void Start()
    {
        if (anim == null)
        {
            anim = GetComponent<Animator>();

            leftFootAnchor = anim.GetBoneTransform(HumanBodyBones.LeftFoot);
            rightFootAchor = anim.GetBoneTransform(HumanBodyBones.RightFoot);

            leftCurrentPosition = leftFootAnchor.position;
            rightCurrentPosition = rightFootAchor.position;

            leftCurrentRotation = leftFootAnchor.rotation;
            rightCurrentRotation = rightFootAchor.rotation;

            StartCoroutine(MoveLeftToHome());
            StartCoroutine(MoveRightToHome());
        }

        audioManager = AudioManager.Instance;
    }

    void Update()
    {
        if (!leftIsMoving)
        {
            float distFromHome = Vector3.Distance(leftCurrentPosition, leftHomeTransform.position);

            if (distFromHome > stepAtDistance)
            {
                StartCoroutine(MoveLeftToHome());
            }
        }

        if (!rightIsMoving)
        {
            float distFromHome = Vector3.Distance(rightCurrentPosition, rightHomeTransform.position);

            if (distFromHome > stepAtDistance)
            {
                StartCoroutine(MoveRightToHome());
            }
        }
    }

    IEnumerator MoveLeftToHome()
    {
        if (rightIsMoving)
        {
            yield break;
        }
        leftIsMoving = true;

        Quaternion startRot = leftFootAnchor.rotation;
        Vector3 startPoint = leftFootAnchor.position;

        Quaternion endRot = transform.rotation;

        Vector3 towardHome = (leftHomeTransform.position - leftFootAnchor.position);
        float overshootDistance = stepAtDistance * stepOvershootFraction;
        Vector3 overshootVector = towardHome * overshootDistance;
        overshootVector = Vector3.ProjectOnPlane(overshootVector, Vector3.up);

        Vector3 endPoint = leftHomeTransform.position + overshootVector;

        Vector3 centerPoint = (startPoint + endPoint) / 2;
        // But also lift off, so we move it up by half the step distance (arbitrarily)
        centerPoint += leftHomeTransform.up * Vector3.Distance(startPoint, endPoint) / 2f;


        float elapsedTime = 0f;

        do
        {
            elapsedTime += Time.deltaTime;

            float normalizedTime = elapsedTime / stepDuration;

            leftCurrentPosition =
      Vector3.Lerp(
        Vector3.Lerp(startPoint, centerPoint, normalizedTime),
        Vector3.Lerp(centerPoint, leftHomeTransform.position + overshootVector, normalizedTime),
        normalizedTime
      );
            leftCurrentRotation = Quaternion.Slerp(startRot, endRot, normalizedTime);

            yield return null;
        }
        while (elapsedTime < stepDuration);

        leftCurrentPosition = leftFootAnchor.position;
        audioManager.PlayFootstepSoundAtLocation(rightFootAchor.position);
        leftIsMoving = false;
    }

    IEnumerator MoveRightToHome()
    {
        yield return null;
        if (leftIsMoving)
        {
            yield break;
        }
        rightIsMoving = true;

        Quaternion startRot = rightFootAchor.rotation;
        Vector3 startPoint = rightFootAchor.position;

        Quaternion endRot = transform.rotation;
        Vector3 towardHome = (rightHomeTransform.position - rightFootAchor.position);
        float overshootDistance = stepAtDistance * stepOvershootFraction;
        Vector3 overshootVector = towardHome * overshootDistance;
        overshootVector = Vector3.ProjectOnPlane(overshootVector, Vector3.up);

        Vector3 endPoint = rightHomeTransform.position + overshootVector;

        Vector3 centerPoint = (startPoint + endPoint) / 2;
        // But also lift off, so we move it up by half the step distance (arbitrarily)
        centerPoint += rightHomeTransform.up * Vector3.Distance(startPoint, endPoint) / 2f;


        float elapsedTime = 0f;

        do
        {
            elapsedTime += Time.deltaTime;

            float normalizedTime = elapsedTime / stepDuration;

            rightCurrentPosition =
      Vector3.Lerp(
        Vector3.Lerp(startPoint, centerPoint, normalizedTime),
        Vector3.Lerp(centerPoint, rightHomeTransform.position + overshootVector, normalizedTime),
        normalizedTime
      );
            rightCurrentRotation = Quaternion.Slerp(startRot, endRot, normalizedTime);

            yield return null;
        }
        while (elapsedTime < stepDuration);

        rightCurrentPosition = rightFootAchor.position;
        audioManager.PlayFootstepSoundAtLocation(rightFootAchor.position);
        rightIsMoving = false;
    }

    private void OnAnimatorIK(int layerIndex)
    {
        if (anim != null)
        {
            //Position Weight
            anim.SetIKPositionWeight(AvatarIKGoal.LeftFoot, 1f);
            anim.SetIKPositionWeight(AvatarIKGoal.RightFoot, 1f);

            //Rotation Weight
            anim.SetIKRotationWeight(AvatarIKGoal.LeftFoot, 1f);
            anim.SetIKRotationWeight(AvatarIKGoal.RightFoot, 1f);

            anim.SetIKPosition(AvatarIKGoal.LeftFoot, leftCurrentPosition);
            anim.SetIKPosition(AvatarIKGoal.RightFoot, rightCurrentPosition);

            anim.SetIKRotation(AvatarIKGoal.LeftFoot, leftCurrentRotation);
            anim.SetIKRotation(AvatarIKGoal.RightFoot, rightCurrentRotation);
        }
    }
}
