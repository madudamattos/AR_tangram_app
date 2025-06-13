using UnityEngine;
using System.Collections;
using System.Collections.Generic;
// using UnityEditor;

[RequireComponent(typeof(CheckPosition))]
public class Piece005 : CheckPosition
{
    // Infos about template Piece
    [SerializeField] List<Transform> templatePointsList = new List<Transform>();

    // Infos about current piece
    [SerializeField] List<Collider> pieceCollidersList = new List<Collider>();

    // Suporting variables
    private bool flag = false;
    
    protected override void Start()
    {
        base.Start();       
    }

    void Update()
    {
        if (!VerifyStartConditions())
        {
            Debug.Log("Start conditions did not match. Returning.");
            return;
        }

        // Game loop
        flag = CheckTemplatePosition();

        if (flag && !isChecking)
        {
            StartCoroutine(IsSamePosition());
        }

        if (isChecking)
        {
            queue.Enqueue(flag);
        }
    }

    protected override bool CheckTemplatePosition()
    {
        if (pieceCollidersList.Count < 4 || templatePointsList.Count < 4)
        {
            Debug.LogWarning("Lista de colliders ou pontos está incompleta.");
            return false;
        }

        if (pieceCollidersList[0].bounds.Contains(templatePointsList[0].position) &&         pieceCollidersList[1].bounds.Contains(templatePointsList[1].position) && 
            pieceCollidersList[2].bounds.Contains(templatePointsList[2].position) &&
            pieceCollidersList[3].bounds.Contains(templatePointsList[3].position))
        {
            return true;
        }

        return false;
    }

    protected override bool VerifyStartConditions()
    {
        if (templateFound) return false;

        // verifing variables
        if (templatePiece == null)
        {
            Debug.Log("Template piece is null");
            templatePiece = base.GetTemplate();
            return false;
        }

        // verify if exst
        if (pieceCollidersList == null)
        {
            Debug.Log("Piece collider list is null");
            return false;
        }

        if(templatePointsList == null)
        {
            Debug.Log("Template points list is null");
            return false;
        }

        Debug.Log("Everything is allright to start");

        return true;
    }
}
