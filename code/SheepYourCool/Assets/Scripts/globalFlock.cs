using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class globalFlock : MonoBehaviour {

    public GameObject mSheepPrefab;
    public static GameObject[] mAllSheep;

    [SerializeField] int mSheepAmount = 10;
    [SerializeField] int mSheepRunSize = 5;

    // Start is called before the first frame update
    void Start()
    {
        mAllSheep = new GameObject[mSheepAmount];
        for (int i = 0; i < mSheepAmount; i++)
        {
            Vector3 pos = new Vector3(
                Random.Range(-mSheepRunSize, mSheepRunSize), 
                mSheepPrefab.transform.position.y, 
                Random.Range(-mSheepRunSize, mSheepRunSize));
            mAllSheep[i] = Instantiate(mSheepPrefab, pos, Quaternion.identity);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
