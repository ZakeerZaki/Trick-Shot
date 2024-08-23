using UnityEngine;

public class SnapToGuide : MonoBehaviour
{
    [SerializeField] private float _snapThreshold = 1.0f;
    private Transform[] _guideInstances;

    private void Start()
    {
        _guideInstances = FindObjectOfType<ObstacleManager>().GetGuideInstances();
    }

    private void Update()
    {
        SnapToClosestGuide();
    }

    private void SnapToClosestGuide()
    {
        foreach (var guide in _guideInstances)
        {
            float distance = Vector3.Distance(transform.position, guide.position);
            if (distance < _snapThreshold)
            {
                guide.gameObject.SetActive(true);
                if (distance < 0.2f)
                {
                    transform.position = guide.position;
                    guide.gameObject.SetActive(false);
                }
            }
            else
            {
                guide.gameObject.SetActive(false);
            }
        }
    }
}
