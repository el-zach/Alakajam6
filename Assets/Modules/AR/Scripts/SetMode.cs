using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetMode : MonoBehaviour
{
    public Transform guitarPos;
    public Transform pianoPos;
    public AudioSource guitarSource, pianoSource;

    public MoveToTarget mover;

    [ContextMenu("Set Piano")]
    void SetPiano()
    {
        transform.position = pianoPos.position;
        transform.rotation = pianoPos.rotation;

        if(mover)
        mover.allowMovement = false;

        guitarSource.Stop();
        pianoSource.Play();
    }

    [ContextMenu("Set Guitar")]
    void SetGuitar()
    {
        transform.position = guitarPos.position;
        transform.rotation = guitarPos.rotation;

        if (mover) mover.allowMovement = false;

        guitarSource.Play();
        pianoSource.Stop();
    }

    [ContextMenu("Set Free")]
    void SetFree()
    {
        if (mover) mover.allowMovement = true;

        guitarSource.Stop();
        pianoSource.Stop();
    }
}
