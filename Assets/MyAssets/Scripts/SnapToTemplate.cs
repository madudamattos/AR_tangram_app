using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnapToTemplate : MonoBehaviour
{
    [SerializeField] FindRightTemplate script_findTemplate;

    private GameObject templatePiece;
    private string templateName;

    private Material currentMaterial;
    private Rigidbody rb;

    [SerializeField] Material highlightMaterial;

    // Start is called before the first frame update
    void Start()
    {
        rb = this.GetComponent<Rigidbody>();
    }

    void OnEnable()
    {
        templatePiece = script_findTemplate.GetTemplate(this.gameObject.name);
        templateName = templatePiece.name;
    }


    public void HighLightTemplate()
    {
        
        Renderer templateRenderer = templatePiece.GetComponent<Renderer>();
        currentMaterial = templateRenderer.material;
        templateRenderer.material = highlightMaterial;
    }

    public void ChangeBackMaterial()
    {
        Renderer templateRenderer = templatePiece.GetComponent<Renderer>();
        templateRenderer.material = currentMaterial;
    }

    void OnTriggerStay(Collider col)
    {
        if(col.gameObject.name == templateName)
        {
           if (!rb.isKinematic) rb.isKinematic = true;
        }
    }

    void OnTriggerExit(Collider col)
    {
        if (col.gameObject.name == templateName)
        {
            rb.isKinematic = false;
            templatePiece.gameObject.GetComponent<MeshRenderer>().enabled = true;
        }
    }

}
