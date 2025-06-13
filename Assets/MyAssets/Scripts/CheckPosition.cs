using UnityEngine;
using System.Collections;
using System.Collections.Generic;
// using UnityEditor; 

[RequireComponent(typeof(FindRightTemplate))]
public class CheckPosition : FindRightTemplate
{
    // Infos about template piece 
    private GameObject mesh;
    [SerializeField] Material highlightMaterial;

    // Suporting variables
    private bool templateFound = false;
    protected bool isChecking = false;
    protected Queue<bool> queue = new Queue<bool>();

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

    bool VerifyStartConditions()
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

        if (templatePointsList == null)
        {
            Debug.Log("Template points list is null");
            return false;
        }

        Debug.Log("Everything is allright to start");

        return true;
    }

    bool CheckTemplatePosition()
    {
        if (pieceCollidersList.Count < 4 || templatePointsList.Count < 4)
        {
            Debug.LogWarning("Lista de colliders ou pontos está incompleta.");
            return false;
        }

        if (pieceCollidersList[0].bounds.Contains(templatePointsList[0].position) && pieceCollidersList[1].bounds.Contains(templatePointsList[1].position) &&
            pieceCollidersList[2].bounds.Contains(templatePointsList[2].position) &&
            pieceCollidersList[3].bounds.Contains(templatePointsList[3].position))
        {
            return true;
        }

        return false;
    }

    protected IEnumerator IsSamePosition()
    {
        isChecking = true;

        yield return new WaitForSeconds(.5f);

        isChecking = false;

        int total = 0;
        int totalTrue = 0;

        while (queue.Count > 0)
        {
            bool item = queue.Dequeue();
            total++;
            if (item) totalTrue++;
        }

        if (total > 0)
        {
            float media = (float) totalTrue / total;

            if (media > 0.75f)
            {
                base.ChangeTemplateMaterial(highlightMaterial);
                base.ActivateTemplateMesh();
                Invoke(nameof(ChangeTemplate), 1.0f);
                templateFound = true;
            }
        }
    }

    public bool TemplateFound()
    {
        return templateFound;
    }

    public void ChangeTemplate()
    {
        base.DeactivateTemplateMesh();
        base.ChangeTemplateMaterial(null);
    }
}
