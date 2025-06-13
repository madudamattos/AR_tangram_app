using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindRightTemplate : MonoBehaviour
{
    protected GameObject templatePiece = null;
    private Material originalMat;
    protected string pieceName = null;

    protected virtual void Start()
    {
        templatePiece = FindTemplate();
    }

    public GameObject FindTemplate()
    {
        pieceName = this.gameObject.name;
      
        string pieceNumber = pieceName.Substring((pieceName.Length - 3), 3);
      
        string templateName = "Template" + "." + pieceNumber;

        templatePiece = GameObject.Find(templateName);

        originalMat = templatePiece.transform.Find("Mesh").gameObject.GetComponent<Renderer>().material;

        return templatePiece;
    }

    public GameObject GetTemplate()
    {
        return templatePiece;
    }

    public void ChangeTemplateMaterial(Material mat)
    {
        Renderer templateRenderer = this.templatePiece.transform.Find("Mesh").gameObject.GetComponent<Renderer>();
        templateRenderer.material = mat ? mat : originalMat;
    }

    public void ActivateTemplateMesh()
    {
        this.templatePiece.transform.Find("Mesh").gameObject.GetComponent<MeshRenderer>().enabled = true;
    }

    public void DeactivateTemplateMesh()
    {
        this.templatePiece.transform.Find("Mesh").gameObject.GetComponent<MeshRenderer>().enabled = false;
    }

}
