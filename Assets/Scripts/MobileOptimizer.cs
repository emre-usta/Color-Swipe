using UnityEngine;

public class MobileOptimizer : MonoBehaviour
{
    [Header("Performance Settings")]
    public int targetFrameRate = 60;
    public bool limitFrameRate = true;
    public bool enableVSync = false;
    
    [Header("Quality Settings")]
    public bool optimizeForMobile = true;
    public int textureQualityLevel = 1; // 0 = Full, 1 = Half, 2 = Quarter
    public bool enableAntiAliasing = false;
    public int shadowQuality = 0; // 0 = Disabled, 1 = Hard, 2 = Soft
    
    [Header("Battery Optimization")]
    public bool reduceFPSWhenInactive = true;
    public int inactiveFrameRate = 30;
    public bool pauseWhenInBackground = true;
    
    [Header("Memory Management")]
    public bool enableObjectPooling = true;
    public float garbageCollectionInterval = 10f;
    
    private bool isApplicationFocused = true;
    private float lastGCTime = 0f;
    
    void Awake()
    {
        // Ensure this object persists across scene loads
        DontDestroyOnLoad(gameObject);
        
        // Apply mobile optimizations
        ApplyMobileOptimizations();
    }
    
    void Start()
    {
        SetupPerformanceSettings();
    }
    
    void Update()
    {
        ManageGarbageCollection();
        MonitorPerformance();
    }
    
    void ApplyMobileOptimizations()
    {
        if (!optimizeForMobile) return;
        
        // Disable VSync for better battery life
        QualitySettings.vSyncCount = enableVSync ? 1 : 0;
        
        // Set target frame rate
        if (limitFrameRate)
        {
            Application.targetFrameRate = targetFrameRate;
        }
        
        // Optimize quality settings for mobile
        QualitySettings.masterTextureLimit = textureQualityLevel;
        QualitySettings.antiAliasing = enableAntiAliasing ? 2 : 0;
        QualitySettings.shadows = (ShadowQuality)shadowQuality;
        
        // Disable unnecessary rendering features
        QualitySettings.softParticles = false;
        QualitySettings.realtimeReflectionProbes = false;
        
        // Optimize physics
        Physics2D.baumgarteScale = 0.2f;
        Physics2D.baumgarteTOIScale = 0.75f;
        Physics2D.timeToSleep = 0.5f;
        Physics2D.linearSleepTolerance = 0.01f;
        Physics2D.angularSleepTolerance = 2.0f;
        
        // Optimize for touch input
        Input.multiTouchEnabled = false; // Single touch is enough for this game
        Input.simulateMouseWithTouches = true;
    }
    
    void SetupPerformanceSettings()
    {
        // Optimize screen settings for mobile
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        
        // Set appropriate screen orientation
        Screen.orientation = ScreenOrientation.Portrait;
        Screen.autorotateToPortrait = true;
        Screen.autorotateToPortraitUpsideDown = false;
        Screen.autorotateToLandscapeLeft = false;
        Screen.autorotateToLandscapeRight = false;
        
        Debug.Log("Mobile Optimizer: Performance settings applied");
    }
    
    void ManageGarbageCollection()
    {
        if (Time.time - lastGCTime > garbageCollectionInterval)
        {
            System.GC.Collect();
            lastGCTime = Time.time;
        }
    }
    
    void MonitorPerformance()
    {
        // Adjust frame rate based on application focus
        if (reduceFPSWhenInactive && !isApplicationFocused)
        {
            Application.targetFrameRate = inactiveFrameRate;
        }
        else if (limitFrameRate)
        {
            Application.targetFrameRate = targetFrameRate;
        }
        
        // Check for low frame rate and adjust quality if needed
        if (Time.smoothDeltaTime > 1f / 30f) // Below 30 FPS
        {
            OptimizeForLowPerformance();
        }
    }
    
    void OptimizeForLowPerformance()
    {
        // Reduce quality settings if performance is poor
        if (QualitySettings.antiAliasing > 0)
        {
            QualitySettings.antiAliasing = 0;
            Debug.Log("Mobile Optimizer: Disabled anti-aliasing due to low performance");
        }
        
        if (QualitySettings.masterTextureLimit < 2)
        {
            QualitySettings.masterTextureLimit++;
            Debug.Log("Mobile Optimizer: Reduced texture quality due to low performance");
        }
    }
    
    void OnApplicationFocus(bool hasFocus)
    {
        isApplicationFocused = hasFocus;
        
        if (pauseWhenInBackground)
        {
            if (hasFocus)
            {
                Time.timeScale = 1f;
                AudioListener.pause = false;
            }
            else
            {
                Time.timeScale = 0f;
                AudioListener.pause = true;
            }
        }
        
        Debug.Log($"Mobile Optimizer: Application focus changed to {hasFocus}");
    }
    
    void OnApplicationPause(bool pauseStatus)
    {
        if (pauseWhenInBackground)
        {
            if (pauseStatus)
            {
                Time.timeScale = 0f;
                AudioListener.pause = true;
            }
            else
            {
                Time.timeScale = 1f;
                AudioListener.pause = false;
            }
        }
        
        Debug.Log($"Mobile Optimizer: Application pause changed to {pauseStatus}");
    }
    
    public void SetTargetFrameRate(int frameRate)
    {
        targetFrameRate = frameRate;
        if (limitFrameRate)
        {
            Application.targetFrameRate = targetFrameRate;
        }
    }
    
    public void ToggleVSync(bool enabled)
    {
        enableVSync = enabled;
        QualitySettings.vSyncCount = enabled ? 1 : 0;
    }
    
    public void SetTextureQuality(int quality)
    {
        textureQualityLevel = Mathf.Clamp(quality, 0, 3);
        QualitySettings.masterTextureLimit = textureQualityLevel;
    }
    
    public void SetAntiAliasing(bool enabled)
    {
        enableAntiAliasing = enabled;
        QualitySettings.antiAliasing = enabled ? 2 : 0;
    }
    
    public float GetCurrentFPS()
    {
        return 1f / Time.smoothDeltaTime;
    }
    
    public string GetPerformanceStats()
    {
        return $"FPS: {GetCurrentFPS():F1}\n" +
               $"Frame Time: {Time.smoothDeltaTime * 1000f:F1}ms\n" +
               $"Memory: {System.GC.GetTotalMemory(false) / 1024 / 1024}MB\n" +
               $"Texture Quality: {QualitySettings.masterTextureLimit}\n" +
               $"Anti-Aliasing: {QualitySettings.antiAliasing}x";
    }
}