using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : Weapon
{
    [Header("Collider")]
    [SerializeField]
    protected Collider swordCollider;
    [SerializeField]
    protected Collider swordTrigger;

    override protected void Start()
    {
        base.Start();
        swordCollider.enabled = false;
        swordTrigger.enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    protected void OnTriggerEnter(Collider other)
    {
        StuckOnObject(other.transform);
        
    }

    private void StuckOnObject(Transform otherObject)
    {
        transform.parent = otherObject;
        rb.isKinematic = true;
        //swordCollider.enabled = true;
        swordTrigger.enabled = false;
    }

    protected override void Player_OnThrow(Player sender)
    {
        base.Player_OnThrow(sender);
        if (sender.currentObject == transform)
        {
            swordCollider.enabled = false;
            swordTrigger.enabled = true;
        }
    }

    protected override void Player_OnAttack(Player sender)
    {
        base.Player_OnAttack(sender);
        if (sender.currentObject == transform)
        {
            Debug.Log("Sword");
        }
    }

    protected override void Player_OnPick(Player sender)
    {
        base.Player_OnPick(sender);
        if (sender.currentObject == transform)
        {
            transform.parent = sender.weaponHolder;
            swordCollider.enabled = true;
            swordTrigger.enabled = false;
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.Euler(0, 0, 0);
        }
    }
}
