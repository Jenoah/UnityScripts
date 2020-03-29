using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance = null;
    private AudioSource audioSource = null;

    [Header("Audioclips")]
    [SerializeField]
    private AudioClip clip = null;

    [SerializeField]
    private AudioClip[] footsteps = null;

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayClip()
    {
        audioSource.PlayOneShot(clip);
    }

    public void PlayFootstepSound()
    {
        audioSource.PlayOneShot(GetRandomFootstepSound());
    }

    public void PlayFootstepSoundAtLocation(Vector3 position)
    {
        AudioSource.PlayClipAtPoint(GetRandomFootstepSound(), position);
    }

    private AudioClip GetRandomFootstepSound()
    {
        if(footsteps.Length > 0)
        {
            int index = Random.Range(0, footsteps.Length);
            return footsteps[index];
        }

        return null;
    }
}
