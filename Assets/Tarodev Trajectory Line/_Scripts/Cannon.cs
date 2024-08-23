using UnityEngine;

public class Cannon : MonoBehaviour
{
    [SerializeField] private Projection _projection;

    private bool _isDraggingObstacles = false;
    private Vector2 _touchStartPos;    // Declare _touchStartPos
    private Vector2 _touchCurrentPos;  // Declare _touchCurrentPos
    private bool _isDragging = false;  // Declare _isDragging

    [SerializeField] private Ball _ballPrefab;
    [SerializeField] private float _force = 20;
    [SerializeField] private Transform _ballSpawn;
    [SerializeField] private Transform _barrelPivot;
    [SerializeField] private float _rotateSpeed = 5f;
    [SerializeField] public ScoreManager scoreManager;

    private void OnEnable()
    {
        ObstacleMovable.OnDraggingStateChanged += HandleDraggingStateChanged;
    }

    private void OnDisable()
    {
        ObstacleMovable.OnDraggingStateChanged -= HandleDraggingStateChanged;
    }

    private void Update()
    {
        if (!_isDraggingObstacles)
        {
            HandleTouchInput();
            if (_projection.IsProjectionActive())
            {
                _projection.SimulateTrajectory(_ballPrefab, _ballSpawn.position, _ballSpawn.forward * _force);
            }
        }
    }

    private void HandleDraggingStateChanged(bool isDragging)
    {
        _isDraggingObstacles = isDragging;
        if (isDragging)
        {
            _projection.SetProjectionActive(false); // Turn off projection when dragging starts
        }
        else
        {
            _projection.SetProjectionActive(true); // Turn on projection when dragging ends
        }
    }

    private void HandleTouchInput()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            switch (touch.phase)
            {
                case TouchPhase.Began:
                    _touchStartPos = touch.position;
                    _isDragging = true;
                    break;

                case TouchPhase.Moved:
                    if (_isDragging)
                    {
                        _touchCurrentPos = touch.position;
                        Vector2 touchDelta = _touchCurrentPos - _touchStartPos;

                        float horizontalRotation = touchDelta.x * _rotateSpeed * Time.deltaTime * 0.1f;
                        transform.Rotate(Vector3.up, horizontalRotation);

                        float verticalRotation = -touchDelta.y * _rotateSpeed * Time.deltaTime * 0.1f;
                        _barrelPivot.Rotate(Vector3.right, verticalRotation);

                        _touchStartPos = _touchCurrentPos;
                    }
                    break;

                case TouchPhase.Ended:
                    _isDragging = false;
                    FireCannon();
                    break;
            }
        }
    }

    private void FireCannon()
    {
        var spawned = Instantiate(_ballPrefab, _ballSpawn.position, _ballSpawn.rotation);
        spawned.Init(_ballSpawn.forward * _force, false);

        /*if (scoreManager != null)
        {
            scoreManager.SubtractScore(2);
        }*/
    }
}
