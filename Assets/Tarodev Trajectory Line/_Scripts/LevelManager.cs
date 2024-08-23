using System.Collections;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private GameObject[] levelPrefabs;
    [SerializeField] private Vector3[] levelPositions;
    private int _currentLevelIndex = 0;
    private GameObject _currentLevel;
    private Projection _projection;  // Reference to the Projection script

    private void Start()
    {
        _projection = FindObjectOfType<Projection>();  // Find the Projection component in the scene

        if (levelPrefabs.Length != levelPositions.Length)
        {
            Debug.LogError("The number of level prefabs does not match the number of positions.");
            return;
        }
        LoadLevel(_currentLevelIndex);
    }

    private void LoadLevel(int levelIndex)
    {
        if (_currentLevel != null)
        {
            SetGuidePositionsInactive(_currentLevel);
            StartCoroutine(DestroyAndLoadNextLevel(levelIndex));
        }
        else
        {
            _currentLevel = Instantiate(levelPrefabs[levelIndex], levelPositions[levelIndex], Quaternion.identity);
            // Turn on the projection for the new level
            if (_projection != null)
            {
                _projection.SetProjectionActive(true);
            }
        }
    }

    private IEnumerator DestroyAndLoadNextLevel(int levelIndex)
    {
        // Wait for 5 seconds before destroying the current level
        yield return new WaitForSeconds(2f);

        Destroy(_currentLevel);
        _currentLevel = Instantiate(levelPrefabs[levelIndex], levelPositions[levelIndex], Quaternion.identity);

        // Turn on the projection for the new level
        if (_projection != null)
        {
            _projection.SetProjectionActive(true);
        }
    }

    public void OnLevelComplete()
    {
        _currentLevelIndex++;

        if (_currentLevelIndex < levelPrefabs.Length)
        {
            LoadLevel(_currentLevelIndex);
            Debug.Log(_currentLevelIndex.ToString());
        }
        else
        {
            Debug.Log("All levels completed!");
        }
    }

    private void SetGuidePositionsInactive(GameObject level)
    {
        Transform guidePositionsParent = level.transform.Find("GuidePositions");
        if (guidePositionsParent != null)
        {
            foreach (Transform guide in guidePositionsParent)
            {
                guide.gameObject.SetActive(false);
            }
        }
    }
}
