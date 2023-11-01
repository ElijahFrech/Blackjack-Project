using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class betUI : MonoBehaviour
{
    [SerializeField] private GameObject upButton;
    [SerializeField] private GameObject downButton;
    [SerializeField] private GameObject removeButton;
    [SerializeField] private GameObject betButton;


    int[] betArray = {5, 10, 20, 30, 40, 50, 60, 70, 80, 90, 100};

    int currentBetAmount = 5;
    int poolMoney = 500;
    
    // Start is called before the first frame update
    void Start()
    {
              
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void increaseBet()
    {

    }

    private void decreaseBet()
    {

    }

    private int CurrentBetAmount { get; set; }
    private int PoolMoney { get; set; }
}
