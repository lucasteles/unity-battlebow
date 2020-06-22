using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{

    [SerializeField] AudioClip hitCharacter;
    [SerializeField] AudioClip hitOther;
    [SerializeField] AudioClip shotArrow;
    [SerializeField] AudioClip die;
    [SerializeField] AudioClip jump;

    [SerializeField] AudioSource source;

    public static SoundManager Instance;
    
    void Awake() => Instance = this;

    void Play(AudioClip clip)
    {
        source.Stop();
        source.PlayOneShot(clip);
    }

    public void PlayHitChatacter() => Play(hitCharacter);
    
    public void PlayHitOther()=> Play(hitOther);

    public void PlayShot() => Play(shotArrow);
    
    public void PlayDie() => Play(die);
    public void PlayJump() => Play(jump);

}
