using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindRightTemplate : MonoBehaviour
{
    private GameObject templatePiece = null;
    private Material originalMat;

    public GameObject GetTemplate()
    {
        string pieceName = this.gameObject.name;
      
        string pieceNumber = pieceName.Substring((pieceName.Length - 3), 3);
      
        string templateName = "Template" + "." + pieceNumber;

        templatePiece = GameObject.Find(templateName);

        originalMat = templatePiece.transform.Find("Mesh").gameObject.GetComponent<Renderer>().material;

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
