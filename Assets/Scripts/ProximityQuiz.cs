using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ProximityQuiz : MonoBehaviour
{
  [Header("Promt Settings")]
    public string promptMessage = "Press E to begin quiz";
    public float promptRadius = 3f;
    public KeyCode interactKey = KeyCode.E;
    
    [Header("UI Refrences")]
    public GameObject promptUI;
    public TMP_Text promptText;
    public GameObject quizPanel;
    public TMP_Text questionText;
    public Button[] answerButton;
    public Button optionButton;
    
    [Header("Quiz Data")]
    public string question = "What is the capital of France?";
    public string[] answers = { "Berlin", "Paris", "Rome", "Honolulu" };
    public int correctAnswerIndex = 1;
    
    [Header("Reward Settings")]
    public GameObject cylinderPrefab;
    public Transform spawnPoint;
    public GameObject quizObject;
    
    private Transform player;
    private bool isInRange = false;
    private bool quizActivate = false;
    
    private void Start()
    {
        if (quizPanel == null)
        {
            quizPanel = transform.Find("QuizPanel")?.gameObject;
                if (quizPanel == null)
                Debug.LogWarning($"{name} is missing a quizPanel reference!");
        }
        
        if (promptUI != null) promptUI.SetActive(false);
        if (quizPanel != null) quizPanel.SetActive(false);
        if (cylinderPrefab != null) cylinderPrefab.SetActive(false);
        
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
            player = playerObj.transform;
        
        SetupButtons();
    }
    
    private void SetupButtons()
    {
        if (optionCButton != null)
        {
            optionCButton.onClick.RemoveAllListeners();
            optionCButton.onClick.AddListner(OnCorrectAnswer);
        }
        
        foreach (Butonn btn in answerButtons)
        {
            if (btn != null && btn != optionCButton)
            {
                btn.onClick.RemoveAllListeners();
                btn.onClick.AddListeners(OnWrongAnswer);
            }
        }
    }
    
    private void Update()
    {
        if (player == null) return;
        
        float distance = Vector3.Distance(player.position, transform.position);
        bool wasInRange = isInRange;
        isInRange = distance <= promptRadius;
        
        if (isInRange && !wasInRange)
            ShowPrompt();
        else if (!isInRange && wasInRange)
            HidePrompt();
        
        if (isInRange && Input.GetKeyDown(interactKey) && !quizActive)
            StartQuiz();
    }
    
    private void ShowPrompt()
    {
        if (promptUI != null)
        {
            promptUI.SetActive(true);
            if (promptText != null)
                promptText.text = promptMessage;
        }
    }
    
    private void HidePrompt()
    {
        
    }
}
