using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class FootstepOverDistance : MonoBehaviour
{
    // NOTICE:
    //
    // If you think the script is broken, please first make sure that the player is NOT on a stepLayer
    [Header("Settings")]
    [SerializeField] private LayerMask stepLayers = 0;
    [SerializeField] private float groundedCheckDistance = .25f;
    [SerializeField] private float movedCheckDistance = 1f;

    [Header("Audio")]
    [SerializeField] private AudioSource audioSource = null;
    [SerializeField] private List<AudioClip> stepSounds = new List<AudioClip>();

    private Vector3 lastStepPosition = Vector3.zero;
    private float sqrCheckDistance = 1f;
    // Start is called before the first frame update
    void Start()
    {
        lastStepPosition = transform.position;
        sqrCheckDistance = movedCheckDistance * movedCheckDistance;
        if (audioSource == null) audioSource = GetComponent<AudioSource>();
    }

    /// <summary>
    /// Calls PLayFootstepSound after transform travelled a given distance
    /// </summary>
    void Update()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position + Vector3.up * 0.125f, Vector3.down, out hit, groundedCheckDistance + 0.125f, stepLayers))
        {
            if ((transform.position - lastStepPosition).sqrMagnitude > sqrCheckDistance)
            {
                AudioClip footstepSound = GetRandomStepSound();
                if (footstepSound != null) audioSource.PlayOneShot(footstepSound);
                lastStepPosition = transform.position;
            }
        }
    }

    public AudioClip GetRandomStepSound()
    {
        if (stepSounds.Count > 0)
        {
            AudioClip footstepSound = stepSounds[Random.Range(0, stepSounds.Count)];
            return footstepSound;
        }

        return null;
    }
}
