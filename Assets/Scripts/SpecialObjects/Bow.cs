using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bow : Weapon
{

    [SerializeField]
    protected Transform arrowSpawn;
    [SerializeField]
    protected GameObject arrowPrefab;
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    protected override void Player_OnThrow(Player sender)
    {
        base.Player_OnThrow(sender);
        if (sender.currentObject == transform)
        {
            transform.parent = null;
        }
    }

    protected override void Player_OnAttack(Player sender)
    {
        base.Player_OnAttack(sender);
        if (sender.currentObject == transform)
        {
            Vector3 bulletVector;
            Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                bulletVector = hit.point - arrowSpawn.position;
                Debug.DrawRay(arrowSpawn.position, bulletVector, Color.red,10f);

            } else
            {
                bulletVector = ray.GetPoint(10f) - arrowSpawn.position;
            }
            GameObject arrow = Instantiate(arrowPrefab);
            arrow.transform.position = arrowSpawn.position;
            arrow.transform.rotation= arrowSpawn.rotation;
            arrow.GetComponent<Rigidbody>().AddForce(bulletVector * sender.Force * 10, ForceMode.Impulse);
            arrow.GetComponent<Bolt>().force = sender.Force;
            arrow.GetComponent<Bolt>().damage = damage * sender.Force;
        }
    }

    protected override void Player_OnPick(Player sender)
    {
        base.Player_OnPick(sender);
        if (sender.currentObject == transform)
        {
            Debug.Log("coucou");
            transform.parent = sender.bowHolder;
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.Euler(0, 0, 0);
        }
    }
}
