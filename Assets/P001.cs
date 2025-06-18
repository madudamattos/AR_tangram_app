using UnityEngine;

public class P001 : MonoBehaviour
{
    bool choosen = false;

    void OnCollisionEnter(Collision col)
    {
        if(col.gameObject.name == "OVRHand")
        // se colidiu com a ponta da mão, retorna true
        choosen = true;    
    }

}
