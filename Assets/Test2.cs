using UnityEngine;

public class Test2 : MonoBehaviour
{
    public Material mat;
    private Material mat_original;

    public void ChangeColor()
    {
        this.GetComponent<Renderer>().material = mat;
    }
}
