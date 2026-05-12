using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public static float BottomBorder { get; private set; }

    [Header("Game Settings")]
    [SerializeField] private float bottomBorder = -8.5f;

    [Header("Events")]
    public UnityEvent SuccessfullShotEvent;
    public UnityEvent FailedShotEvent;
    public UnityEvent GameOverEvent;

    [Header("References")]
    [SerializeField] private TextMeshProUGUI gameScoreUI;
    [SerializeField] private Launcher launcher;

    private int gameScore = 0;

    void Awake()
    {
        if (Instance == null) Instance = this;

        BottomBorder = bottomBorder;

        SuccessfullShotEvent.AddListener(OnSuccessfullShot);
        FailedShotEvent.AddListener(OnFailedShot);
    }

    public void Restart()
    {
        gameScore = 0;
        if (gameScoreUI != null) gameScoreUI.text = "0";

        launcher.AddBall();
    }

    private void OnSuccessfullShot()
    {
        if (gameScoreUI != null) gameScoreUI.text = (++gameScore).ToString();
        launcher?.AddBall();
    }
    private void OnFailedShot()
    {
        GameOverEvent?.Invoke();
    }
}