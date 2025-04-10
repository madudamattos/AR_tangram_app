using UnityEngine;

public class testFunction : MonoBehaviour
{
    private Material originalMat;
    public Material newMat;

    public GameObject Piece;
    Collider m_Collider;
    Vector3 m_Point;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        originalMat = GetComponent<Renderer>().material;
        m_Collider = GetComponent<Collider>();
        m_Point = Piece.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        m_Point = Piece.transform.position;

        if (m_Collider.bounds.Contains(m_Point))
        {
            Debug.Log("Bounds contain the point : " + m_Point);
            this.GetComponent<Renderer>().material = newMat;
        }
        else
        {

            this.GetComponent<Renderer>().material = originalMat;
        }
    }

    
}
