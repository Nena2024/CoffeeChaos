using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class OrderManager : MonoBehaviour
{
    public Animator[] animators;
    public Animator cupAimator;
    public TextMeshProUGUI text;

    private int sugarCount;
    private int coffeeCount;

    public GameObject emptyCupImage, oneSugarImage, twoSugarImage, oneCoffeeImage, twoCoffeeImage;
    public GameObject oneSugarAnim, twoSugarAnim, oneCoffeeAnim, TwoCoffeeAnim;
    public Button startCoffeeMakerBtnLeft, startCoffeeMakerBtnRight, sugarButton, coffeeButton;



    private string[] orders = { "OneCF", "TwoCoffee", "OneSugar", "TwoSugar" };


    private int[] currentorderIndices = new int[2];
    private int[] playerSelections = new int[2];


    private float gameTime = 120f; // two mins in seconds

    private bool gameRunning = true;


    void Start()
    {
        GenerateRandomOrder();
        StartCoroutine(GameTimer());

    }

    void GenerateRandomOrder()
    {
        for (int i = 0; i < animators.Length; i++)
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

    public void AddSugar()
    {
        if (sugarCount < 2)
        {
            sugarCount++;
            coffeeCount = 0;
            UpdateCupDisplay();
        }
    }
    public void AddCoffee()
    {
        if (coffeeCount < 2)
        {
            coffeeCount++;
            sugarCount = 0;
            UpdateCupDisplay();
        }
    }
    private void UpdateCupDisplay()
    {
        startCoffeeMakerBtnLeft.gameObject.SetActive(true);

        emptyCupImage.SetActive(false);
        oneSugarImage.SetActive(false);
        twoSugarImage.SetActive(false);
        oneCoffeeImage.SetActive(false);
        twoCoffeeImage.SetActive(false);

        if (sugarCount == 1)
        {
            oneSugarImage.SetActive(true);
        }
        else if (sugarCount == 2)
        {
            twoSugarImage.SetActive(true);
        }
        else if (coffeeCount == 1)
        {
            oneCoffeeImage.SetActive(true);
        }
        else if (coffeeCount == 2)
            twoCoffeeImage.SetActive(true);
    }
    public void StartOrder()
    {
        cupAimator.ResetTrigger("SmallSugar");
        cupAimator.ResetTrigger("LargeSugar");
        cupAimator.ResetTrigger("SmallCoffee");
        cupAimator.ResetTrigger("LargeCoffee");


        /*  switch (sugarCount)
          {
              case 1:
                  animators[2].SetTrigger("SmallSugar");
                  break;
              case 2:
                  animators[2].SetTrigger("LargeSugar");
                  break;

          }
          switch (coffeeCount)
          {
              case 1:
                  animators[2].SetTrigger("SmallCoffee");
                  break;
              case 2:
                  animators[2].SetTrigger("LargeCoffee");
                  break;

          }*/



        if (sugarCount == 1)
        {
            Debug.Log("Triggering one Sugar");
            oneSugarImage.SetActive(false);
            cupAimator.SetTrigger("SmallSugar");



        }
        else if (sugarCount == 2)
        {
            Debug.Log("Triggering Two Sugar");
            twoSugarImage.SetActive(false);


            cupAimator.SetTrigger("LargeSugar");



        }
        else if (coffeeCount == 1)

        {
            Debug.Log("Triggering one Cofffee");
            oneCoffeeImage.SetActive(false);


            cupAimator.SetTrigger("SmallCoffee");



        }
        else if (coffeeCount == 2)
        {
            Debug.Log("Triggering Two Coffee");
            twoCoffeeImage.SetActive(false);


            cupAimator.SetTrigger("LargeCoffee");



        }
        ResetOrder();
    }

    public void ResetOrder()
    {
        sugarCount = 0;
        coffeeCount = 0;
        UpdateCupDisplay();
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

            text.text = "Time: " + timeString.ToString();

        }
        gameRunning = false;
        Debug.Log("Time's up!");
    }


    /* moshkel injast ke baraye animation ghesmati ke bayad livan por beshe anjam nemishe . error nadare  , bishtar bekhatere ineke avalin trigeri ke 
     ba taavajoh be animation e avali ke entekhab mikonam miad , ma baghi ejra nemishe , dafe bad check kon bebin moshkel kojas */
    void Update()
    {

    }
}
