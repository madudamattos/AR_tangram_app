using UnityEngine;

public class P001 : MonoBehaviour
{
    bool choosen = false;

    void OnCollisionEnter(Collision col)
    {
        if(col.gameObject.name == "OVRHand")
        // se colidiu com a ponta da m�o, retorna true
        choosen = true;    
    }

}
