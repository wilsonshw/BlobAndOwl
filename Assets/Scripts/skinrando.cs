using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class skinrando : MonoBehaviour
{
    public SkinnedMeshRenderer p1Body;
    public SkinnedMeshRenderer p1Hand;

    public SkinnedMeshRenderer p2Body;
    public SkinnedMeshRenderer p2Hand;

    public Mesh[] allBodyMeshes;
    public Mesh[] allHandMeshes;
    // Start is called before the first frame update
    void Start()
    {
        int rando1 = Random.Range(0, allBodyMeshes.Length);
        p1Body.sharedMesh = allBodyMeshes[rando1];
        p1Hand.sharedMesh = allHandMeshes[rando1];

        int rando2 = Random.Range(0, allBodyMeshes.Length);
        p2Body.sharedMesh = allBodyMeshes[rando2];
        p2Hand.sharedMesh = allHandMeshes[rando2];
    }

}
