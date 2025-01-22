using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class OrderManager : MonoBehaviour
{




    bool isServe = false;
    bool serveFinished = false;
    public Animator Serve;
    public GameObject ServeAnimate;

    bool isFilling = false;
    public Animator cupAimator;
    public GameObject oneSugarAnim, twoSugarAnim, oneCoffeeAnim, TwoCoffeeAnim;

    public TextMeshProUGUI text;
    public TextMeshProUGUI scoreText;

    private int sugarCount;
    private int coffeeCount;
    private int score = 0;

    public GameObject emptyCupImage, oneSugarImage, twoSugarImage, oneCoffeeImage, twoCoffeeImage;
    public Button startCoffeeMakerBtnLeft, startCoffeeMakerBtnRight, sugarButton, coffeeButton, serveButton;

    private string randomOrder; 
    public Animator[] animators;
    private string[] orders = { "OneCF", "TwoCoffee", "OneSugar", "TwoSugar" };


    private int[] currentorderIndices = new int[1];
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

    public  void GenerateNewOrderForPlace(int placeIndex)
    {
        currentorderIndices[placeIndex] = Random.Range(0, orders.Length);
        randomOrder = orders[currentorderIndices[placeIndex]];
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
        Debug.Log(orderType);
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

        oneSugarAnim.gameObject.SetActive(true);


        if (sugarCount == 1)
        {
            cupAimator.SetTrigger("SmallSugar");
            //  oneSugarAnim.gameObject.SetActive(true);
            Debug.Log("Triggering one Sugar");
            oneSugarImage.SetActive(false);

            AnimatorStateInfo stateInfo = cupAimator.GetCurrentAnimatorStateInfo(0);
            Debug.Log($"Current State: { stateInfo.fullPathHash}");
            isFilling = true;


        }
        else if (sugarCount == 2)
        {
            cupAimator.SetTrigger("LargeSugar");
            //  twoSugarAnim.gameObject.SetActive(true);
            Debug.Log("Triggering Two Sugar");
            twoSugarImage.SetActive(false);

            AnimatorStateInfo stateInfo = cupAimator.GetCurrentAnimatorStateInfo(0);
            Debug.Log($"Current State: { stateInfo.fullPathHash}");
            isFilling = true;

        }
        else if (coffeeCount == 1)

        {
            cupAimator.SetTrigger("SmallCoffee");

            //  oneCoffeeAnim.gameObject.SetActive(true);
            Debug.Log("Triggering one Cofffee");
            oneCoffeeImage.SetActive(false);

            AnimatorStateInfo stateInfo = cupAimator.GetCurrentAnimatorStateInfo(0);
            Debug.Log($"Current State: { stateInfo.fullPathHash}");
            isFilling = true;
        }
        else if (coffeeCount == 2)
        {
            cupAimator.SetTrigger("LargeCoffee");
            // TwoCoffeeAnim.gameObject.SetActive(true);
            Debug.Log("Triggering Two Coffee");
            twoCoffeeImage.SetActive(false);

            AnimatorStateInfo stateInfo = cupAimator.GetCurrentAnimatorStateInfo(0);
            Debug.Log($"Current State: { stateInfo.fullPathHash}");
            isFilling = true;

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
    public void serveCup()
    {
        isServe = true;
        emptyCupImage.gameObject.SetActive(false);
        oneSugarImage.gameObject.SetActive(false);
        twoSugarImage.gameObject.SetActive(false);
        oneCoffeeImage.gameObject.SetActive(false);
        twoCoffeeImage.gameObject.SetActive(false);
        oneSugarAnim.SetActive(false);
        twoSugarAnim.SetActive(false);
        oneCoffeeAnim.SetActive(false);
        TwoCoffeeAnim.SetActive(false);
        ServeAnimate.SetActive(true);
        Serve.Play("Serve", -1, 0f);
        Debug.Log("cup is being served");
    }
    public void IsOrderMatched(string doneOrder)
    {
        if (doneOrder==randomOrder)
        {
            score += 500;
            scoreText.text = "Score: " + score;
            Debug.Log(scoreText.text);
        }
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
        if (isFilling && cupAimator.GetCurrentAnimatorStateInfo(0).IsName("SmallSugar") && cupAimator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
        {

            Debug.Log("small sugar cup filling is stopped!");
            startCoffeeMakerBtnLeft.gameObject.SetActive(false);
            serveButton.gameObject.SetActive(true);
            IsOrderMatched("OneSugar");
            
            isFilling = false;
           

        }
        else if (isFilling && cupAimator.GetCurrentAnimatorStateInfo(0).IsName("LargeSugar") && cupAimator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
        {
            Debug.Log("large sugar cup filling is stopped!");
            startCoffeeMakerBtnLeft.gameObject.SetActive(false);
            serveButton.gameObject.SetActive(true);
            IsOrderMatched("TwoSugar");
            isFilling = false;
           
        }
        else if (isFilling && cupAimator.GetCurrentAnimatorStateInfo(0).IsName("SmallCoffee") && cupAimator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
        {
            Debug.Log("small coffee cup filling is stopped!");
            startCoffeeMakerBtnLeft.gameObject.SetActive(false);
            serveButton.gameObject.SetActive(true);
            IsOrderMatched("OneCF");
            isFilling = false;
           

        }
        else if (isFilling && cupAimator.GetCurrentAnimatorStateInfo(0).IsName("LargeCoffee") && cupAimator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
        {
            Debug.Log("Large coffee cup filling is stopped!");
            startCoffeeMakerBtnLeft.gameObject.SetActive(false);
            serveButton.gameObject.SetActive(true);
            IsOrderMatched("TwoCoffee");
            isFilling = false;
            
        }

        if (isServe && Serve.GetCurrentAnimatorStateInfo(0).IsName("Serve") && Serve.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
        {
            Debug.Log("Serve finished");
            serveButton.gameObject.SetActive(false);
            GenerateNewOrderForPlace(0);
            ServeAnimate.SetActive(false);
            serveFinished = true;
            isServe = false;


        }
    }
}