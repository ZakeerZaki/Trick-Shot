using UnityEngine;

public class ObstacleMovable : MonoBehaviour
{
    private bool _isDragging = false;
    private Vector3 _offset;
    private Camera _mainCamera;
    private Vector2 _touchStartPos;

    public delegate void DraggingStateChanged(bool isDragging);
    public static event DraggingStateChanged OnDraggingStateChanged;

    private void Start()
    {
        _mainCamera = Camera.main;  // Get the main camera
    }

    private void Update()
    {
        HandleTouchInput();
    }

    private void HandleTouchInput()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            switch (touch.phase)
            {
                case TouchPhase.Began:
                    if (IsTouchOnObstacle(touch.position))
                    {
                        _isDragging = true;
                        OnDraggingStateChanged?.Invoke(_isDragging);

                        Vector3 screenPoint = _mainCamera.WorldToScreenPoint(transform.position);
                        _offset = transform.position - _mainCamera.ScreenToWorldPoint(new Vector3(touch.position.x, touch.position.y, screenPoint.z));
                        _touchStartPos = touch.position;
                    }
                    break;

                case TouchPhase.Moved:
                    if (_isDragging)
                    {
                        Vector2 touchDelta = touch.position - _touchStartPos;
                        Vector3 cursorPosition = _mainCamera.ScreenToWorldPoint(new Vector3(touch.position.x, touch.position.y, _mainCamera.WorldToScreenPoint(transform.position).z)) + _offset;

                        // Move the object along the x and z axes
                        transform.position = new Vector3(cursorPosition.x, transform.position.y, cursorPosition.z);
                        _touchStartPos = touch.position;
                    }
                    break;

                case TouchPhase.Ended:
                    if (_isDragging)
                    {
                        _isDragging = false;
                        OnDraggingStateChanged?.Invoke(_isDragging);
                    }
                    break;
            }
        }
    }

    private bool IsTouchOnObstacle(Vector2 touchPosition)
    {
        Ray ray = _mainCamera.ScreenPointToRay(touchPosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            return hit.transform == transform;
        }
        return false;
    }
}
