using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ColorWheelGame : MonoBehaviour
{
    [Header("Game Settings")]
    public float baseSpinSpeed = 100f;
    public float speedIncreasePerLevel = 20f;
    public float stopThreshold = 10f;
    
    [Header("UI References")]
    public GameUI gameUI;
    
    [Header("Game Objects")]
    public ColorWheel colorWheel;
    
    private int currentScore = 0;
    private Color targetColor;
    private string targetColorName;
    private bool gameActive = true;
    private int level = 1;
    
    // Color definitions
    private Color[] wheelColors = {
        Color.red,
        Color.blue,
        Color.green,
        Color.yellow,
        Color.magenta,
        Color.cyan
    };
    
    private string[] colorNames = {
        "Red",
        "Blue", 
        "Green",
        "Yellow",
        "Magenta",
        "Cyan"
    };
    
    void Start()
    {
        SetupGame();
        if (gameUI != null)
        {
            gameUI.ShowGamePlay();
        }
    }
    
    void SetupGame()
    {
        currentScore = 0;
        level = 1;
        gameActive = true;
        
        SelectNewTargetColor();
        UpdateUI();
        
        if (colorWheel != null)
        {
            colorWheel.SetColors(wheelColors);
            colorWheel.SetSpinSpeed(baseSpinSpeed);
        }
    }
    
    void SelectNewTargetColor()
    {
        int randomIndex = Random.Range(0, wheelColors.Length);
        targetColor = wheelColors[randomIndex];
        targetColorName = colorNames[randomIndex];
        
        if (gameUI != null)
            gameUI.UpdateTargetColor(targetColorName, targetColor);
    }
    
    public void OnWheelStopped(Color stoppedColor)
    {
        if (!gameActive) return;
        
        if (ColorMatches(stoppedColor, targetColor))
        {
            // Correct match
            currentScore += level * 10;
            level++;
            
            // Play correct match sound
            if (AudioManager.Instance != null)
                AudioManager.Instance.PlayCorrectMatch();
            
            // Increase wheel speed
            float newSpeed = baseSpinSpeed + (speedIncreasePerLevel * (level - 1));
            colorWheel.SetSpinSpeed(newSpeed);
            
            SelectNewTargetColor();
            UpdateUI();
            
            // Brief pause before next round
            StartCoroutine(NextRoundDelay());
        }
        else
        {
            // Wrong color - game over
            if (AudioManager.Instance != null)
                AudioManager.Instance.PlayWrongMatch();
                
            GameOver();
        }
    }
    
    bool ColorMatches(Color color1, Color color2)
    {
        float threshold = 0.1f;
        return Mathf.Abs(color1.r - color2.r) < threshold &&
               Mathf.Abs(color1.g - color2.g) < threshold &&
               Mathf.Abs(color1.b - color2.b) < threshold;
    }
    
    IEnumerator NextRoundDelay()
    {
        yield return new WaitForSeconds(0.5f);
        colorWheel.EnableInput();
    }
    
    void GameOver()
    {
        gameActive = false;
        
        if (AudioManager.Instance != null)
            AudioManager.Instance.PlayGameOver();
            
        if (gameUI != null)
            gameUI.ShowGameOver(currentScore);
            
        colorWheel.DisableInput();
    }
    
    void UpdateUI()
    {
        if (gameUI != null)
        {
            gameUI.UpdateScore(currentScore);
            gameUI.UpdateLevel(level);
        }
    }
    
    public void RestartGame()
    {
        SetupGame();
    }
}