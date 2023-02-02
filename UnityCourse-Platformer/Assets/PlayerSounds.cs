using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSounds : MonoBehaviour
{
    
    [SerializeField] private AudioClip _finishClip;
    private AudioSource _source;

    private void Start()
    {
        _source = GetComponent<AudioSource>();
    }

    public void FinishSound()
    {
        _source.PlayOneShot(_finishClip);
    }
    
}
