using System.Collections;
using System.Collections.Generic;
using Meta.XR.MRUtilityKit;
using UnityEngine;

public class ActivateObjects : MonoBehaviour
{

    public Transform refPosition;
    public GameObject tangramRef;

    Vector3 anchorCenter;
    Quaternion anchorRotation;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(InitializeAfterSceneLoad());
    }

    IEnumerator InitializeAfterSceneLoad()
    {
        yield return new WaitForSeconds(1f);
        InitializeObjects();
    }

    void InitializeObjects()
    {
        var rooms = MRUK.Instance.GetCurrentRoom().Anchors;

        foreach (var anchor in rooms)
        {
            if (anchor.gameObject.name == "TABLE")
            {
                anchorCenter = anchor.GetAnchorCenter();
                anchorRotation = anchor.transform.rotation;

                // Obtém os centros das faces da bounding box
                Vector3[] faceCenters = anchor.GetBoundsFaceCenters();

                // Verifica se há faces e usa a face superior
                if (faceCenters.Length > 0)
                {
                    Vector3 topFace = faceCenters[0]; // Índice 0 é a face superior
                    this.transform.position = topFace;
                    this.transform.rotation = anchorRotation;
                    tangramRef.transform.position = new Vector3(refPosition.position.x, refPosition.position.y + 0.05f, refPosition.position.z);
                }

                break;
            }
        }
    }

}
