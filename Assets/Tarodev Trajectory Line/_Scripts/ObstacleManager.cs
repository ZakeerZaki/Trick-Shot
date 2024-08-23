using UnityEngine;

public class ObstacleManager : MonoBehaviour
{
    [SerializeField] private GameObject[] obstaclePrefabs;  // List of obstacle prefabs
    [SerializeField] private Vector3[] obstaclePositions;  // List of positions for each obstacle
    [SerializeField] private Vector3[] obstacleRotations;  // List of rotations for each obstacle
    [SerializeField] private GameObject guidePrefab;  // Prefab for the position guides
    [SerializeField] private Vector3[] guidePositions;  // List of positions for each guide
    [SerializeField] private Transform cupTarget;  // The final cup target
    [SerializeField] private Transform _parentObject;

    private Transform[] targetInstances;  // Array to hold the instantiated obstacle instances
    private Transform[] guideInstances;  // Array to hold the instantiated guide instances

    private void Start()
    {
        // Ensure the number of positions matches the number of prefabs
        if (obstaclePrefabs.Length != obstaclePositions.Length || obstaclePrefabs.Length != obstacleRotations.Length)
        {
            Debug.LogError("The number of obstacle prefabs does not match the number of positions or rotations.");
            return;
        }

        // Instantiate all obstacle prefabs at the specified positions and rotations
        targetInstances = new Transform[obstaclePrefabs.Length + 1];  // +1 for the cup target
        for (int i = 0; i < obstaclePrefabs.Length; i++)
        {
            GameObject instance = Instantiate(obstaclePrefabs[i], obstaclePositions[i], Quaternion.Euler(obstacleRotations[i]), _parentObject);
            targetInstances[i] = instance.transform;
        }

        // Add the cup target as the final destination
        targetInstances[obstaclePrefabs.Length] = cupTarget;

        // Instantiate the guide prefabs at the specified positions
        guideInstances = new Transform[guidePositions.Length];
        for (int i = 0; i < guidePositions.Length; i++)
        {
            GameObject guideInstance = Instantiate(guidePrefab, guidePositions[i], Quaternion.identity);
            guideInstances[i] = guideInstance.transform;
        }
    }

    // Return the array of guide instances for snapping
    public Transform[] GetGuideInstances()
    {
        return guideInstances;
    }

    // Optionally, create a method to return the array of target instances
    public Transform[] GetTargetInstances()
    {
        return targetInstances;
    }
}