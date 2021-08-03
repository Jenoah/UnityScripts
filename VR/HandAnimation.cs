using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// For more help, watch: https://www.youtube.com/watch?v=DxKWq7z4Xao


public class HandAnimation : MonoBehaviour
{
    [SerializeField] private Animator animator = null;
    [SerializeField] private string triggerAnimationName = "Trigger";
    [SerializeField] private string gripAnimationName = "Grip";

    private float currentTriggerValue = 0f;
    private float currentGripValue = 0f;
    private float targetTriggerValue = 0f;
    private float targetGripValue = 0f;

    void Start()
    {
        if (animator == null) animator = GetComponent<Animator>();
    }

    
    void Update()
    {
        AnimateHands();
    }

    public void SetGripValue(float gripValue)
    {
        targetGripValue = gripValue;
    }

    public void SetTriggerValue(float triggerValue)
    {
        targetTriggerValue = triggerValue;
    }

    private void AnimateHands()
    {
        if(currentTriggerValue != targetTriggerValue)
        {
            currentTriggerValue = targetTriggerValue;
            animator.SetFloat(triggerAnimationName, currentTriggerValue);
        }

        if(currentGripValue != targetGripValue)
        {
            currentGripValue = targetGripValue;
            animator.SetFloat(gripAnimationName, currentGripValue);
        }
    }
}
