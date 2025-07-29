using UnityEngine;
using System.Collections;

public class ColorWheel : MonoBehaviour
{
    [Header("Wheel Settings")]
    public Transform wheelTransform;
    public float friction = 0.95f;
    public float minSpinSpeed = 50f;
    
    [Header("Segments")]
    public WheelSegment[] segments = new WheelSegment[6];
    
    [Header("Input Settings")]
    public float swipeSensitivity = 2f;
    public float minSwipeDistance = 50f;
    
    private float currentSpinSpeed = 100f;
    private float targetSpinSpeed = 0f;
    private bool isSpinning = false;
    private bool inputEnabled = true;
    
    // Touch input variables
    private Vector2 startTouchPosition;
    private Vector2 lastTouchPosition;
    private bool isTouching = false;
    private float touchStartTime;
    
    private ColorWheelGame gameController;
    
    void Start()
    {
        gameController = FindObjectOfType<ColorWheelGame>();
        SetupSegments();
    }
    
    void Update()
    {
        HandleInput();
        UpdateWheelRotation();
        CheckIfStopped();
    }
    
    void SetupSegments()
    {
        if (segments.Length != 6)
        {
            Debug.LogError("ColorWheel must have exactly 6 segments!");
            return;
        }
        
        float anglePerSegment = 360f / 6f;
        
        for (int i = 0; i < segments.Length; i++)
        {
            if (segments[i] != null)
            {
                // Position each segment
                float angle = i * anglePerSegment;
                segments[i].transform.localRotation = Quaternion.Euler(0, 0, angle);
            }
        }
    }
    
    public void SetColors(Color[] colors)
    {
        if (colors.Length != 6)
        {
            Debug.LogError("Must provide exactly 6 colors for the wheel!");
            return;
        }
        
        for (int i = 0; i < segments.Length && i < colors.Length; i++)
        {
            if (segments[i] != null)
            {
                segments[i].SetColor(colors[i]);
            }
        }
    }
    
    public void SetSpinSpeed(float speed)
    {
        minSpinSpeed = speed;
    }
    
    void HandleInput()
    {
        if (!inputEnabled) return;
        
#if UNITY_EDITOR || UNITY_STANDALONE
        HandleMouseInput();
#elif UNITY_ANDROID || UNITY_IOS
        HandleTouchInput();
#endif
    }
    
    void HandleTouchInput()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            Vector2 touchPosition = Camera.main.ScreenToWorldPoint(touch.position);
            
            switch (touch.phase)
            {
                case TouchPhase.Began:
                    StartTouch(touchPosition);
                    break;
                    
                case TouchPhase.Moved:
                    UpdateTouch(touchPosition);
                    break;
                    
                case TouchPhase.Ended:
                case TouchPhase.Canceled:
                    EndTouch(touchPosition);
                    break;
            }
        }
    }
    
    void HandleMouseInput()
    {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        
        if (Input.GetMouseButtonDown(0))
        {
            StartTouch(mousePosition);
        }
        else if (Input.GetMouseButton(0) && isTouching)
        {
            UpdateTouch(mousePosition);
        }
        else if (Input.GetMouseButtonUp(0))
        {
            EndTouch(mousePosition);
        }
    }
    
    void StartTouch(Vector2 position)
    {
        startTouchPosition = position;
        lastTouchPosition = position;
        isTouching = true;
        touchStartTime = Time.time;
        targetSpinSpeed = 0f;
    }
    
    void UpdateTouch(Vector2 position)
    {
        if (!isTouching) return;
        
        Vector2 wheelCenter = wheelTransform.position;
        
        // Calculate angle difference
        Vector2 lastDirection = (lastTouchPosition - wheelCenter).normalized;
        Vector2 currentDirection = (position - wheelCenter).normalized;
        
        float angleDiff = Vector2.SignedAngle(lastDirection, currentDirection);
        
        // Apply rotation
        wheelTransform.Rotate(0, 0, angleDiff * swipeSensitivity);
        
        lastTouchPosition = position;
    }
    
    void EndTouch(Vector2 position)
    {
        if (!isTouching) return;
        
        isTouching = false;
        
        float swipeDistance = Vector2.Distance(startTouchPosition, position);
        float swipeTime = Time.time - touchStartTime;
        
        if (swipeDistance > minSwipeDistance && swipeTime < 0.5f)
        {
            // Calculate spin direction and speed based on swipe
            Vector2 wheelCenter = wheelTransform.position;
            Vector2 startDirection = (startTouchPosition - wheelCenter).normalized;
            Vector2 endDirection = (position - wheelCenter).normalized;
            
            float swipeAngle = Vector2.SignedAngle(startDirection, endDirection);
            float spinDirection = Mathf.Sign(swipeAngle);
            
            float speedMultiplier = Mathf.Clamp(swipeDistance / 200f, 0.5f, 3f);
            targetSpinSpeed = minSpinSpeed * speedMultiplier * spinDirection;
            
            // Play wheel spin sound
            if (AudioManager.Instance != null)
                AudioManager.Instance.PlayWheelSpin();
            
            isSpinning = true;
        }
    }
    
    void UpdateWheelRotation()
    {
        if (isSpinning)
        {
            // Apply spin speed
            wheelTransform.Rotate(0, 0, targetSpinSpeed * Time.deltaTime);
            
            // Apply friction
            targetSpinSpeed *= friction;
            
            // Check if wheel should stop
            if (Mathf.Abs(targetSpinSpeed) < 10f)
            {
                targetSpinSpeed = 0f;
                isSpinning = false;
            }
        }
    }
    
    void CheckIfStopped()
    {
        if (!isSpinning && targetSpinSpeed == 0f && !isTouching)
        {
            // Wheel has stopped, check which color is at the top
            Color stoppedColor = GetColorAtTop();
            if (gameController != null)
            {
                gameController.OnWheelStopped(stoppedColor);
            }
        }
    }
    
    Color GetColorAtTop()
    {
        // Determine which segment is closest to the top (0 degrees)
        float currentRotation = wheelTransform.eulerAngles.z;
        
        // Normalize rotation to 0-360
        while (currentRotation < 0) currentRotation += 360;
        while (currentRotation >= 360) currentRotation -= 360;
        
        // Calculate which segment is at the top
        float segmentAngle = 360f / 6f;
        int segmentIndex = Mathf.RoundToInt(currentRotation / segmentAngle) % 6;
        
        if (segments[segmentIndex] != null)
        {
            return segments[segmentIndex].GetColor();
        }
        
        return Color.white;
    }
    
    public void EnableInput()
    {
        inputEnabled = true;
    }
    
    public void DisableInput()
    {
        inputEnabled = false;
        isSpinning = false;
        targetSpinSpeed = 0f;
        isTouching = false;
    }
}