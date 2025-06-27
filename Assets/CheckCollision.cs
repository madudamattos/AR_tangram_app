using UnityEngine;

public class CheckCollision : MonoBehaviour
{
    private string colliderName = "";

    void OnTriggerEnter(Collider col)
    {
        Debug.Log("1:" + col.gameObject.transform.parent.name);
        Debug.Log(col.gameObject.transform.parent.name.Substring(0, 5));

        if (col.gameObject.transform.parent.name.Substring(0, 5) == "Piece")
        {
            colliderName = col.gameObject.transform.parent.name;
            Debug.Log("[CHECK COLLISION]: piece found: " + colliderName);
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
