using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeStats : MonoBehaviour
{
    private Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = this.GetComponent<Rigidbody>();
    }

    public void SetKinematicFalse()
    {
        rb.isKinematic = false;
    }

    public void SetKinematicTrue()
    {
        rb.isKinematic = true;
    }
}
