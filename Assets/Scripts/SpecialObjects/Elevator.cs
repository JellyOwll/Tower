using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ElevatorType
{
    ABA = 0,
    AB = 1,
    BA = 2,
}

public class Elevator : MonoBehaviour
{

    [SerializeField]
    protected Transform pointA;
    [SerializeField]
    protected Transform pointB;
    [SerializeField]
    protected ElevatorType type;
    protected Rigidbody rb;
    [SerializeField]
    protected float speed;
    protected Action DoAction;
    void Start()
    {
        DoAction = DoActionVoid;
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        DoAction();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Player>())
        {
            if (Vector3.Distance(transform.position, pointA.position) <= 1)
            {
                if(type == ElevatorType.ABA || type == ElevatorType.AB)
                {
                    DoAction = GoToB;
                }
            } else if(Vector3.Distance(transform.position,pointB.position) <= 1)
            {
                if (type == ElevatorType.ABA || type == ElevatorType.BA)
                {
                    DoAction = GoToA;
                }
            }
        }
        
    }

    protected void GoToA()
    {
        transform.position = Vector3.Lerp(transform.position, pointA.position, speed * Time.deltaTime);
        if(transform.position == pointA.position)
        {
            DoAction = DoActionVoid;
        }
    }

    protected void GoToB()
    {

        transform.position = Vector3.Lerp(transform.position, pointB.position, speed * Time.deltaTime);
        if (transform.position == pointB.position)
        {
            DoAction = DoActionVoid;
        }
    }

    protected void DoActionVoid()
    {

    }
}
