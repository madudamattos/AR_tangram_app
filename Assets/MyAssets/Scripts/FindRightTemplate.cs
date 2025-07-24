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

    /*    public GameObject FindTemplate()
        {
            pieceName = this.gameObject.name;

            string pieceNumber = pieceName.Substring((pieceName.Length - 3), 3);

            string templateName = "Template" + "." + pieceNumber;

            templatePiece = GameObject.Find(templateName);

            originalMat = templatePiece.transform.Find("Mesh").gameObject.GetComponent<Renderer>().material;

            return templatePiece;
        }*/

    public GameObject FindTemplate()
    {
        pieceName = this.gameObject.name;
        string pieceNumber = pieceName.Substring((pieceName.Length - 3), 3);
        string templateName = "Template." + pieceNumber;

        GameObject found = GameObject.Find(templateName);

        if (found == null)
        {
            Debug.LogError($"[FindTemplate] Template '{templateName}' não encontrado.");
            return null;
        }

        Transform mesh = found.transform.Find("Mesh");
        if (mesh == null)
        {
            Debug.LogError($"[FindTemplate] O objeto '{templateName}' não contém um filho chamado 'Mesh'.");
            return found;
        }

        Renderer r = mesh.GetComponent<Renderer>();
        if (r == null)
        {
            Debug.LogError($"[FindTemplate] O objeto '{mesh.name}' não possui um componente Renderer.");
        }
        else
        {
            originalMat = r.material;
        }

        return found;
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
