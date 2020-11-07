using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneManager : MonoBehaviour
{
    public bool mCursorLockedAndInvisible = true;
    // Start is called before the first frame update
    void Start()
    {
        if (mCursorLockedAndInvisible)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
