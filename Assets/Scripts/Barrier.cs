using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barrier : MonoBehaviour
{
    public AudioClip HitAudio;

    public void PlayAudio() {
        AudioSource.PlayClipAtPoint(HitAudio, transform.position);
    }
}
