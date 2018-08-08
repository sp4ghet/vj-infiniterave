using UnityEngine;

public class MoveAndRotateObject : MonoBehaviour
{
    [SerializeField] float speed = 180;
    private float angle_ = 0f;
    private Vector3 axis_;

    void Start()
    {
        axis_ = Random.insideUnitSphere;
    }

    void Update()
    {
        angle_ += speed * Time.deltaTime;
        transform.rotation = Quaternion.AngleAxis(angle_, axis_);
        transform.position = new Vector3(
            Mathf.Sin(Time.time * Mathf.PI * 0.5f),
            0.4f * Mathf.Cos(Time.time * Mathf.PI * 0.2f),
            0.2f * Mathf.Cos(Time.time * Mathf.PI * 0.3f));
    }
}