using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class HangmanController : MonoBehaviour
{
    [SerializeField] GameObject letterContainer;
    [SerializeField] GameObject keyboardButton;
    [SerializeField] GameObject keyboardContainer;
    [SerializeField] GameObject wordContainer;
    [SerializeField] GameObject[] hangmanStages;
    [SerializeField] TextAsset possibleWords;

    private string word;
    private int incorrectGuesses, correctGuessses;

    private void Start()
    {
        InitializeButtons();
        InitialiseGame();
    }

    private void InitialiseGame()
    {
        //Bütün oyunu resetlerken
        incorrectGuesses = 0;
        correctGuessses = 0;
        foreach(Button child in keyboardContainer.GetComponentsInChildren<Button>())
        {
            child.interactable = true;
        }
        //Kesin buradan hata alacağız
        foreach(Transform child in wordContainer.transform)
        {
            Destroy(child.gameObject);
        }
        foreach(GameObject stage in hangmanStages)
        {
            stage.SetActive(false);
        }

        word = GenerateWord().ToUpper();

        foreach(char letter in word)
        {
            Instantiate(letterContainer, wordContainer.transform);
        }
    }
    
    private string GenerateWord()
    {
        string[] wordList = possibleWords.text.Split("\n");
        string line = wordList[Random.Range(0, wordList.Length)];
        return line.Substring(0, line.Length - 1);
    }


    private void InitializeButtons()
    {
        for(int i = 65; i <= 90; i++)
        {
            CreateButton(i);
        }
    }

    private void CreateButton(int i)
    {
        GameObject temp = Instantiate(keyboardButton, keyboardContainer.transform);
        temp.GetComponentInChildren<TextMeshProUGUI>().text = ((char)i).ToString();
        temp.GetComponent<Button>().onClick.AddListener(delegate { CheckLetter(((char)i).ToString()); });
    }

    private void CheckLetter(string inputLetter)
    {
        bool letterInWord = false;

        for(int i = 0; i < word.Length; i++)
        {
            if (inputLetter == word[i].ToString())
            {
                letterInWord = true;
                correctGuessses++;
                wordContainer.GetComponentsInChildren<TextMeshProUGUI>()[i].text = inputLetter;
            }
        }


        if(letterInWord == false)
        {
            incorrectGuesses++;
            hangmanStages[incorrectGuesses - 1].SetActive(true);
        }

        CheckOutcome();
    }

    private void CheckOutcome()
    {
        if(correctGuessses == word.Length)
        {
            for(int i = 0; i < word.Length; i++)
            {
                wordContainer.GetComponentsInChildren<TextMeshProUGUI>()[i].color = Color.green;
            }
            Invoke("InitialiseGame", 5f);
        }

        if(incorrectGuesses == hangmanStages.Length)
        {
            for (int i = 0; i < word.Length; i++)
            {
                wordContainer.GetComponentsInChildren<TextMeshProUGUI>()[i].color = Color.red;
                wordContainer.GetComponentsInChildren<TextMeshProUGUI>()[i].text = word[i].ToString();
            }
            Invoke("InitialiseGame", 3f);
        }
    }
}
