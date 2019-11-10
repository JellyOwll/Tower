using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WeaponState
{
    unlocked = 0,
    locked = 1,
}

public class Weapon : MovableObject
{
    [Header("State")]
    [SerializeField]
    protected WeaponState state;

    [Header("Parameters")]
    [SerializeField]
    protected float damage;
    [SerializeField]
    protected float price;

    

    // Start is called before the first frame update
    override protected void Start()
    {
        base.Start();
        Player.OnAttack += Player_OnAttack;
    }

    virtual protected void Player_OnAttack(Player sender)
    {
        if(sender.currentObject == transform)
        {

        }
    }

    protected override void Player_OnThrow(Player sender)
    {
        base.Player_OnThrow(sender);
        if (sender.currentObject == transform)
        {
            sender.hasWeapon = false;
            transform.parent = null;
        }
    }

    protected override void Player_OnPick(Player sender)
    {
        base.Player_OnPick(sender);
        if (sender.currentObject == transform)
        {
            sender.hasWeapon = true;
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.Euler(0, 0, 0);
        }
    }

    override protected void OnDestroy()
    {
        base.OnDestroy();
        Player.OnAttack -= Player_OnAttack;
    }
}
