using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour, Interactable
{
    public string Name => "Открыть E";
    private Animator anim;
    private AudioSource source;

    public void Interact()
    {
        anim.Play("Open");
        source.Play();
        Destroy(this);  
    }

    void Start()
    {
        anim = GetComponent<Animator>();
        source = GetComponent<AudioSource>();
    }


}
