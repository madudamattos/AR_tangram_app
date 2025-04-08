using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindRightTemplate : MonoBehaviour
{
    private GameObject templatePiece;

    string templateName = "";

    public GameObject GetTemplate(string pieceName)
    {
        string pieceNumber = pieceName.Substring((pieceName.Length - 3), 3);
        
        templateName = "Template" + "." + pieceNumber;

        templatePiece = GameObject.Find(templateName);
    
        return templatePiece;
    }

}
