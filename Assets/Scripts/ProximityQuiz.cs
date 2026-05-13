using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ProximityQuiz : MonoBehaviour
{
    [Header("Prompt Settings")]
    public string promptMessage = "Press E to begin quiz";
    public float promptRadius = 3f;
    public KeyCode interactKey = KeyCode.E;

    [Header("UI References")]
    public GameObject promptUI;          
    public TMP_Text promptText;          
    public GameObject quizPanel;         
    public TMP_Text questionText;        
    public Button[] answerButtons;       
    public Button optionCButton;         

    [Header("Quiz Data")]
    public string question = "What is the capital of France?";
    public string[] answers = { "Berlin", "Paris", "Rome", "Madrid" };
    public int correctAnswerIndex = 1;

    [Header("Reward Settings")]
    public GameObject keyPrefab;         
    public Transform spawnPoint;         
    public GameObject quizObject;        

    private Transform player;
    private bool isInRange = false;
    private bool quizActive = false;

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
        if (keyPrefab != null) keyPrefab.SetActive(false);


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
            optionCButton.onClick.AddListener(OnCorrectAnswer);
        }

        foreach (Button btn in answerButtons)
        {
            if (btn != null && btn != optionCButton)
            {
                btn.onClick.RemoveAllListeners();
                btn.onClick.AddListener(OnWrongAnswer);
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
        if (promptUI != null)
            promptUI.SetActive(false);
    }

    private void StartQuiz()
    {
        quizActive = true;
        HidePrompt();

        if (quizPanel != null)
        {
            quizPanel.SetActive(true);
            if (questionText != null)
                questionText.text = question;

            // Unlock cursor for UI
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    private void OnCorrectAnswer()
    {
        Debug.Log("Correct Answer Clicked!");
        SpawnReward();
        TriggerHoverAndRotate();
        EndQuiz();
    }

    private void OnWrongAnswer()
    {
        Debug.Log("Wrong Answer Clicked!");
        EndQuiz();
    }

    private void EndQuiz()
    {
        if (quizPanel != null)
            quizPanel.SetActive(false);

        quizActive = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void SpawnReward()
    {
        if (keyPrefab != null)
        {
            Debug.Log("Spawning key reward...");
            keyPrefab.SetActive(true);
            if (spawnPoint != null)
                keyPrefab.transform.position = spawnPoint.position;
        }
    }

    private void TriggerHoverAndRotate()
    {
        if (quizObject != null)
        {
            Debug.Log("Activating hover and rotation on quiz object...");

            HoverLift liftScript = quizObject.GetComponent<HoverLift>();
            RotateObject rotateScript = quizObject.GetComponent<RotateObject>();

            if (liftScript != null)
            {
                liftScript.enabled = true;
                liftScript.ActivateHover();
            }

            if (rotateScript != null)
                rotateScript.enabled = true;
        }
        else
        {
            Debug.LogWarning("Quiz object not assigned in inspector!");
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, promptRadius);
    }
}
