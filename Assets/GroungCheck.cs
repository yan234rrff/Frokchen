using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroungCheck : MonoBehaviour
{
    public PlayerConroller player;
    private void OnTriggerEnter(Collider other)
    {
        player.onGround = true;
    }
}
