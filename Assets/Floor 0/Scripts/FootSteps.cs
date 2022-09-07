using System.Collections.Generic;
using UnityEngine;

public class FootSteps : MonoBehaviour
{
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip[] clips;

    private Vector3 positionLast;
    private Vector3 positionCurrent;
    
    void Start()
    {
        positionLast =  new Vector3(transform.position.x, 0, transform.position.z);
        positionCurrent = new Vector3(transform.position.x, 0, transform.position.z);
    }

    void Update()
    {
        positionCurrent.x = transform.position.x;
        positionCurrent.z = transform.position.z;
        positionCurrent.y = 0;

        //var difference = (positionLast - positionCurrent).magnitude;
        var difference = Vector3.Distance(positionLast, positionCurrent);


        if (difference >= 1 || difference <= -1)
        {
            audioSource.Stop();
            audioSource.clip = GetRandomClip();
            audioSource.pitch = Random.Range(0.97f, 1.03f);
            audioSource.Play();
            positionLast = positionCurrent;
        }
    }

    private AudioClip GetRandomClip()
    {
        return (AudioClip)clips.GetValue(Random.Range(0, 1));
    }
}
