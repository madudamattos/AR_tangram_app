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

        originalMat = templatePiece.transform.Find("Mesh_1").gameObject.GetComponent<Renderer>().material;

        return templatePiece;
    }

    public GameObject GetTemplate()
    {
        return templatePiece;
    }

    public void ChangeTemplateMaterial(Material mat)
    {
        Renderer templateRenderer = this.templatePiece.transform.Find("Mesh_1").gameObject.GetComponent<Renderer>();
        templateRenderer.material = mat ? mat : originalMat;


        if (this.templatePiece.transform.Find("Mesh_2") != null)
        {
            templateRenderer = this.templatePiece.transform.Find("Mesh_2").gameObject.GetComponent<Renderer>();
            templateRenderer.material = mat ? mat : originalMat;
        }
    }

    public void ActivateTemplateMesh(int meshNumber = 1)
    {
        if (meshNumber == 1)
        {
            this.templatePiece.transform.Find("Mesh_1").gameObject.GetComponent<MeshRenderer>().enabled = true;
        } 
        else if(meshNumber == 2)
        {
            this.templatePiece.transform.Find("Mesh_2").gameObject.GetComponent<MeshRenderer>().enabled = true;
        }
        
    }

    public void DeactivateTemplateMesh()
    {
       this.templatePiece.transform.Find("Mesh_1").gameObject.GetComponent<MeshRenderer>().enabled = false;
       
       if(this.templatePiece.transform.Find("Mesh_2") != null)
       {
            this.templatePiece.transform.Find("Mesh_2").gameObject.GetComponent<MeshRenderer>().enabled = false;
       }
    }
    
    public bool GetTemplateMeshActive(int meshNumber = 1)
    {
        if (meshNumber == 1)
        {
            return this.templatePiece.transform.Find("Mesh_1").gameObject.GetComponent<MeshRenderer>().enabled;
        }
        else if (meshNumber == 2)
        {
            return this.templatePiece.transform.Find("Mesh_2").gameObject.GetComponent<MeshRenderer>().enabled;
        }
        else return false;
    }

    public GameObject GetTemplateMesh(int meshNumber = 1)
    {
        return this.templatePiece.transform.Find($"Mesh_{meshNumber}").gameObject;
    }

    // bota o transform desejado no 2º mesh para peças iguais
    public void SetTemplateMeshTransform(Transform transform, int meshNumber = 2)
    {
        this.templatePiece.transform.Find($"Mesh_{meshNumber}").gameObject.transform.position = transform.position;
        this.templatePiece.transform.Find($"Mesh_{meshNumber}").gameObject.transform.rotation = transform.rotation;
        this.templatePiece.transform.Find($"Mesh_{meshNumber}").gameObject.transform.localScale = transform.localScale;
    }


}
