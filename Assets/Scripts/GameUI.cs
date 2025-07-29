using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameUI : MonoBehaviour
{
    [Header("UI Panels")]
    public GameObject gamePlayPanel;
    public GameObject gameOverPanel;
    public GameObject pausePanel;
    
    [Header("Game UI Elements")]
    public Text scoreText;
    public Text levelText;
    public Text targetColorText;
    public Image targetColorDisplay;
    public Button pauseButton;
    
    [Header("Game Over UI")]
    public Text finalScoreText;
    public Text highScoreText;
    public Button restartButton;
    public Button mainMenuButton;
    
    [Header("Pause UI")]
    public Button resumeButton;
    public Button pauseRestartButton;
    public Button pauseMainMenuButton;
    public Slider sfxVolumeSlider;
    public Slider musicVolumeSlider;
    
    [Header("Animation Settings")]
    public float fadeInDuration = 0.5f;
    public float fadeOutDuration = 0.3f;
    public float scaleAnimationDuration = 0.4f;
    
    private CanvasGroup gamePlayCanvasGroup;
    private CanvasGroup gameOverCanvasGroup;
    private CanvasGroup pauseCanvasGroup;
    
    private int highScore = 0;
    
    void Awake()
    {
        SetupCanvasGroups();
        LoadHighScore();
        SetupButtons();
        SetupVolumeSliders();
    }
    
    void SetupCanvasGroups()
    {
        if (gamePlayPanel != null)
        {
            gamePlayCanvasGroup = gamePlayPanel.GetComponent<CanvasGroup>();
            if (gamePlayCanvasGroup == null)
                gamePlayCanvasGroup = gamePlayPanel.AddComponent<CanvasGroup>();
        }
        
        if (gameOverPanel != null)
        {
            gameOverCanvasGroup = gameOverPanel.GetComponent<CanvasGroup>();
            if (gameOverCanvasGroup == null)
                gameOverCanvasGroup = gameOverPanel.AddComponent<CanvasGroup>();
        }
        
        if (pausePanel != null)
        {
            pauseCanvasGroup = pausePanel.GetComponent<CanvasGroup>();
            if (pauseCanvasGroup == null)
                pauseCanvasGroup = pausePanel.AddComponent<CanvasGroup>();
        }
    }
    
    void SetupButtons()
    {
        if (pauseButton != null)
            pauseButton.onClick.AddListener(PauseGame);
        
        if (resumeButton != null)
            resumeButton.onClick.AddListener(ResumeGame);
        
        if (restartButton != null)
            restartButton.onClick.AddListener(RestartGame);
        
        if (pauseRestartButton != null)
            pauseRestartButton.onClick.AddListener(RestartGameFromPause);
        
        if (mainMenuButton != null)
            mainMenuButton.onClick.AddListener(GoToMainMenu);
        
        if (pauseMainMenuButton != null)
            pauseMainMenuButton.onClick.AddListener(GoToMainMenuFromPause);
    }
    
    void SetupVolumeSliders()
    {
        if (sfxVolumeSlider != null)
        {
            sfxVolumeSlider.value = AudioManager.Instance != null ? AudioManager.Instance.sfxVolume : 0.7f;
            sfxVolumeSlider.onValueChanged.AddListener(OnSFXVolumeChanged);
        }
        
        if (musicVolumeSlider != null)
        {
            musicVolumeSlider.value = AudioManager.Instance != null ? AudioManager.Instance.musicVolume : 0.3f;
            musicVolumeSlider.onValueChanged.AddListener(OnMusicVolumeChanged);
        }
    }
    
    public void ShowGamePlay()
    {
        ShowPanel(gamePlayPanel, gamePlayCanvasGroup);
        HidePanel(gameOverPanel, gameOverCanvasGroup);
        HidePanel(pausePanel, pauseCanvasGroup);
    }
    
    public void ShowGameOver(int finalScore)
    {
        UpdateFinalScore(finalScore);
        ShowPanel(gameOverPanel, gameOverCanvasGroup);
        HidePanel(gamePlayPanel, gamePlayCanvasGroup);
        
        // Animate game over panel entrance
        if (gameOverPanel != null)
        {
            StartCoroutine(AnimateGameOverPanel());
        }
    }
    
    public void ShowPauseMenu()
    {
        ShowPanel(pausePanel, pauseCanvasGroup);
        Time.timeScale = 0f;
    }
    
    public void HidePauseMenu()
    {
        HidePanel(pausePanel, pauseCanvasGroup);
        Time.timeScale = 1f;
    }
    
    void ShowPanel(GameObject panel, CanvasGroup canvasGroup)
    {
        if (panel != null)
        {
            panel.SetActive(true);
            if (canvasGroup != null)
            {
                StartCoroutine(FadeIn(canvasGroup));
            }
        }
    }
    
    void HidePanel(GameObject panel, CanvasGroup canvasGroup)
    {
        if (panel != null && canvasGroup != null)
        {
            StartCoroutine(FadeOutAndHide(panel, canvasGroup));
        }
        else if (panel != null)
        {
            panel.SetActive(false);
        }
    }
    
    IEnumerator FadeIn(CanvasGroup canvasGroup)
    {
        canvasGroup.alpha = 0f;
        canvasGroup.interactable = false;
        
        float elapsed = 0f;
        while (elapsed < fadeInDuration)
        {
            elapsed += Time.unscaledDeltaTime;
            canvasGroup.alpha = Mathf.Lerp(0f, 1f, elapsed / fadeInDuration);
            yield return null;
        }
        
        canvasGroup.alpha = 1f;
        canvasGroup.interactable = true;
    }
    
    IEnumerator FadeOutAndHide(GameObject panel, CanvasGroup canvasGroup)
    {
        canvasGroup.interactable = false;
        
        float elapsed = 0f;
        while (elapsed < fadeOutDuration)
        {
            elapsed += Time.unscaledDeltaTime;
            canvasGroup.alpha = Mathf.Lerp(1f, 0f, elapsed / fadeOutDuration);
            yield return null;
        }
        
        canvasGroup.alpha = 0f;
        panel.SetActive(false);
    }
    
    IEnumerator AnimateGameOverPanel()
    {
        Transform panelTransform = gameOverPanel.transform;
        Vector3 originalScale = panelTransform.localScale;
        panelTransform.localScale = Vector3.zero;
        
        float elapsed = 0f;
        while (elapsed < scaleAnimationDuration)
        {
            elapsed += Time.unscaledDeltaTime;
            float progress = elapsed / scaleAnimationDuration;
            float easeOutBounce = EaseOutBounce(progress);
            panelTransform.localScale = Vector3.Lerp(Vector3.zero, originalScale, easeOutBounce);
            yield return null;
        }
        
        panelTransform.localScale = originalScale;
    }
    
    float EaseOutBounce(float t)
    {
        if (t < 1f / 2.75f)
        {
            return 7.5625f * t * t;
        }
        else if (t < 2f / 2.75f)
        {
            return 7.5625f * (t -= 1.5f / 2.75f) * t + 0.75f;
        }
        else if (t < 2.5f / 2.75f)
        {
            return 7.5625f * (t -= 2.25f / 2.75f) * t + 0.9375f;
        }
        else
        {
            return 7.5625f * (t -= 2.625f / 2.75f) * t + 0.984375f;
        }
    }
    
    public void UpdateScore(int score)
    {
        if (scoreText != null)
        {
            scoreText.text = "Score: " + score;
            AnimateScoreUpdate();
        }
    }
    
    public void UpdateLevel(int level)
    {
        if (levelText != null)
        {
            levelText.text = "Level: " + level;
        }
    }
    
    public void UpdateTargetColor(string colorName, Color color)
    {
        if (targetColorText != null)
            targetColorText.text = "Target: " + colorName;
        
        if (targetColorDisplay != null)
            targetColorDisplay.color = color;
    }
    
    void UpdateFinalScore(int score)
    {
        if (finalScoreText != null)
            finalScoreText.text = "Score: " + score;
        
        if (score > highScore)
        {
            highScore = score;
            SaveHighScore();
        }
        
        if (highScoreText != null)
            highScoreText.text = "Best: " + highScore;
    }
    
    void AnimateScoreUpdate()
    {
        if (scoreText != null)
        {
            StartCoroutine(ScalePulse(scoreText.transform));
        }
    }
    
    IEnumerator ScalePulse(Transform target)
    {
        Vector3 originalScale = target.localScale;
        Vector3 targetScale = originalScale * 1.2f;
        
        // Scale up
        float elapsed = 0f;
        while (elapsed < 0.1f)
        {
            elapsed += Time.unscaledDeltaTime;
            target.localScale = Vector3.Lerp(originalScale, targetScale, elapsed / 0.1f);
            yield return null;
        }
        
        // Scale down
        elapsed = 0f;
        while (elapsed < 0.1f)
        {
            elapsed += Time.unscaledDeltaTime;
            target.localScale = Vector3.Lerp(targetScale, originalScale, elapsed / 0.1f);
            yield return null;
        }
        
        target.localScale = originalScale;
    }
    
    void PauseGame()
    {
        ShowPauseMenu();
    }
    
    void ResumeGame()
    {
        HidePauseMenu();
    }
    
    void RestartGame()
    {
        Time.timeScale = 1f;
        ColorWheelGame gameController = FindObjectOfType<ColorWheelGame>();
        if (gameController != null)
        {
            gameController.RestartGame();
        }
        ShowGamePlay();
    }
    
    void RestartGameFromPause()
    {
        HidePauseMenu();
        RestartGame();
    }
    
    void GoToMainMenu()
    {
        Time.timeScale = 1f;
        // Load main menu scene - for now just restart
        RestartGame();
    }
    
    void GoToMainMenuFromPause()
    {
        HidePauseMenu();
        GoToMainMenu();
    }
    
    void OnSFXVolumeChanged(float value)
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.SetSFXVolume(value);
        }
    }
    
    void OnMusicVolumeChanged(float value)
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.SetMusicVolume(value);
        }
    }
    
    void LoadHighScore()
    {
        highScore = PlayerPrefs.GetInt("ColorWheelHighScore", 0);
    }
    
    void SaveHighScore()
    {
        PlayerPrefs.SetInt("ColorWheelHighScore", highScore);
        PlayerPrefs.Save();
    }
}