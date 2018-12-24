using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSourceManage :  GameInstance<AudioSourceManage> {

    public AudioSource audioSource;

	public void Init(AudioSource audioSource)
    {
        this.audioSource = audioSource;
    }
}
