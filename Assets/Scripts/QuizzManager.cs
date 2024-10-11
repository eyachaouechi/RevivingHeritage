using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using static JsonReaderQuiz;

public class QuizzManager : MonoBehaviour
{
    public int[] questionIDs; // Manually set the question IDs in the inspector
    public TextMeshProUGUI questionText;
    public Button[] answerButtons;
    public JsonReaderQuiz questionLoader;
    public int level;
    private string language; 

    public AudioClip correctAudioClip;
    public AudioClip wrongAudioClip;
    private AudioSource audioSource;
    public List<Question> questions;
    public Dictionary<int, Question> questionsById;

    private int currentQuestionIndex;
    public TextAsset selectedJSON;

    void Start()
    {

        int selectedLevel = PlayerPrefs.GetInt("SelectedLevel", 0);
        int localizationID = PlayerPrefs.GetInt("Localekey", 0);
        string localization = GetLocalizationString(localizationID);
        selectedJSON = null;


        selectedJSON=JsonFormat(selectedLevel, localization);

        if (selectedJSON != null)
        {
            questionLoader.LoadJson(selectedJSON);
            Debug.Log("JSON loaded: " + selectedJSON.name);

            // Initialize the dictionary
            questionsById = new Dictionary<int, Question>();

            // Populate the dictionary with questions
            foreach (var question in questionLoader.quizQuestions.questions)
            {
                if (!questionsById.ContainsKey(question.question_id))
                {
                    questionsById[question.question_id] = question;
                    Debug.Log($"Added Question ID: {question.question_id}");
                }
                else
                {
                    Debug.LogWarning($"Duplicate question ID found: {question.question_id}");
                }
            }

            // Load questions based on the provided IDs
            foreach (int id in questionIDs)
            {
                Debug.Log($"Looking for question with ID: {id}");
                if (questionsById.TryGetValue(id, out var question))
                {
                    questions.Add(question);
                    Debug.Log($"Added Question: {question.question}");
                }
                else
                {
                    Debug.LogError($"Question with ID {id} not found in the JSON.");
                }
            }
        }
        else
        {
            Debug.LogError("Selected JSON file is null.");
        }
   
         currentQuestionIndex = 0;
         LoadQuestion();

      
        if (audioSource == null)
        {
            audioSource = FindObjectOfType<AudioSource>();
            if (audioSource == null)
            {
               // Debug.Log("AudioSource component not found in the scene.");
            }
        }
    }


    private TextAsset JsonFormat(int level, string localizationID)
    {
        if (level == 0)
        {
            if (localizationID == "AR")
            {
                return questionLoader.jsonCarthageArabic;
            }
            if (localizationID == "ENG")
            {
                return questionLoader.jsonCarthageEnglish;
            }
            else
            {
                return questionLoader.jsonCarthageFrench;
            }
        }
        if (level == 1)
        {
            if (localizationID == "AR")
            {
                return questionLoader.jsonMatmataArabic;
            }
            if (localizationID == "ENG")
            {
                return questionLoader.jsonMatmataEnglish;
            }
            else
            {
                return questionLoader.jsonMatmataFrench;
            }
        }
        else
        {
            if (localizationID == "AR")
            {
                return questionLoader.jsonKSAArabic;
            }
            if (localizationID == "ENG")
            {
                return questionLoader.jsonKSAEnglish;
            }
            else
            {
                return questionLoader.jsonKSAFrench;
            }
        }
    }

    private string GetLocalizationString(int localizationID)
    {
        switch (localizationID)
        {
            case 0:
                return "AR"; // Arabic
            case 1:
                return "ENG"; // English
            case 2:
                return "FR"; // French
            default:
                return "ENG"; // Default to English if unknown
        }
    }

    public void LoadQuestion()
    {

        Debug.Log("id 0 is " + questionIDs[0]); ;
;
        if (questions != null/* && currentQuestionIndex <= questions.Count*/)
        {
          //  Debug.Log($"Displaying Question: {question.question}");
            questionText.text = questions[currentQuestionIndex].question;
            var question = questions[currentQuestionIndex];

            for (int i = 0; i < answerButtons.Length; i++)
            {
                if (i < question.answers.Count) // Use Count instead of Length
                {
                    TextMeshProUGUI buttonText = answerButtons[i].GetComponentInChildren<TextMeshProUGUI>();
                    buttonText.text = question.answers[i];
                    Debug.Log($"Answer {i}: {question.answers[i]}");

                    int answerIndex = i; // Local copy for the lambda
                    answerButtons[i].onClick.RemoveAllListeners(); // Clear previous listeners
                    answerButtons[i].onClick.AddListener(() => OnAnswerClicked(answerIndex, question.correct));
                }
                else
                {
                    Debug.LogWarning($"Not enough answers provided for question: {question.question}");
                }
            }
        }
        else
        {
            Debug.LogError("No more questions available.");
        }
    }

    void OnAnswerClicked(int answerIndex, int correctAnswer)
    {
        if (answerIndex == correctAnswer)
        {
            StartCoroutine(HandleCorrectAnswer(answerButtons[answerIndex]));
        }
        else
        {
            StartCoroutine(HandleWrongAnswer(answerButtons[answerIndex]));
        }
    }

    IEnumerator HandleCorrectAnswer(Button answerButton)
    {
        Color initialColor = answerButton.GetComponent<Image>().color;
        answerButton.GetComponent<Image>().color = Color.green;
        foreach (var button in answerButtons) {
            button.interactable = false;
        }
        if (audioSource != null && correctAudioClip != null)
        {
            audioSource.PlayOneShot(correctAudioClip);
        }

        yield return new WaitForSeconds(2);
        answerButton.GetComponent<Image>().color = initialColor;
      

        currentQuestionIndex++;
        foreach (var button in answerButtons)
        {
            button.interactable = true;
        }
        if (currentQuestionIndex < questions.Count)
        {
            LoadQuestion();
        }
        else
        {
            ClosePanel();
        }
    }

    IEnumerator HandleWrongAnswer(Button answerButton)
    {
        Color initialColor = answerButton.GetComponent<Image>().color;
        answerButton.GetComponent<Image>().color = Color.red;
        foreach (var button in answerButtons)
        {
            button.interactable = false;
        }

        if (audioSource != null && wrongAudioClip != null)
        {
            audioSource.PlayOneShot(wrongAudioClip);
        }

        yield return new WaitForSeconds(2);

        answerButton.GetComponent<Image>().color = initialColor;
        foreach (var button in answerButtons)
        {
            button.interactable = true;
        }
    }

    void ClosePanel()
    {
        gameObject.transform.GetChild(3).gameObject.SetActive(false);
    }
}
