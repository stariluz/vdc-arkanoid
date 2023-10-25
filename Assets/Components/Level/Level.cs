using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
    private int remainnigBlocks=0;
    // Start is called before the first frame update
    void Start()
    {
        remainnigBlocks = transform.childCount;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool IsLevelCompleted()
    {
        if (remainnigBlocks == 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public void BlockDestroyed()
    {
        remainnigBlocks--;
    }
}
