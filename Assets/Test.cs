using UnityEngine;
using System.Collections;
using UnityEngine.Events;

public class Test : MonoBehaviour
{
    public GameObject proximitySensor;
    public Transform obj;

    public UnityEvent onTouchEnter;  

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (proximitySensor.GetComponent<Collider>().bounds.Contains(obj.position))
        {
            onTouchEnter.Invoke();
        }

    }
}
