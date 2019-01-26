using UnityEngine;

public class FollowRotation : MonoBehaviour
{
    [SerializeField] private Transform _targetTransform = null;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void LateUpdate()
    {
        transform.rotation = _targetTransform.rotation;
    }
}
