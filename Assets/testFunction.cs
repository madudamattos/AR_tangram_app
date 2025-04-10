using UnityEngine;

public class testFunction : MonoBehaviour
{
    private Material originalMat;
    public Material newMat;

    public GameObject Piece;
    private GameObject proximitySensor;

    Collider m_Collider;
    Vector3 m_Point;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        proximitySensor = this.transform.Find("ProximitySensor").gameObject;
        m_Collider = proximitySensor.GetComponent<Collider>();
        m_Point = Piece.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        m_Point = Piece.transform.position;

        if(proximitySensor == null)
        {
            print("Proximity sensor not found");
            return;
        }

        if (m_Collider.bounds.Contains(m_Point))
        {
            Debug.Log("Bounds contain the point : " + m_Point);
        } else
        {
            Debug.Log("Bounds does NOT contain the point: " + m_Point);
        }

    }

    
}
