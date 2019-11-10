using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ladder : MonoBehaviour
{
    public delegate void LadderEventHandler(Ladder sender);
    static public event LadderEventHandler EnterLadder;
    static public event LadderEventHandler ExitLadder;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.GetComponent<Player>())
        {
            EnterLadder?.Invoke(this);

        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<Player>())
        {
            ExitLadder?.Invoke(this);
        }
    }
}
