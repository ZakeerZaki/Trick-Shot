using UnityEngine;

public class Ball : MonoBehaviour
{
    [SerializeField] private Rigidbody _rb;
    [SerializeField] private AudioSource _source;
    [SerializeField] private AudioClip[] _clips;
    [SerializeField] private GameObject _poofPrefab;
    [SerializeField] private float _jumpForce = 5f;
    [SerializeField] private float _forwardForceMultiplier = 10f;

    private Transform[] _targetInstances;
    private int _currentTargetIndex = 0;
    private bool _isGhost;
    [SerializeField] private float destroyDelay = 1.0f;

    private LevelManager levelManager;
    private ScoreManager scoreManager;
    private Projection _projection;  // Reference to the Projection script

    private void Start()
    {
        _targetInstances = FindObjectOfType<ObstacleManager>().GetTargetInstances();
        scoreManager = FindObjectOfType<ScoreManager>(); // Find the ScoreManager in the scene
        _projection = FindObjectOfType<Projection>();    // Find the Projection component in the scene
    }

    private void Awake()
    {
        levelManager = FindObjectOfType<LevelManager>();
    }

    public void Init(Vector3 velocity, bool isGhost)
    {
        _isGhost = isGhost;
        _rb.AddForce(velocity, ForceMode.Impulse);

        // Decrease score by 2 when the ball is shot
        /*if (scoreManager != null)
        {
            scoreManager.SubtractScore(2);
        }*/
    }

    public void OnCollisionEnter(Collision col)
    {
        if (_isGhost) return;

        Instantiate(_poofPrefab, col.contacts[0].point, Quaternion.Euler(col.contacts[0].normal));
        _source.clip = _clips[Random.Range(0, _clips.Length)];
        _source.Play();

        if (col.transform == _targetInstances[_currentTargetIndex])
        {
            _currentTargetIndex++;

            if (_currentTargetIndex < (_targetInstances.Length - 1))
            {
                Vector3 directionToNextTarget = (_targetInstances[_currentTargetIndex].position - transform.position).normalized;

                _rb.velocity = Vector3.zero;
                Vector3 jumpForce = new Vector3(0, _jumpForce, 0);
                Vector3 forwardForce = directionToNextTarget * _forwardForceMultiplier;

                _rb.AddForce(jumpForce + forwardForce, ForceMode.Impulse);
            }
        }

        if (col.gameObject.CompareTag("Cup"))
        {
            if (levelManager != null)
            {
                levelManager.OnLevelComplete();
            }

            // Increase score by 10 when the ball hits the cup
            if (scoreManager != null)
            {
                scoreManager.AddScore(10);
            }

            // Turn the projection back on for the next level
            if (_projection != null)
            {
                _projection.SetProjectionActive(true);
            }

            Invoke(nameof(DestroyBall), destroyDelay);
        }
    }

    private void DestroyBall()
    {
        Destroy(gameObject);
    }
}