using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class Level : MonoBehaviour
{
    private int remainigBlocks=0;
    
    // Start is called before the first frame update
    void Start()
    {
        CountBlocks();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void CountBlocks(){
        Transform[] children=transform.GetComponentsInChildren<Transform>();
        remainigBlocks=children.Length-1;
        // Debug.Log((remainigBlocks));
    }

    public bool IsLevelCompleted()
    {
        if (remainigBlocks == 0)
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
        remainigBlocks--;
    }
}
