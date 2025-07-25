using UnityEngine;
using System.Collections;
using System.Collections.Generic;
// using UnityEditor; 

[RequireComponent(typeof(FindRightTemplate))]
public class CheckPosition : FindRightTemplate
{
    // Infos about template piece 
    private GameObject mesh;

    // Suporting variables
    private bool templateFound = false;
    protected bool isChecking = false;
    protected Queue<int> queue = new Queue<int>();
    private int flag = 0;

    // Scripts
    [SerializeField] ControlScene _controlScene;


    // Infos about current piece
    [Header("Piece Sensors Reference")]
    [SerializeField] List<Collider> pieceCollidersList = new List<Collider>();

    // Infos about template Piece
    [Header("Template Points Reference")]
    [SerializeField] List<Transform> templatePointsList = new List<Transform>();

    [Header("Optional Template Points Reference")]
    [SerializeField] List<Transform> optionalTemplatePoints = new List<Transform>();


    // Assets
    [Header("Extra Assets")]
    [SerializeField] private AudioSource soundPieceFound;
    [SerializeField] Material highlightMaterial;

    protected override void Start()
    {
        base.Start();
        pieceName = base.pieceName;
    }

    void Update()
    {
        if (!VerifyStartConditions())
        {
            // Debug.Log("[CHECKPOSITION]: Start conditions did not match. Returning.");
            return;
        }

        // Game loop
        flag = CheckTemplatePosition();

        if (flag != 0 && !isChecking)
        {
            StartCoroutine(IsSamePosition(flag));
        }

        if (isChecking)
        {
            queue.Enqueue(flag);
        }
    }

    bool VerifyStartConditions()
    {
        if (_controlScene.GetGameLoop() == false) return false;
        if (templateFound) return false;

        // verifing variables
        if (templatePiece == null)
        {
            Debug.Log("[CHECKPOSITION]: Template piece is null");
            templatePiece = base.FindTemplate();
            return false;
        }

        if (pieceName == null)
        {
            Debug.Log("[CHECKPOSITION]: Piecename is null");
            return false;
        }

        // verify if exst
        if (pieceCollidersList == null)
        {
            Debug.Log("[CHECKPOSITION]: Piece collider list is null");
            return false;
        }

        if (templatePointsList == null || templatePointsList.Count == 0)
        {
            Debug.Log("[CHECKPOSITION]: Template points list is null or empty");
            return false;
        }

        return true;
    }

    // 0 = não achou nada / 1 = achou posicao default / 2 = achou posicao secundaria
    int CheckTemplatePosition()
    {
        if (pieceCollidersList.Count < 4 || templatePointsList.Count < 4)
        {
            Debug.Log("[CHECKPOSITION]: Colliders list or template points list for piece 005 is incomplete. Returning false");
            return 0;
        }

        if (pieceName == "Piece.001" || pieceName == "Piece.002" || pieceName == "Piece.005" || pieceName == "Piece.003" || pieceName == "Piece.006")
        {
            if (pieceCollidersList[1].bounds.Contains(templatePointsList[1].position) &&
               pieceCollidersList[2].bounds.Contains(templatePointsList[2].position) &&
               pieceCollidersList[3].bounds.Contains(templatePointsList[3].position))
                return 1;
            else if (pieceCollidersList[1].bounds.Contains(templatePointsList[1].position) &&
                    pieceCollidersList[3].bounds.Contains(templatePointsList[2].position) &&
                    pieceCollidersList[2].bounds.Contains(templatePointsList[3].position))
                return 1;
            // peça 2 não tem posição opcional
            if (optionalTemplatePoints.Count != 0)
            {
                if (pieceCollidersList[1].bounds.Contains(optionalTemplatePoints[1].position) &&
                    pieceCollidersList[2].bounds.Contains(optionalTemplatePoints[2].position) &&
                    pieceCollidersList[3].bounds.Contains(optionalTemplatePoints[3].position))
                    return 2;
                else if (pieceCollidersList[1].bounds.Contains(optionalTemplatePoints[1].position) &&
                         pieceCollidersList[3].bounds.Contains(optionalTemplatePoints[2].position) &&
                         pieceCollidersList[2].bounds.Contains(optionalTemplatePoints[3].position))
                    return 2;
            }
            return 0;
        }
        else if (pieceName == "Piece.004")
        {
            if (pieceCollidersList[1].bounds.Contains(templatePointsList[1].position) &&
                pieceCollidersList[2].bounds.Contains(templatePointsList[2].position) &&
                pieceCollidersList[3].bounds.Contains(templatePointsList[3].position) &&
                pieceCollidersList[4].bounds.Contains(templatePointsList[4].position))
                return 1;
            else if (pieceCollidersList[3].bounds.Contains(templatePointsList[1].position) &&
                     pieceCollidersList[4].bounds.Contains(templatePointsList[2].position) &&
                     pieceCollidersList[1].bounds.Contains(templatePointsList[3].position) &&
                     pieceCollidersList[2].bounds.Contains(templatePointsList[4].position))
                return 1;
            return 0;
        }
        else if (pieceName == "Piece.007")
        {
            if (pieceCollidersList[1].bounds.Contains(templatePointsList[1].position) &&
                pieceCollidersList[2].bounds.Contains(templatePointsList[2].position) &&
                pieceCollidersList[3].bounds.Contains(templatePointsList[3].position) &&
                pieceCollidersList[4].bounds.Contains(templatePointsList[4].position))
                return 1;
            else if (pieceCollidersList[4].bounds.Contains(templatePointsList[1].position) &&
                     pieceCollidersList[1].bounds.Contains(templatePointsList[2].position) &&
                     pieceCollidersList[2].bounds.Contains(templatePointsList[3].position) &&
                     pieceCollidersList[3].bounds.Contains(templatePointsList[4].position))
                return 1;
            else if (pieceCollidersList[3].bounds.Contains(templatePointsList[1].position) &&
                     pieceCollidersList[4].bounds.Contains(templatePointsList[2].position) &&
                     pieceCollidersList[1].bounds.Contains(templatePointsList[3].position) &&
                     pieceCollidersList[2].bounds.Contains(templatePointsList[4].position))
                return 1;
            else if (pieceCollidersList[2].bounds.Contains(templatePointsList[1].position) &&
                     pieceCollidersList[3].bounds.Contains(templatePointsList[2].position) &&
                     pieceCollidersList[4].bounds.Contains(templatePointsList[3].position) &&
                     pieceCollidersList[1].bounds.Contains(templatePointsList[4].position))
                return 1;
            return 0;
        }
        else
        {
            Debug.LogWarning($"[CHECKPOSITION]: Piece name '{pieceName}' not recognized. Returning false.");
        }

        return 0;
    }

    protected IEnumerator IsSamePosition(int flag)
    {
        isChecking = true;

        yield return new WaitForSeconds(.5f);

        isChecking = false;

        int total = 0;
        int totalTrue = 0;

        while (queue.Count > 0)
        {
            int item = queue.Dequeue();
            total++;
            if (item != 0) totalTrue++;
        }

        if (total > 0)
        {
            float media = (float)totalTrue / total;

            if (media > 0.75f)
            {
                base.ChangeTemplateMaterial(highlightMaterial);
                base.ActivateTemplateMesh(flag);
                Invoke(nameof(ChangeTemplate), 1.0f);
                soundPieceFound.Play();
                templateFound = true;
            }
        }
    }

    public bool TemplateFound()
    {
        return templateFound;
    }

    public void SetTemplateFound(bool state)
    { 
        templateFound = state; 
    }

    // Template set back its original material
    public void ChangeTemplate()
    {
        //base.DeactivateTemplateMesh();
        base.ChangeTemplateMaterial(null);
    }
}
