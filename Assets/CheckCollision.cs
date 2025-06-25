using UnityEngine;

public class CheckCollision : MonoBehaviour
{
    public string colliderName = "";
    public bool collide = false;

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.transform.parent.name.Substring(0, 4)  == "Piece")
        {
            colliderName = col.gameObject.transform.parent.name;
            collide = true;

            // callback event
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
        collide = false;
    }
}
