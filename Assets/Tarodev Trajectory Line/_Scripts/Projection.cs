using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Projection : MonoBehaviour
{
    [SerializeField] private LineRenderer _line;
    [SerializeField] private int _maxPhysicsFrameIterations = 100;
    [SerializeField] private Transform _obstaclesParent;

    private Scene _simulationScene;
    private PhysicsScene _physicsScene;
    private readonly Dictionary<Transform, Transform> _spawnedObjects = new Dictionary<Transform, Transform>();
    private bool _isProjectionActive = true; // Default to true

    private void Start()
    {
        CreatePhysicsScene();
        _line.enabled = true; // Ensure the LineRenderer is on by default
    }

    private void CreatePhysicsScene()
    {
        _simulationScene = SceneManager.CreateScene("Simulation", new CreateSceneParameters(LocalPhysicsMode.Physics3D));
        _physicsScene = _simulationScene.GetPhysicsScene();

        foreach (Transform obj in _obstaclesParent)
        {
            var ghostObj = Instantiate(obj.gameObject, obj.position, obj.rotation);
            ghostObj.GetComponent<Renderer>().enabled = false;
            SceneManager.MoveGameObjectToScene(ghostObj, _simulationScene);
            if (!ghostObj.isStatic) _spawnedObjects.Add(obj, ghostObj.transform);
        }
    }

    private void Update()
    {
        foreach (var item in _spawnedObjects)
        {
            item.Value.position = item.Key.position;
            item.Value.rotation = item.Key.rotation;
        }
    }

    public void SimulateTrajectory(Ball ballPrefab, Vector3 pos, Vector3 velocity)
    {
        if (!_isProjectionActive) return; // Only simulate if projection is active

        _line.enabled = true;

        var ghostObj = Instantiate(ballPrefab, pos, Quaternion.identity);
        SceneManager.MoveGameObjectToScene(ghostObj.gameObject, _simulationScene);

        ghostObj.Init(velocity, true);

        _line.positionCount = _maxPhysicsFrameIterations;
        int frameCount = 0;
        Vector3 lastPosition = ghostObj.transform.position;

        for (frameCount = 0; frameCount < _maxPhysicsFrameIterations; frameCount++)
        {
            _physicsScene.Simulate(Time.fixedDeltaTime);

            if (HasCollision(ghostObj, out Vector3 collisionPoint))
            {
                _line.positionCount = frameCount + 1;
                _line.SetPosition(frameCount, collisionPoint);
                break;
            }

            _line.SetPosition(frameCount, ghostObj.transform.position);
            lastPosition = ghostObj.transform.position;
        }

        if (frameCount == _maxPhysicsFrameIterations)
        {
            _line.positionCount = _maxPhysicsFrameIterations;
            _line.SetPosition(frameCount - 1, lastPosition);
        }

        Destroy(ghostObj.gameObject);
    }

    private bool HasCollision(Ball ball, out Vector3 collisionPoint)
    {
        Collider[] colliders = Physics.OverlapSphere(ball.transform.position, 0.1f);
        foreach (Collider collider in colliders)
        {
            if (collider.gameObject != ball.gameObject)
            {
                collisionPoint = ball.transform.position;
                return true;
            }
        }
        collisionPoint = Vector3.zero;
        return false;
    }

    // Method to toggle the projection on or off
    public void SetProjectionActive(bool active)
    {
        _isProjectionActive = active;
        _line.enabled = active; // Enable or disable the line renderer accordingly
    }

    // Check if the projection is currently active
    public bool IsProjectionActive()
    {
        return _isProjectionActive;
    }
}
