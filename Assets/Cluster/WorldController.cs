using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldController : MonoBehaviour
{
    public GameObject WhiteCube;
    public GameObject BlackCube;

    public int TotalCube = 128;
    private CubeController[] Cubes;

    public int TotalCluster = 16;
    private int[] Clusters;

    public float WorldRadius = 8f;
    public float AttractionForceScale = 1f;

    public WallpaperEngine Settings;

    public Material WhiteCubeMat;
    public Material BlackCubeMat;

    // Start is called before the first frame update
    void Start()
    {
        if (TotalCube % 2 != 0)
            TotalCube += 1;

        Cubes = new CubeController[TotalCube];

        for(int i=0;i<TotalCube;i++)
        {
            GameObject C = GameObject.Instantiate(i % 2 == 0 ? WhiteCube : BlackCube, Random.insideUnitSphere * WorldRadius, Quaternion.identity);
            Cubes[i] = C.GetComponent<CubeController>();
        }

        Clusters = new int[TotalCluster];
        for (int i = 0; i < TotalCluster; i++)
        {
            Clusters[i] = i;
            Cubes[i].Body.isKinematic = true;
        }

        Settings.ChangeCustomColorHandler += delegate (Color Value)
        {
            BlackCubeMat.color = Value;
        };

        Settings.ChangeSchemeColorHandler += delegate (Color Value)
        {
            WhiteCubeMat.color = Value;
        };
    }
    

    bool IsCluster(int Index )
    {
        return System.Array.Exists(Clusters, C => C == Index);
    }

    void ResetWorld()
    {
        for (int i = 0; i < TotalCube; i++)
        {            
            Cubes[i].transform.position = Random.insideUnitSphere * WorldRadius;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 Dir;
        int Current, Closest;
        for (int i = 0; i < Cubes.Length; i++)
        {
            Current = i;

//            for (int j = 0; j < Clusters.Length; j++)
            {
                Closest = i % Clusters.Length;

                Dir = Cubes[Closest].transform.position - Cubes[Current].transform.position;

                Cubes[Current].Body.AddForce(Dir * AttractionForceScale * Cubes[Current].Damping, ForceMode.Force);
            }
        }

        if (Input.GetKeyDown(KeyCode.Space))
            ResetWorld();
    }

    int FindClosestCube( int Index )
    {
        Vector3 Distance;
        float ShortestDistanceSqr = float.PositiveInfinity;
        int Closest = 0;
        for( int C = 0; C<Cubes.Length; C++ )
        {
            if (Index == C && IsCluster(C)) // not self and not a cluster
                continue;

            Distance = Cubes[Index].transform.position - Cubes[C].transform.position;

            if( Distance.sqrMagnitude < ShortestDistanceSqr )
            {
                // found new shortest
                Closest = C;
                ShortestDistanceSqr = Distance.sqrMagnitude;
            }
        }
        return Closest;
    }
    
    private void OnDrawGizmosSelected()
    {
        // Display the explosion radius when selected
        Gizmos.color = new Color(1, 1, 0, 0.75F);
        Gizmos.DrawWireSphere(transform.position, WorldRadius);
    }

}
