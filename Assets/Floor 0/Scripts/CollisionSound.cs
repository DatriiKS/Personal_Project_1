using System.Collections;
using UnityEngine;

public class CollisionSound : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;

    private Rigidbody m_rigidbody;

    private bool isAudioPlaying = false;
    void Start()
    {
        m_rigidbody = GetComponent<Rigidbody>();
    }

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("Played");
        if (isAudioPlaying == false)
        {
            audioSource.volume = m_rigidbody.velocity.magnitude * 2;
            audioSource.Play();
        }
        isAudioPlaying = !isAudioPlaying;
    }

    private void OnTriggerExit(Collider other)
    {
        isAudioPlaying = false;
    }
}
