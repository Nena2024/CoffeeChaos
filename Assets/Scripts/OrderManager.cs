using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class OrderManager : MonoBehaviour
{


    bool isServe = false;
    bool isServeRightside = false;
    bool isFilling = false;
    bool isFillingRightside = false;
    bool serveFinished = false;
    bool serveRightsideFinished = false;
    bool firstOrder = false;
    bool secondOrder = false;
    bool orderOneComplete = false;
    bool orderTwoComplete = false;

    bool isPaused = false;
    bool firstPlaceIsEmpty = true;
    bool secondPlaceIsEmpty = false;
    bool leftImagesActive = false;
    bool rightImagesActive = false;
    bool sugarAlreadyAdded = false;
    bool coffeeAlreadyAdded = false;
    bool SugarOnRightAlreadyAdded = false;
    bool CoffeeOnRightAlreadyAdded = false;
    bool serveOneOrTwo = false;
    bool timeUpMessageShowed = false;
    bool lastMessageDone = false;
    

    bool leftCupCompleted = true;
    bool rightCupCompleted = true;
    bool isLeftAcvtive = false;
    bool isRightAcvtive = false;
    bool SugarLock = false;
    bool CoffeeLock = false;
    bool SugarLockRight = false;
    bool CoffeeLockRight = false;
    bool ingerdientForbidden = false;
    bool ingerdientForbiddenScnd = false;
    bool alreadyHasOrderOne = false;
    bool alreadyHasOrderTwo = false;


    bool orderComplete = false;

    public static OrderManager Instance;

    public Animator Serve;
    public GameObject ServeAnimate;

    public Animator serveRightside;
    public GameObject serveAnimateRightside;

    public Animator TimeUp;
    public GameObject timeUpAnimate;

    public Animator ShowScore;
    public GameObject showScoreAnimate; 


    public Animator cupAimator;
    public GameObject oneSugarAnim, twoSugarAnim, oneCoffeeAnim, TwoCoffeeAnim;

    public Animator cupAnimatorRight;
    public GameObject oneSugarAnimRight, twoSugarAnimRigh, oneCoffeeAnimRight, TwoCoffeeAnimRight;

    public TextMeshProUGUI text;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI totalScore;

    private int sugarCount = 0;
    private int coffeeCount = 0;
    private int sugarCountSnd = 0;
    private int coffeeCountSnd = 0;
    private int startPoint = 1;
    private int score = 0;
    private int saveSocre;

    private Coroutine timerCoroutine;

    public GameObject emptyCupImage, oneSugarImage, twoSugarImage, oneCoffeeImage, twoCoffeeImage;
    public GameObject emptyCupImageR, oneSugarImageR, twoSugarImageR, oneCoffeeImageR, twoCoffeeImageR;
    private GameObject selectedCup;
    public Button startCoffeeMakerBtnLeft, startCoffeeMakerBtnRight, sugarButton, coffeeButton, serveButton, serveButtonRightside;

    public Animator firstOrderAnim;
    public Animator secondOrderAnim;

    private string[] orders = { "OneCF", "TwoCoffee", "OneSugar", "TwoSugar" };
    private string[] ordersSecond = { "OneCF", "TwoCoffee", "OneSugar", "TwoSugar" };


    private int[] currentorderIndices = new int[2];
    public GameObject[] orderPlaces;
    private int[] playerSelections = new int[2];

    string randomOrder;
    string randomOrderTwo;


    private float gameTime = 5f; // two mins in seconds
    private float orderDelay = 1f; // 1 seconds for second order;
    private float nextCupApear = 5f;
    private float saveTime;
    private float timeLeft;
    private int currentOrderIndex = 0;
    private bool gameRunning = true;

   // alan jae ke kar nemikone bade animation e akhar stop 4 sanie kar nemikone   

    void Start()
    {
       
        StartCoroutine(GameTimer());
        StartCoroutine(ShowOrders());
       

    }
    IEnumerator WaitSeconds()// in taze ezafe shod 

    {
        Debug.Log("came to wait");
        yield return new WaitForSeconds(3f);
        timeUpMessageShowed = true;

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
            timeUp();
            
           
        
        Debug.Log("Time's up!");
        
    }
    void Update()
    {
        if (timeLeft > 0)
        {
            timeLeft -= Time.deltaTime;

            int minutes = Mathf.FloorToInt(timeLeft / 60);
            int seconds = Mathf.FloorToInt(timeLeft % 60);
            string timeString = string.Format("{0:00}:{1:00}", minutes, seconds);
            text.text = "Time: " + timeString.ToString();

        }

        if (Input.GetMouseButtonDown(0))

        {

            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);


            RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero);
            if (hit.collider != null)
            {
                Debug.Log("clicked");
                if (hit.collider.CompareTag("CupLeft"))
                {
                    Debug.Log("click on left ");
                    isLeftAcvtive = true;
                    isRightAcvtive = false;
                    Debug.Log("left is active");

                }
                else if (hit.collider.CompareTag("CupRight"))
                {
                    Debug.Log("click on right");

                    Debug.Log("right is active");
                    isRightAcvtive = true;
                    isLeftAcvtive = false;

                }
            }


        }
        if (lastMessageDone && ShowScore.GetCurrentAnimatorStateInfo(0).IsName("ShowScore") && ShowScore.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
        {
            ShowFinalScore();

        }

        if ( timeUpMessageShowed && TimeUp.GetCurrentAnimatorStateInfo(0).IsName("TimeUp") && TimeUp.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
        {
            timeUpMessageShowed = false;
            LastMessage();
        
        }

        if (isFilling && cupAimator.GetCurrentAnimatorStateInfo(0).IsName("SmallSugar") && cupAimator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
        {

            Debug.Log("small sugar cup filling is stopped!");
            startCoffeeMakerBtnLeft.gameObject.SetActive(false);
            serveButton.gameObject.SetActive(true);
            isFilling = false;


        }
        else if (isFilling && cupAimator.GetCurrentAnimatorStateInfo(0).IsName("LargeSugar") && cupAimator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
        {
            Debug.Log("large sugar cup filling is stopped!");
            startCoffeeMakerBtnLeft.gameObject.SetActive(false);
            serveButton.gameObject.SetActive(true);
            isFilling = false;

        }
        else if (isFilling && cupAimator.GetCurrentAnimatorStateInfo(0).IsName("SmallCoffee") && cupAimator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
        {
            Debug.Log("small coffee cup filling is stopped!");
            startCoffeeMakerBtnLeft.gameObject.SetActive(false);
            serveButton.gameObject.SetActive(true);
            isFilling = false;


        }
        else if (isFilling && cupAimator.GetCurrentAnimatorStateInfo(0).IsName("LargeCoffee") && cupAimator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
        {
            Debug.Log("Large coffee cup filling is stopped!");
            startCoffeeMakerBtnLeft.gameObject.SetActive(false);
            serveButton.gameObject.SetActive(true);
            isFilling = false;

        }

        if (isServe && Serve.GetCurrentAnimatorStateInfo(0).IsName("Serve") && Serve.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
        {
            Debug.Log("Serve finished");
            serveButton.gameObject.SetActive(false);
            whichPlaceIsDone();
            ServeAnimate.SetActive(false);
            serveFinished = true;
            isServe = false;
            firstPlaceIsEmpty = true;


        }



        if (isFillingRightside && cupAnimatorRight.GetCurrentAnimatorStateInfo(0).IsName("SmallSugar") && cupAnimatorRight.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
        {

            Debug.Log("filling of small sugar cup on the right side  is stopped!");
            startCoffeeMakerBtnRight.gameObject.SetActive(false);
            serveButtonRightside.gameObject.SetActive(true);
            isFillingRightside = false;
            IsOrderMatchedSecond("SmallSugarR");

        }
        else if (isFillingRightside && cupAnimatorRight.GetCurrentAnimatorStateInfo(0).IsName("LargeSugar") && cupAnimatorRight.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
        {
            Debug.Log("filling of large sugar cup on the right side is stopped!");
            startCoffeeMakerBtnRight.gameObject.SetActive(false);
            serveButtonRightside.gameObject.SetActive(true);
            isFillingRightside = false;
            IsOrderMatchedSecond("LargeSugarR");
        }
        else if (isFillingRightside && cupAnimatorRight.GetCurrentAnimatorStateInfo(0).IsName("SmallCoffee") && cupAnimatorRight.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
        {
            Debug.Log("filling of small coffee cup on the right side is stopped!");
            startCoffeeMakerBtnRight.gameObject.SetActive(false);
            serveButtonRightside.gameObject.SetActive(true);
            isFillingRightside = false;
            IsOrderMatchedSecond("SmallCoffeeR");

        }
        else if (isFillingRightside && cupAnimatorRight.GetCurrentAnimatorStateInfo(0).IsName("LargeCoffee") && cupAnimatorRight.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
        {
            Debug.Log("filling of Large coffee cup on the right side is stopped!");
            startCoffeeMakerBtnRight.gameObject.SetActive(false);
            serveButtonRightside.gameObject.SetActive(true);
            isFillingRightside = false;
            IsOrderMatchedSecond("LargeCoffeeR");
        }

        if (isServeRightside && serveRightside.GetCurrentAnimatorStateInfo(0).IsName("Serve") && serveRightside.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
        {
            Debug.Log("Serve on the Rightside's finished");
            serveButtonRightside.gameObject.SetActive(false);

            whichPlaceIsDone();
            serveAnimateRightside.SetActive(false);
            serveRightsideFinished = true;
            isServeRightside = false;
            secondPlaceIsEmpty = true;

        }
    }

    IEnumerator ShowOrders()
    {
        while (true)
        {
            if (!alreadyHasOrderOne)
            {
                Debug.Log("first order is " + firstOrder + "alreadyhasorder" + alreadyHasOrderOne);
                GenerateRandomOrderForPlaceOne();
                yield return new WaitUntil(() => orderOneComplete);
                yield return new WaitForSeconds(orderDelay);
            }
             if (!alreadyHasOrderTwo)
            {
                Debug.Log("second order is  " + secondOrder + "already has order two " + alreadyHasOrderTwo);
                secondOrderAnim.gameObject.SetActive(true);
                GenerateRandomOrderForPlaceTwo();
                yield return new WaitUntil(() => orderTwoComplete);
                yield return new WaitForSeconds(orderDelay);
            }


        }

    }
    public void timeUp()
    {
        timeUpAnimate.SetActive(true);
        TimeUp.Play("TimeUp");
        StartCoroutine(WaitSeconds());
        
        
       
          
    }
   public void LastMessage()
    {
        timeUpAnimate.SetActive(false);
        showScoreAnimate.gameObject.SetActive(true);
        ShowScore.Play("ShowScore");
        lastMessageDone = true;
        
    }
    public void ShowFinalScore()
    {
        lastMessageDone = false;
        StartCoroutine(WaitSeconds());
        totalScore.text = score.ToString();
    }
    public void GenerateRandomOrderForPlaceOne()
    {
        int placeIndex = Random.Range(0, orders.Length);
        randomOrder = orders[placeIndex];
        Debug.Log("randomOrder is " + randomOrder);
        PlayerOrderAnimation(firstOrderAnim, randomOrder);
        orderOneComplete = false;
        firstOrder = true;

    }
    public void GenerateRandomOrderForPlaceTwo()
    {
        int placeIndex = Random.Range(0, ordersSecond.Length);
        randomOrderTwo = ordersSecond[placeIndex];
        Debug.Log("randomOrderSecond is " + randomOrderTwo);
        PlayerOrderAnimationSecond(firstOrderAnim, randomOrderTwo);
        orderTwoComplete = false;
        secondOrder = true;

    }

    private void PlayerOrderAnimation(Animator animator, string orderType)
    {
        //secondOrderAnim.gameObject.SetActive(true);

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

    private void PlayerOrderAnimationSecond(Animator animator, string orderType)
    {

        switch (orderType)
        {
            case "OneCF":
                secondOrderAnim.SetTrigger("OneCoffeeR");
                break;
            case "TwoCoffee":
                secondOrderAnim.SetTrigger("TwoCoffeeR");
                break;
            case "OneSugar":
                secondOrderAnim.SetTrigger("OneSugarR");
                break;
            case "TwoSugar":
                secondOrderAnim.SetTrigger("TwoSugarR");
                break;

        }
    }
    public void CompleteOrder(string orderNumber)
    {
        if (orderNumber == "One")
        {
            orderOneComplete = true;
            Debug.Log("Order one completed!");

        }
        else if (orderNumber == "Two")
        {
            orderTwoComplete = true;
            Debug.Log("Order Two completed!");
        }

    }



    public void AddSugar()
    {
        if (isLeftAcvtive)
        {
            Debug.Log("showing if the left side can be used" + isLeftAcvtive);
            if (!ingerdientForbidden)
            {
                Debug.Log("showing if the left side ingerdient can be used" + ingerdientForbidden);

                if (sugarCount < 2)
                {

                    sugarCount++;
                    coffeeCount = 0;
                    Debug.Log("sugar added to first place");
                    UpdateCupDisplay();
                }
            }


        }

        if (isRightAcvtive)
        {
            Debug.Log("showing if the right side can be used" + isRightAcvtive);
            if (!ingerdientForbiddenScnd)
            {
                Debug.Log("showing if the right side ingerdient can be used" + ingerdientForbiddenScnd);

                if (sugarCountSnd < 2)
                {

                    sugarCountSnd++;
                    coffeeCountSnd = 0;
                    Debug.Log("sugar added to second place ");
                    UpdateCupDisplayScnd();
                }
            }


        }

    }

    public void AddCoffee()
    {
        if (isLeftAcvtive)
        {
            Debug.Log("showing if the left side can be used" + isLeftAcvtive);
            if (!ingerdientForbidden)
            {
                Debug.Log("showing if the left side ingerdient can be used" + ingerdientForbidden);
                if (coffeeCount < 2)
                {

                    coffeeCount++;
                    sugarCount = 0;
                    Debug.Log("coffee added to first place");
                    UpdateCupDisplay();

                }
            }

        }
        if (isRightAcvtive)
        {
            Debug.Log("showing if the right side can be used" + isRightAcvtive);
            if (!ingerdientForbiddenScnd)
            {
                Debug.Log("showing if the right side ingerdient can be used" + ingerdientForbiddenScnd);
                if (coffeeCountSnd < 2)
                {

                    coffeeCountSnd++;
                    sugarCountSnd = 0;
                    Debug.Log("coffee added to second place");
                    UpdateCupDisplayScnd();

                }
            }


        }


    }
    public void whichPlaceIsDone()
    {
        if(isServe)
        {
            GenerateRandomOrderForPlaceOne();
        }
        else if (isServeRightside)
        {
            GenerateRandomOrderForPlaceTwo();
        }
    }
    public bool LeftOrRightPlace()
    {
        if (isLeftAcvtive)

            return true;

        else
            return false;
    }


    private void UpdateCupDisplay()

    {
        alreadyHasOrderOne = false;

        if (isLeftAcvtive)
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
            {
                twoCoffeeImage.SetActive(true);

            }

        }
    }
    private void UpdateCupDisplayScnd()
    {
        alreadyHasOrderTwo = false;

        if (isRightAcvtive)
        {
            startCoffeeMakerBtnRight.gameObject.SetActive(true);

            emptyCupImageR.SetActive(false);
            oneSugarImageR.SetActive(false);
            twoSugarImageR.SetActive(false);
            oneCoffeeImageR.SetActive(false);
            twoCoffeeImageR.SetActive(false);

            if (sugarCountSnd == 1)
            {
                oneSugarImageR.SetActive(true);
                Debug.Log("images on the right ");

            }
            else if (sugarCountSnd == 2)
            {
                twoSugarImageR.SetActive(true);
                Debug.Log("images on the right ");

            }
            else if (coffeeCountSnd == 1)
            {
                oneCoffeeImageR.SetActive(true);
                Debug.Log("images on the right ");

            }
            else if (coffeeCountSnd == 2)
            {
                twoCoffeeImageR.SetActive(true);
                Debug.Log("images on the right ");

            }
        }


    }


    string GetAnimationTrigger(int sugarCount, int coffeeCount)
    {
        if (sugarCount == 1) return "OneSugar";
        if (sugarCount == 2) return "TwoSugar";
        if (coffeeCount == 1) return "OneCoffee";
        if (coffeeCount == 2) return "TwoCoffee";
        return "Empty";
    }

    void ResetCup(bool isLeft)
    {
        if (isLeft)
        {
            sugarCount = 0;
            coffeeCount = 0;
        }
        else
        {
            sugarCountSnd = 0;
            coffeeCountSnd = 0;
        }
       
    }
    public void StartOrder()
    {


        cupAimator.ResetTrigger("SmallSugar");
        cupAimator.ResetTrigger("LargeSugar");
        cupAimator.ResetTrigger("SmallCoffee");
        cupAimator.ResetTrigger("LargeCoffee");

        cupAimator.gameObject.SetActive(true);

        ingerdientForbidden = true;


        if (sugarCount == 1 && firstOrder)
        {
            cupAimator.SetTrigger("SmallSugar");

            Debug.Log("Triggering one Sugar");
            oneSugarImage.SetActive(false);

            AnimatorStateInfo stateInfo = cupAimator.GetCurrentAnimatorStateInfo(0);
            Debug.Log($"Current State: { stateInfo.fullPathHash}");
            IsOrderMatched("OneSugar");
            isFilling = true;


        }
        else if (sugarCount == 2 && firstOrder)
        {
            cupAimator.SetTrigger("LargeSugar");

            Debug.Log("Triggering Two Sugar");
            twoSugarImage.SetActive(false);

            AnimatorStateInfo stateInfo = cupAimator.GetCurrentAnimatorStateInfo(0);
            Debug.Log($"Current State: { stateInfo.fullPathHash}");
            IsOrderMatched("TwoSugar");
            isFilling = true;

        }
        else if (coffeeCount == 1 && firstOrder)

        {
            cupAimator.SetTrigger("SmallCoffee");


            Debug.Log("Triggering one Cofffee");
            oneCoffeeImage.SetActive(false);

            AnimatorStateInfo stateInfo = cupAimator.GetCurrentAnimatorStateInfo(0);
            Debug.Log($"Current State: { stateInfo.fullPathHash}");
            IsOrderMatched("OneCF");
            isFilling = true;
        }
        else if (coffeeCount == 2 && firstOrder)
        {
            cupAimator.SetTrigger("LargeCoffee");

            Debug.Log("Triggering Two Coffee");
            twoCoffeeImage.SetActive(false);

            AnimatorStateInfo stateInfo = cupAimator.GetCurrentAnimatorStateInfo(0);
            Debug.Log($"Current State: { stateInfo.fullPathHash}");
            IsOrderMatched("TwoCoffee");
            isFilling = true;

        }
        ResetOrder();


    }

    public void StartOrderSecond()
    {

        cupAnimatorRight.ResetTrigger("SmallCoffeeR");
        cupAnimatorRight.ResetTrigger("LargeCoffeeR");
        cupAnimatorRight.ResetTrigger("SmallSugarR");
        cupAnimatorRight.ResetTrigger("LargeSugarR");

        cupAnimatorRight.gameObject.SetActive(true);

        ingerdientForbiddenScnd = true;


        if (sugarCountSnd == 2)
        {
            cupAnimatorRight.SetTrigger("LargeSugarR");
            Debug.Log("Triggering Two Sugar on Rightside");
            twoSugarImageR.SetActive(false);
            AnimatorStateInfo stateInfo = cupAnimatorRight.GetCurrentAnimatorStateInfo(0);
            Debug.Log($"Current State: { stateInfo.fullPathHash}");
            IsOrderMatchedSecond("TwoSugar");
            isFillingRightside = true;

        }
        else if (coffeeCountSnd == 1)

        {
            cupAnimatorRight.SetTrigger("SmallCoffeeR");
            Debug.Log("Triggering one Cofffee on Rightside");
            oneCoffeeImageR.SetActive(false);
            AnimatorStateInfo stateInfo = cupAnimatorRight.GetCurrentAnimatorStateInfo(0);
            Debug.Log($"Current State: { stateInfo.fullPathHash}");
            IsOrderMatchedSecond("OneCF");
            isFillingRightside = true;
        }
        else if (coffeeCountSnd == 2)
        {
            cupAnimatorRight.SetTrigger("LargeCoffeeR");
            Debug.Log("Triggering Two Coffee on Rightside");
            twoCoffeeImageR.SetActive(false);
            AnimatorStateInfo stateInfo = cupAnimatorRight.GetCurrentAnimatorStateInfo(0);
            Debug.Log($"Current State: { stateInfo.fullPathHash}");
            IsOrderMatchedSecond("TwoCoffee");
            isFillingRightside = true;

        }
        else if (sugarCountSnd == 1)
        {
            cupAnimatorRight.SetTrigger("SmallSugarR");
            Debug.Log("Triggering one Sugar on Rightside");
            oneSugarImageR.SetActive(false);
            AnimatorStateInfo stateInfo = cupAnimatorRight.GetCurrentAnimatorStateInfo(0);
            Debug.Log($"Current State: { stateInfo.fullPathHash}");
            IsOrderMatchedSecond("OneSugar");
            isFillingRightside = true;


        }
        ResetOrderSnd();


    }


    public void ResetOrder()
    {
        sugarCount = 0;
        coffeeCount = 0;

    }
    public void ResetOrderSnd()
    {
        sugarCountSnd = 0;
        coffeeCountSnd = 0;
    }

  
    public void PauseButton()
    {
        SceneManager.LoadScene(1);
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
        firstOrder = false;
        // DelayforNextOrder();
        emptyCupImage.gameObject.SetActive(true);
        ingerdientForbidden = false;
        isLeftAcvtive = false;
        CompleteOrder("One");


    }
    public void serveCupTwo()
    {
        isServeRightside = true;
        emptyCupImageR.gameObject.SetActive(false);
        oneSugarImageR.gameObject.SetActive(false);
        twoSugarImageR.gameObject.SetActive(false);
        oneCoffeeImageR.gameObject.SetActive(false);
        twoCoffeeImageR.gameObject.SetActive(false);
        oneSugarAnimRight.SetActive(false);
        twoSugarAnimRigh.SetActive(false);
        oneCoffeeAnimRight.SetActive(false);
        TwoCoffeeAnimRight.SetActive(false);
        serveAnimateRightside.SetActive(true);
        Serve.Play("Serve", -1, 0f);
        Debug.Log("cup is being served");
        secondOrder = false;
        //DelayforNextOrder();
        emptyCupImageR.gameObject.SetActive(true);
        ingerdientForbiddenScnd = false;
        isRightAcvtive = false;
        CompleteOrder("Two");



    }
    
    public void IsOrderMatched(string doneOrder)
    {
        if (doneOrder == randomOrder || doneOrder == randomOrderTwo)
        {
            score += 500;
            scoreText.text = "Score: " + score;
            Debug.Log(scoreText.text);
        }
    }
    public void IsOrderMatchedSecond(string doneOrderSecond)
    {
        if (randomOrder == doneOrderSecond || randomOrderTwo == doneOrderSecond)
        {
            score += 500;
            scoreText.text = "Score: " + score;
            Debug.Log(scoreText.text);
        }
    }


    void SelectCup(bool isLeft)
    {
        if (isLeft)
        {
            selectedCup = emptyCupImage;


            Debug.Log("left cup selected ");
        }
        else
        {
            selectedCup = emptyCupImageR;


            Debug.Log("right cup selected ");
        }
    }



}