using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindRightTemplate : MonoBehaviour
{
    private GameObject templatePiece = null;
    private Material originalMat;

    string templateName = "";

    public GameObject GetTemplate(string pieceName)
    {
        string pieceNumber = pieceName.Substring((pieceName.Length - 3), 3);
        
        templateName = "Template" + "." + pieceNumber;

        templatePiece = GameObject.Find(templateName);

        originalMat = templatePiece.transform.Find("Mesh").gameObject.GetComponent<Renderer>().material;

        return templatePiece;
    }

    public void ChangeTemplateMaterial(Material mat)
    { 
        Renderer templateRenderer = templatePiece.transform.Find("Mesh").gameObject.GetComponent<Renderer>();
        templateRenderer.material = mat ? mat : originalMat;
    }

}
