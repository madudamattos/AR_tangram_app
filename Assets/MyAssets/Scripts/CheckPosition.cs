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
    [HideInInspector] public bool templateFound = false;
    [SerializeField] Material highlightMaterial;
    protected bool isChecking = false;
    protected Queue<bool> queue = new Queue<bool>();
    
    protected override void Start()
    {
        base.Start();
    }

    protected virtual bool VerifyStartConditions()
    {
        return false;
    }

    protected virtual bool CheckTemplatePosition()
    {
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
