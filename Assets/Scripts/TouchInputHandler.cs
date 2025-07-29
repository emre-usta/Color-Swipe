using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class SwipeEvent : UnityEvent<Vector2, Vector2, float> { }

public class TouchInputHandler : MonoBehaviour
{
    [Header("Swipe Settings")]
    public float minSwipeDistance = 100f;
    public float maxSwipeTime = 1f;
    
    [Header("Events")]
    public SwipeEvent OnSwipe;
    public UnityEvent OnTouchStart;
    public UnityEvent OnTouchEnd;
    
    private Vector2 startTouchPosition;
    private float startTouchTime;
    private bool isSwiping = false;
    
    void Update()
    {
        HandleInput();
    }
    
    void HandleInput()
    {
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
            
            switch (touch.phase)
            {
                case TouchPhase.Began:
                    StartSwipe(touch.position);
                    break;
                    
                case TouchPhase.Ended:
                case TouchPhase.Canceled:
                    EndSwipe(touch.position);
                    break;
            }
        }
    }
    
    void HandleMouseInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            StartSwipe(Input.mousePosition);
        }
        else if (Input.GetMouseButtonUp(0))
        {
            EndSwipe(Input.mousePosition);
        }
    }
    
    void StartSwipe(Vector2 position)
    {
        startTouchPosition = position;
        startTouchTime = Time.time;
        isSwiping = true;
        OnTouchStart?.Invoke();
    }
    
    void EndSwipe(Vector2 endPosition)
    {
        if (!isSwiping) return;
        
        isSwiping = false;
        OnTouchEnd?.Invoke();
        
        float swipeTime = Time.time - startTouchTime;
        float swipeDistance = Vector2.Distance(startTouchPosition, endPosition);
        
        if (swipeDistance >= minSwipeDistance && swipeTime <= maxSwipeTime)
        {
            Vector2 swipeDirection = (endPosition - startTouchPosition).normalized;
            OnSwipe?.Invoke(startTouchPosition, endPosition, swipeDistance);
        }
    }
}