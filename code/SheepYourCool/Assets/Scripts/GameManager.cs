using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public bool mCursorLockedAndInvisible = true;

    public MapManager mMapManager;
    public SheepManager mSheepManager;

    // Start is called before the first frame update
    void Start()
    {
        if (mCursorLockedAndInvisible)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        if (GlobalVariables.mInitializedPerMenu)
        {
            mMapManager.SetSize(GlobalVariables.mAreaSize);
            mSheepManager.mSheepAmount = GlobalVariables.mSheepAmount;
        }

        mMapManager.Initialize();
        mSheepManager.Initialize();




    }

    // Update is called once per frame
    void Update()
    {

    }

    public void FenceClosed(List<Vector3> fencePoints)
    {
        Vector2[] fencePolygon = new Vector2[fencePoints.Count];
        for (int i = 0; i < fencePolygon.Length; i++)
        {
            Vector3 fencePoint = fencePoints[i];
            fencePolygon[i] = new Vector2(fencePoint.x, fencePoint.z);
        }

        for (int i = 0; i < mSheepManager.mAllSheepAsList.Count; i++)
        {
            GameObject sheep = mSheepManager.mAllSheepAsList[i];
            if (!sheep.GetComponent<Flock>().mIsCatched && ContainsPoint(fencePolygon, new Vector2(sheep.transform.position.x, sheep.transform.position.z)))
            {
                mSheepManager.CatchSheep(i);

            }
        }
    }


    //method from http://wiki.unity3d.com/index.php?title=PolyContainsPoint&_ga=2.263947726.390560634.1604691747-1928379729.1603541402
    private bool ContainsPoint(Vector2[] polyPoints, Vector2 p)
    {

        var j = polyPoints.Length - 1;
        bool inside = false;
        for (int i = 0; i < polyPoints.Length; j = i++)
        {
            if (((polyPoints[i].y <= p.y && p.y < polyPoints[j].y) || (polyPoints[j].y <= p.y && p.y < polyPoints[i].y)) &&
               (p.x < (polyPoints[j].x - polyPoints[i].x) * (p.y - polyPoints[i].y) / (polyPoints[j].y - polyPoints[i].y) + polyPoints[i].x))
                inside = !inside;
        }
        if (inside)
        {
            Debug.Log("CATCHED SHEEP!");
        }
        return inside;
    }

}
