using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EzySlice;

public class LaserBeam : MonoBehaviour
{
    public Transform cutPlane_0;
    public Transform cutPlane_1;
    public Material crossMaterial;
    public LayerMask layerMask;
    public ParticleSystem p;
    public Vector3 planeSize;
    public float cutForce;
    public float totalPiecesForCut;
    public List<TetrisPiece> hitPieces;
    private Vector3 Dir
    {
        get
        {
            Vector3 dir = new Vector3(transform.position.x, transform.position.y, transform.position.z - 1) - transform.position;
            dir = dir.normalized;
            //Vector3 dir = new Vector3(0, 0, -1);
            return dir;
        }
    }


    private void Update()
    {
        hitPieces = RayCastAll();
        if(hitPieces.Count >= totalPiecesForCut)
        {
            SlicingTime();

        }
    }


    public List<TetrisPiece> RayCastAll()
    {
        RaycastHit[] hits;
        List<TetrisPiece> tetrisPieces = new List<TetrisPiece>();
        hits = Physics.RaycastAll(transform.position, transform.right, 100.0F);

        for (int i = 0; i < hits.Length; i++)
        {
            RaycastHit hit = hits[i];
            TetrisPiece tp = hit.transform.GetComponent<TetrisPiece>();

            if (tp)
            {
                tetrisPieces.Add(tp);
                tp.ChangeColor = true;
            }
        }

        return tetrisPieces;
    }


    private void MakeCuttable(List<TetrisPiece> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            list[i].ChangeMyLayerMask("Cuttable");
        }
    }

    public void SlicingTime()
    {
        MakeCuttable(hitPieces);
        Slice(cutPlane_0);
        Slice(cutPlane_1);
        cutPlane_0.GetComponentInChildren<ParticleSystem>().Play();
        cutPlane_1.GetComponentInChildren<ParticleSystem>().Play();
    }



    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawCube(cutPlane_0.position, planeSize);
        Gizmos.DrawCube(cutPlane_1.position, planeSize);
    }





    #region Ezy Slice Methods
    public void Slice(Transform cutPlane)
    {
        Collider[] hits = Physics.OverlapBox(cutPlane.position, planeSize, cutPlane.rotation, layerMask);

        if (hits.Length <= 0)
        {
            return;
        }

        for (int i = 0; i < hits.Length; i++)
        {
            SlicedHull hull = SliceObject(hits[i].gameObject, crossMaterial);
            if (hull != null)
            {
                GameObject bottom = hull.CreateLowerHull(hits[i].gameObject, crossMaterial);
                GameObject top = hull.CreateUpperHull(hits[i].gameObject, crossMaterial);
                AddHullComponents(bottom);
                AddHullComponents(top);
                Destroy(hits[i].gameObject);
            }
        }
    }

    public SlicedHull SliceObject(GameObject obj, Material crossSectionMaterial = null)
    {
        // slice the provided object using the transforms of this object
        if (obj.GetComponent<MeshFilter>() == null)
            return null;

        return obj.Slice(cutPlane_0.position, cutPlane_0.up, crossSectionMaterial);
    }

    public void AddHullComponents(GameObject go)
    {
        go.layer = 9;
        Rigidbody rb = go.AddComponent<Rigidbody>();
        rb.interpolation = RigidbodyInterpolation.Interpolate;
        MeshCollider collider = go.AddComponent<MeshCollider>();
        collider.convex = true;
        go.AddComponent<HullScript>();
        rb.AddForce(Dir * cutForce, ForceMode.Impulse);
    }

    #endregion
    
}
