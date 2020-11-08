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
}
