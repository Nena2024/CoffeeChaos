using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class OrderManager : MonoBehaviour
{
    public Animator[] animators;
    public  TextMeshProUGUI text; 

    private string[] orders = { "OneCF","TwoCoffee","OneSugar","TwoSugar"};
   

    private int[] currentorderIndices=new int[2];
    

    private float gameTime = 120f; // two mins in seconds
   
    private bool gameRunning = true;
    

    void Start()
    {
        GenerateRandomOrder();
        StartCoroutine(GameTimer());
    }
    
    void GenerateRandomOrder()
    {
        for (int i=0; i < animators.Length; i++)
        {

            GenerateNewOrderForPlace(i);
            
        }

    }

    private void GenerateNewOrderForPlace(int placeIndex)
    {
        currentorderIndices[placeIndex] = Random.Range(0, orders.Length);
        string randomOrder = orders[currentorderIndices[placeIndex]];
        PlayerOrderAnimation(animators[placeIndex], randomOrder);

    }
    
    private void PlayerOrderAnimation(Animator animator, string orderType)
    {
        switch (orderType)
        {
            case "OneCF":
                animator.SetTrigger("OneCF");
                break;
            case "TwoCoffee":
                animator.SetTrigger("TwoCoffee");
                break;
            case "OneSugar":
                animator.SetTrigger("OneSugar");
                break;
            case "TwoSugar":
                animator.SetTrigger("TwoSugar");
                break;

        }
    }
    
    public void CompleteOrderAtPlace(int placeIndex)
    {
        GenerateNewOrderForPlace(placeIndex);
    }
   
    IEnumerator GameTimer()
    {
       

        while (gameTime > 0)
        {
            gameTime -= Time.deltaTime;
            int minutes = Mathf.FloorToInt(gameTime / 60);
            int seconds = Mathf.FloorToInt(gameTime % 60);
            string timeString = string.Format("{0:00}:{1:00}", minutes, seconds);
            yield return null;
            
             text.text = "Time: "+timeString.ToString();

        }
        gameRunning = false;
        Debug.Log("Time's up!");
    }


    void Update()
    {
        
    }
}
