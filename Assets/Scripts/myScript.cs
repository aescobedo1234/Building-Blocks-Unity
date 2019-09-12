//Arnold Escobedo 
//Com 465
//Homework 3 Building Blocks

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class myScript : MonoBehaviour
{
    public Material blockMaterial;
    public Material baseMaterial;
    public Transform CubeToTransform;

    // Use this for initialization
    Ray ray;
    RaycastHit hitX;
    bool created = false;
    float scaleY = 0.1f;
    float spacing = 0.1f;
    bool displayTransparentCube;
    List<Transform> baseObjects;


    void Start()
    {
        //Create the Platform Where we will add blocks
        var type = Type.GetType("myScript");
        baseObjects = new List<Transform>();
        GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        cube.transform.position = new Vector3(0 + (0 * spacing), 0, 1 + (1 * spacing));
        cube.transform.rotation = Quaternion.identity;
        cube.transform.localScale = new Vector3(10, scaleY, 10);

        cube.name = string.Format("base", 0, 1);
        cube.GetComponent<Renderer>().material = baseMaterial;

    }

    // Update is called once per frame
    void Update()
    {
        //This is so that the mouse appears to have the outline of the block before it is placed
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hitX))
        {

            if (hitX.collider.name == "base")
            {

                foreach (Transform t in baseObjects)
                {
                    //run pos comparison
                    //if shares space, return
                    if (Vector3.Distance(hitX.point, t.position) < 1.4f)
                    {

                        CubeToTransform.GetComponent<Renderer>().material.color = new Color(255f, 0f, 0f, 0.5f);
                    }
                    else
                    {
                        //color transparent yellow
                        CubeToTransform.GetComponent<Renderer>().material.color = new Color(1.3f, 1.1f, 0, 0.5f);
                    }
                }

                //color transparent yellow
                //CubeToTransform.GetComponent<Renderer>().material.color = new Color(1.3f, 1.1f, 0, 0.5f);

                
                CubeToTransform.transform.position = new Vector3(hitX.point.x, hitX.point.y + (0.5f), hitX.point.z);

            }
            if (hitX.collider.name == "cube")
            {

                Vector3 position = hitX.transform.position + hitX.normal;

                // calculate the rotation to create the object aligned with the face normal
                Quaternion rotation = Quaternion.FromToRotation(Vector3.up, hitX.normal);

                CubeToTransform.transform.position = position;
                CubeToTransform.transform.rotation = rotation;

                //green Transparent color
                CubeToTransform.GetComponent<Renderer>().material.color = new Color(0, 1.1f, 0, 0.5f);
            }

        }
        //when left click execute following code:
        if (Input.GetMouseButtonUp(0))
        {

            #region Screen To World
            RaycastHit hitInfo = new RaycastHit();


            bool hit = Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo);
            if (hit)
            {
                // Find center of face
                Vector3 position = hitInfo.transform.position + hitInfo.normal;

                // calculate the rotation to create the object aligned with the face normal:
                Quaternion rotation = Quaternion.FromToRotation(Vector3.up, hitInfo.normal);

                if (hitInfo.transform.gameObject.name == "base")
                {
                    bool canSpawn = true;

                    foreach (Transform t in baseObjects)
                    {
                        //run pos comparison
                        //if shares space, return
                        if (Vector3.Distance(hitInfo.point, t.position) < 1.4f)
                        {
                            canSpawn = false;
                            CubeToTransform.GetComponent<Renderer>().material.color = Color.red;
                        }
                    }

                    if (canSpawn)
                    {
                        //create cube
                        var cube = GameObject.CreatePrimitive(PrimitiveType.Cube);

                        cube.tag = "MyCube";
                        cube.GetComponent<Renderer>().material = blockMaterial;

                        //fix cube position
                        cube.transform.position = new Vector3(hitInfo.point.x, hitInfo.point.y + (0.5f), hitInfo.point.z);
                        cube.name = "cube";

                        baseObjects.Add(cube.transform);
                    }
                }
                else
                {
                    var cube = GameObject.CreatePrimitive(PrimitiveType.Cube);

                    cube.tag = "MyCube";
                    cube.name = "cube";
                    cube.GetComponent<Renderer>().material = blockMaterial;

                    cube.transform.position = position;
                    cube.transform.rotation = rotation;
                    baseObjects.Add(cube.transform);
                }

            }
            else
            {
                Debug.Log("No Hit");
            }
            #endregion
        }
    }
}
