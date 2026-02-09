using UnityEngine;

public class Spin : MonoBehaviour
{
    [SerializeField] public float rotation = 10f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0f, rotation * Time.deltaTime, 0f, Space.Self);
    }
}
