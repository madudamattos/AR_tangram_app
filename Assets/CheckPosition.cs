using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CheckPosition : MonoBehaviour
{
    [SerializeField] FindRightTemplate script;
    private GameObject templatePiece;
    private string pieceName;

    [SerializeField] Material originalMaterial;
    [SerializeField] Material highlightMaterial;

    bool samePos = false;
    bool sameRot = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void OnEnable()
    {
        pieceName = this.gameObject.name;
    }

    // Update is called once per frame
    void Update()
    {
        if (templatePiece != null)
        {
            // print("IN LOOP");
            samePos = isSamePosition(templatePiece, this.gameObject);
            sameRot = isSameRotation(templatePiece, this.gameObject);
            if (pieceName == "Piece.005")
            {
                print(pieceName);
                print("pos: " + this.gameObject.transform.position);
                print("pos template: " + templatePiece.transform.position);
                if (samePos) print("SAME POSITION");
                print("rot: " + this.gameObject.transform.rotation.eulerAngles);
                print("rot template: " + templatePiece.transform.rotation.eulerAngles);
                if (sameRot) print("SAME ROTATION");
            }

            if (samePos)
            {
                templatePiece.transform.GetChild(0).gameObject.GetComponent<Renderer>().material = highlightMaterial;
            }
            else
            {
                templatePiece.transform.GetChild(0).gameObject.GetComponent<Renderer>().material = originalMaterial;
            }

        } else
        {
            templatePiece = script.GetTemplate(pieceName);
            if (templatePiece != null)
            {
                pieceName = this.gameObject.name;
            }
            
        }

    }

    public bool isSamePosition(GameObject template, GameObject piece)
    {
        Vector3 offset = new Vector3(0.01f, 0.01f, 0.5f);

        float diff_x = Mathf.Abs(template.transform.position.x - piece.transform.position.x);
        float diff_y = Mathf.Abs(template.transform.position.y - piece.transform.position.y);
        float diff_z = Mathf.Abs(template.transform.position.z - piece.transform.position.z);

        return diff_x < offset.x && diff_y < offset.y && diff_z < offset.z;
    }

    public bool isSameRotation(GameObject template, GameObject piece)
    {
        // offset rotacao 10 graus 
        Vector3 offset = new Vector3(15, 15, 15);

        Vector3 templateEuler = template.transform.rotation.eulerAngles;
        Vector3 pieceEuler = piece.transform.rotation.eulerAngles;

        // Calculara diferença com Mathf.DeltaAngle garante que a diferença em angulos seja sempre entre [-180 e 180], evitando problemas quando os valores passam de 360 para 0
        float diff_x = Mathf.Abs(Mathf.DeltaAngle(templateEuler.x, pieceEuler.x));
        float diff_y = Mathf.Abs(Mathf.DeltaAngle(templateEuler.y, pieceEuler.y));
        float diff_z = Mathf.Abs(Mathf.DeltaAngle(templateEuler.z, pieceEuler.z));


        return diff_z < offset.z;
    }
}
