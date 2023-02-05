using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;


public class TestHexagonPoint: MonoBehaviour
{
    [SerializeField] SpriteRenderer mouseMeshRenderer;
    [SerializeField] Material greenMat;
    [SerializeField] Material redMat;

    private Hexagon hex;

    private void Start()
    {
        hex = new Hexagon(new Vector3(0, 0), 5f);
    }

    private void Update()
    {
        Vector3 testPosition = UtilsClass.GetMouseWorldPosition();
        //Debug.Log("Mouse: " + testPosition.ToString() );
        mouseMeshRenderer.material = redMat;

        bool inside = true;
        for (int i = 0; i < 6; i++)
        {
            if (testPosition.x < hex.upperRightCorner.x &&
                testPosition.x > hex.upperLeftCorner.x)
            {
                if (testPosition.y < hex.upperCorner.y && testPosition.y > hex.lowerCorner.y)
                {

                    Vector3 dirFromURtoUC = hex.upperCorner - hex.upperRightCorner;
                    //Debug.Log("Before:" + dirFromURtoUC);
                    Vector3 dotDirUR = UtilsClass.ApplyRotationToVector(dirFromURtoUC, 90);
                    //Debug.Log("After:" + dotDirUR);

                    Vector3 dirToTestPoint = testPosition - hex.upperRightCorner;
                    float dotUR = Vector3.Dot(dotDirUR.normalized, dirToTestPoint.normalized);

                    //Debug.DrawLine(hex.upperCorner, testPosition, Color.magenta);
                  //  Debug.DrawLine(hex.upperRightCorner, hex.upperRightCorner + dotDirUR, Color.yellow);

                    Vector3 dirFromULtoUC = hex.upperCorner - hex.upperLeftCorner;
                    Vector3 dotDirUL = UtilsClass.ApplyRotationToVector(dirFromULtoUC, -90);
                    dirToTestPoint = testPosition - hex.upperLeftCorner;
                    float dotUL = Vector3.Dot(dotDirUL.normalized, dirToTestPoint.normalized);
                 //   Debug.DrawLine(hex.upperCorner, dotDirUL, Color.white);
                 //   Debug.DrawLine(hex.upperLeftCorner, hex.upperLeftCorner + dotDirUL, Color.yellow);

                    Vector3 dirFromLRtoLC = hex.lowerCorner - hex.lowerRightCorner;
                    Vector3 dotDirLR = UtilsClass.ApplyRotationToVector(dirFromLRtoLC,-90);
                    dirToTestPoint = testPosition - hex.lowerRightCorner;
                    float dotLR = Vector3.Dot(dotDirLR.normalized, dirToTestPoint.normalized);
                //    Debug.DrawLine(hex.lowerCorner, hex.lowerCorner + dotDirLR, Color.white);
                //    Debug.DrawLine(hex.lowerRightCorner, hex.lowerRightCorner + dotDirLR, Color.blue);
                //    Debug.DrawLine(hex.lowerRightCorner, hex.lowerRightCorner + dotDirLR, Color.yellow);

                    Vector3 dirFromLLtoLC = hex.lowerCorner - hex.lowerLeftCorner;
                    Vector3 dotDirLL = UtilsClass.ApplyRotationToVector(dirFromLLtoLC, 90);
                    dirToTestPoint = testPosition - hex.lowerLeftCorner;
                    float dotLL = Vector3.Dot(dotDirLL.normalized, dirToTestPoint.normalized);
               //     Debug.DrawLine(hex.lowerCorner, testPosition, Color.magenta);
               //     Debug.DrawLine(hex.lowerCorner, dotDirLL, Color.white);
                //    Debug.DrawLine(hex.lowerLeftCorner, hex.lowerLeftCorner + dotDirLL, Color.yellow);

                    if (dotUR> 0 && dotUL > 0 && dotLR > 0 && dotLL > 0)
                    {
                        mouseMeshRenderer.material = greenMat;
                    }

                }
            }


        }
    }
}

public class Hexagon 
{
    public float halfSize;
    public Vector3 centerPoint;

    public Vector3[] corners;
    public Vector3 upperRightCorner;
    public Vector3 upperLeftCorner;
    public Vector3 upperCorner;
    public Vector3 lowerRightCorner;
    public Vector3 lowerLeftCorner;
    public Vector3 lowerCorner;

    public Hexagon(Vector3 centerPoint, float halfSize)
    {
        this.centerPoint = centerPoint;
        this.halfSize = halfSize;

        //corners = new Vector3[6];

        //corners[0] = centerPoint + new Vector3(0, 1) * halfSize;
        //corners[3] = centerPoint + new Vector3(0, -1) * halfSize;
        //corners[5] = centerPoint + new Vector3(1, 0) * halfSize + new Vector3(0,1) * halfSize*.5f;
        //corners[1] = centerPoint + new Vector3(-1, 0) * halfSize + new Vector3(0, 1) * halfSize * .5f;
        //corners[4] = centerPoint + new Vector3(1, 0) * halfSize + new Vector3(0, -1) * halfSize * .5f;
        //corners[2] = centerPoint + new Vector3(-1, 0) * halfSize + new Vector3(0, -1) * halfSize * .5f;

        upperCorner = centerPoint + new Vector3(0, 1) * halfSize;
        lowerCorner = centerPoint + new Vector3(0, -1) * halfSize;
        upperRightCorner = centerPoint + new Vector3(1, 0) * halfSize + new Vector3(0, 1) * halfSize * .5f;
        upperLeftCorner = centerPoint + new Vector3(-1, 0) * halfSize + new Vector3(0, 1) * halfSize * .5f;
        lowerRightCorner = centerPoint + new Vector3(1, 0) * halfSize + new Vector3(0, -1) * halfSize * .5f;
        lowerLeftCorner = centerPoint + new Vector3(-1, 0) * halfSize + new Vector3(0, -1) * halfSize * .5f;
    }

   
}
