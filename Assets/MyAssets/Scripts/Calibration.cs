using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;


public class Calibration : MonoBehaviour
{
    [Header("Calibration Variables")]
    public float moveSpeed = .05f;
    public float verticalMoveSpeed = .05f;
    public float scaleMoveSpeed = .01f;
    public float horizontalMoveSpeed = .05f;
    public float horizontalRotateSpeed = 50f;

    [Header("Pieces")]
    public List<GameObject> mesh = new List<GameObject>();

    [Header("Table")]
    public GameObject piecesRef;
    public GameObject tangramRef;

    // variables
    public int num = 0;
    public int mode = 0; // mode 0 is for mesh editing/mode 1 is for table editing

    // Update is called once per frame
    void Update()
    {
        if(OVRInput.Get(OVRInput.Button.SecondaryThumbstick))
        {
            FinishCalibration();
        }

        OVRInput.Controller activeController = OVRInput.GetActiveController();
        if ((activeController & OVRInput.Controller.Touch) != 0)
        {
            // Se apertar o trigger de cima troca o mesh
            if (OVRInput.GetDown(OVRInput.Button.SecondaryIndexTrigger))
            {
                mesh[num].SetActive(false);
                num = (num + 1) % mesh.Count;
                mesh[num].SetActive(true);
            }

            // troca o modo de edição mesh/mesa
            if (OVRInput.Get(OVRInput.Button.PrimaryThumbstick))
            {
                if (mode == 0)
                {
                    Debug.Log("Switching mode to 1");
                    mode = 1;
                }
                else
                {
                    mode = 0;
                    Debug.Log("Switching mode to 0");
                }
            }

            // Movimenta a mesa
            Vector2 leftThumbstick = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick);

            Vector3 movement = new Vector3(leftThumbstick.x, 0, leftThumbstick.y) * moveSpeed * Time.deltaTime;
            piecesRef.transform.Translate(movement, Space.World);
            tangramRef.transform.Translate(movement, Space.World);

            // Movimenta o mesh 
            /*Vector3 movement = new Vector3(leftThumbstick.x, 0, leftThumbstick.y) * moveSpeed * Time.deltaTime;
            mesh[num].transform.Translate(movement, Space.World);*/

            // Rotaciona o mesh no próprio eixo Z 
            Vector2 rightThumbstick = OVRInput.Get(OVRInput.Axis2D.SecondaryThumbstick);

            float rotateZ = -rightThumbstick.x * horizontalRotateSpeed * Time.deltaTime;
            /*mesh[num].transform.Rotate(Vector3.up, rotateZ, Space.World);*/
            piecesRef.transform.Rotate(Vector3.up, rotateZ, Space.World);
            tangramRef.transform.Rotate(Vector3.up, rotateZ, Space.World);


            // Sobre mesh (modo 0) / sobe mesa (modo 1)
            if (OVRInput.Get(OVRInput.Button.Three)) // Mapeado para o botão 'X' no controle esquerdo
            {
                if(mode == 0)
                {
                    // Sobe a mesa
                    Debug.Log("Subindo a mesa");
                    piecesRef.transform.localPosition += Vector3.forward * verticalMoveSpeed * Time.deltaTime;
                    tangramRef.transform.localPosition += Vector3.forward * verticalMoveSpeed * Time.deltaTime;
                } 
                else
                {
                    // Sobe o mesh
                    Debug.Log("Subindo o mesh");
                    mesh[num].transform.localPosition += Vector3.forward * verticalMoveSpeed * Time.deltaTime;
                }
            }

            // Desce mesh (modo 0) / desce mesa (modo 1)
            if (OVRInput.Get(OVRInput.Button.Four)) // Mapeado para o botão 'Y' no controle esquerdo
            {
                if (mode == 0)
                {
                    Debug.Log("Descendo a mesa");
                    piecesRef.transform.localPosition += Vector3.back * verticalMoveSpeed * Time.deltaTime;
                    tangramRef.transform.localPosition += Vector3.back * verticalMoveSpeed * Time.deltaTime;
                }
                else
                {
                    Debug.Log("Descendo o mesh");
                    mesh[num].transform.localPosition += Vector3.back * verticalMoveSpeed * Time.deltaTime;
                }
            }

            // Aumenta o mesh
            if (OVRInput.Get(OVRInput.Button.PrimaryIndexTrigger))
            {
                // Cria um vetor de escala uniforme para adicionar à escala atual
                Vector3 scaleIncrease = Vector3.one * scaleMoveSpeed * Time.deltaTime;
                mesh[num].transform.localScale += scaleIncrease;
            }

            // Diminui o mesh
            if (OVRInput.Get(OVRInput.Button.PrimaryHandTrigger))
            {
                // Cria um vetor de escala uniforme para subtrair da escala atual
                Vector3 scaleDecrease = Vector3.one * scaleMoveSpeed * Time.deltaTime;
                mesh[num].transform.localScale -= scaleDecrease;

                // Opcional: prevenir que a escala fique negativa ou muito pequena
                if (mesh[num].transform.localScale.x < 0.01f || mesh[num].transform.localScale.y < 0.01f || mesh[num].transform.localScale.z < 0.01f)
                {
                    mesh[num].transform.localScale = new Vector3(0.01f, 0.01f, 0.01f); // Define um mínimo
                }
            }
        }
    }
    
    [System.Serializable]
    public class MeshTransformData
    {
        public string name;
        public float[] position;
        public float[] rotation;
        public float[] scale;

        public MeshTransformData(GameObject obj)
        {
            name = obj.name;
            Vector3 pos = obj.transform.localPosition;
            Vector3 rot = obj.transform.localRotation.eulerAngles;
            Vector3 scl = obj.transform.localScale;

            position = new float[] { pos.x, pos.y, pos.z };
            rotation = new float[] { rot.x, rot.y, rot.z };
            scale = new float[] { scl.x, scl.y, scl.z };
        }
    }

    [System.Serializable]
    public class MeshTransformCollection
    {
        public List<MeshTransformData> Meshes = new List<MeshTransformData>();
    }

    public void FinishCalibration()
    {
        MeshTransformCollection collection = new MeshTransformCollection();

        foreach (GameObject obj in mesh)
        {
            if (obj != null)
            {
                collection.Meshes.Add(new MeshTransformData(obj));
            }
        }

        string json = JsonUtility.ToJson(collection, true);
        string path = Application.persistentDataPath + "/meshTransforms.json";
        File.WriteAllText(path, json);


        Debug.Log("Mesh data exported to: " + path);
    }

}