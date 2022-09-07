using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioRandomizer : MonoBehaviour
{
    [SerializeField] private List<AudioClip> audioClips;

    [SerializeField] private AudioSource RelatedAudioSource;

    private void Awake()
    {
        RelatedAudioSource.clip = audioClips[Random.Range(0, audioClips.Count)];

        RelatedAudioSource.Play();

        RelatedAudioSource.pitch = Random.Range(0.95f, 1.05f);
    }
}
