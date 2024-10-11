using System;
using System.Collections.Generic;
using UnityEngine;

public class JsonReaderQuiz : MonoBehaviour
{
    [Serializable]
    public class Question
    {
        public int question_id;
        public string question;
        public List<string> answers;
        public int correct;
    }

    [Serializable]
    public class QuestionList
    {
        public List<Question> questions;
    }

    public QuestionList quizQuestions = new QuestionList();

    public TextAsset jsonCarthageArabic;
    public TextAsset jsonCarthageEnglish;
    public TextAsset jsonCarthageFrench;
    public TextAsset jsonMatmataArabic;
    public TextAsset jsonMatmataEnglish;
    public TextAsset jsonMatmataFrench;
    public TextAsset jsonKSAArabic;
    public TextAsset jsonKSAEnglish;
    public TextAsset jsonKSAFrench;


    public Dictionary<int, Question> questionsById;

    public void LoadJson(TextAsset jsonFile)
    {
       


        if (jsonFile != null)
        {
           // Debug.Log("Loading JSON file: " + jsonFile.name);
            try
            {
                quizQuestions = JsonUtility.FromJson<QuestionList>(jsonFile.text);

               // Debug.Log("Loaded JSON: " + jsonFile.text);

                // Log the loaded info to check if parsing is correct
                foreach (var question in quizQuestions.questions)
                {
                  //  questionsById[question.question_id] = question;
                    //Debug.Log($"Loaded Question ID: {question.question_id}, Content={question.question}");
                }
            }
            catch (Exception e)
            {
                Debug.LogError("Error parsing JSON: " + e.Message);
                Debug.LogError("JSON content: " + jsonFile.text);
            }
        }
        else
        {
            Debug.LogError("JSON file is null.");
        }
    }

    public Question GetQuestionByID(int id)
    {
        if (questionsById != null && questionsById.ContainsKey(id))
        {
            return questionsById[id];
        }
        return null;
    }
}
