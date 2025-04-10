using UnityEngine;

public class CheckPosition2 : MonoBehaviour
{
    // Infos about template piece 
    [SerializeField] FindRightTemplate script;
    private GameObject templatePiece = null;
    private GameObject mesh;
    private Material originalMaterial;

    // Infos about current piece
    private string pieceName;
    [SerializeField] GameObject proximity_sensor;
    private Collider ps_collider;

    // Suporting variables
    [SerializeField] Material highlightMaterial;
    Vector3 cp;
    private bool flag = false;
    private bool sameRot = false;
    private bool templateFound = false;
    private bool isChecking = false;
    float media;

    void OnEnable()
    {
        pieceName = this.gameObject.name;
        templatePiece = script.GetTemplate(pieceName);
        cp = templatePiece.transform.position;
        ps_collider = proximity_sensor.GetComponent<Collider>();
    }

    void Update()
    {

        if (templatePiece == null)
        {
            templatePiece = script.GetTemplate(pieceName);
            cp = templatePiece.transform.position;
        }

        if (proximity_sensor == null)
        {
            Debug.LogWarning("ProximitySensor não encontrado!");
            return;
        }

        if (!ps_collider)
        {
            ps_collider = proximity_sensor.GetComponent<Collider>();
        }
        else
        {
            if (ps_collider.bounds.Contains(cp))
            {
                script.ChangeTemplateMaterial(highlightMaterial);
            }
        }



    }

}
