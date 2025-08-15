using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckCollisionLeft: MonoBehaviour
{
    private string colliderName = "";

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.transform.parent.name.Substring(0, 5) == "Piece")
        {
            colliderName = col.gameObject.transform.parent.name;
        }
        else
        {
            ResetVariables();
        }

    }

    void OnTriggerExit(Collider col)
    {
        ResetVariables();
    }

    public void ResetVariables()
    {
        colliderName = "";
    }

    public string GetCollision()
    {
        return colliderName;
    }
}
