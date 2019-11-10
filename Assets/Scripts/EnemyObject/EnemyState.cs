using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum State
{
    patrol = 0,
    chase = 1,
    attack = 2,
    search = 3,
}

abstract public class EnemyState : MonoBehaviour
{
    [SerializeField]
    protected State _enemyState;
    protected Action DoAction;


    public State enemyState 
    { 
        get => _enemyState;
        set
        {
            _enemyState = value;
            CheckState();
        }
    }

    virtual protected void CheckState()
    {
        if(enemyState == State.patrol)
        {
            DoAction = DoActionPatrol;
        } else if(enemyState == State.chase)
        {
            DoAction = DoActionChase;
        } else if(enemyState == State.search)
        {
            DoAction = DoActionSearch;
        }
    }

    // Start is called before the first frame update
    protected virtual void Start()
    {
        DoAction = DoActionVoid;
        enemyState = State.patrol;
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        DoAction();
    }

    protected void DoActionVoid()
    {

    }

    protected abstract void DoActionChase();
    protected abstract void DoActionSearch();

    protected abstract void DoActionPatrol();
}
