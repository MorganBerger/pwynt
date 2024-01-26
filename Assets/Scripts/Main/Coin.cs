using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    enum TossResult: int {
        Heads = 0,
        Tails = 1
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    TossResult Toss() {
        return TossResult.Tails;
    }
}
