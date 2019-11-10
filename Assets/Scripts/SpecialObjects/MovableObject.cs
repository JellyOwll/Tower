using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovableObject : MonoBehaviour
{
    protected Rigidbody rb;


    virtual protected void Start()
    {
        rb = GetComponent<Rigidbody>();
        Player.OnPick += Player_OnPick;
        Player.OnThrow += Player_OnThrow;
    }

    protected virtual void Player_OnThrow(Player sender)
    {
        if (sender.currentObject == transform)
        {
            GetComponent<Collider>().enabled = true;
            rb.isKinematic = false;
        }
    }

    protected virtual void Player_OnPick(Player sender)
    {
        if(sender.currentObject == transform)
        {
            rb.isKinematic = true;
            GetComponent<Collider>().enabled = false;
            transform.parent = null;

        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    virtual protected void OnDestroy()
    {
        Player.OnPick -= Player_OnPick;
        Player.OnThrow -= Player_OnThrow;

    }
}
