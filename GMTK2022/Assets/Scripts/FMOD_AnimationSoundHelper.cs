using System.Collections;
using System.Collections.Generic;
using FMODUnity;
using UnityEngine;

public class FMOD_AnimationSoundHelper : MonoBehaviour {
    public string soundToPlay = "";
    public StudioEventEmitter eventEmitter1;
    public StudioEventEmitter eventEmitter2;
    public StudioEventEmitter eventEmitter3;
    // Start is called before the first frame update

    public void PlaySound1 () {
        if (eventEmitter1 == null) {
            //AudioManager.instance.PlaySFX (soundToPlay);
        } else {
            eventEmitter1.Play ();
        }
    }
    public void PlaySound2 () {
        if (eventEmitter2 == null) {
            //AudioManager.instance.PlaySFX (soundToPlay);
        } else {
            eventEmitter2.Play ();
        }
    }
    public void PlaySound3 () {
        if (eventEmitter3 == null) {
            //AudioManager.instance.PlaySFX (soundToPlay);
        } else {
            eventEmitter3.Play ();
        }
    }
}