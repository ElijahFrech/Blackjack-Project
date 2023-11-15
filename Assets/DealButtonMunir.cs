using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DealButtonMunir : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] public CardManager cardManager; 

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnClick(){
        Debug.Log("Deal Button Clicked");
        cardManager.DealCards();
    }

    
    public void OnClick2(){
        Debug.Log("Deal Button Clicked");
        cardManager.DealerCards();
    }
}
