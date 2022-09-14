using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannon : MonoBehaviour
{
    private float f;
    public float speed = 5f;
    public Transform shootingPoint;
    public float explosionRadius = 50f;
    public float shootingForce;
    public GameObject[] tetrisPieces;
    public SliderScript sliderRef;
    public MeshDeformerInput buttonRef;
    public bool canCannonRotate = true;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (buttonRef.isTouchingButton)
        {
            canCannonRotate = false;
        }
        else canCannonRotate = true;

        RotateCannon();
    }

    public void Shoot()
    {
        int i = Random.Range(0, tetrisPieces.Length);
        GameObject go = Instantiate(tetrisPieces[i], shootingPoint.transform.position, Quaternion.identity);


        Collider[] objects = UnityEngine.Physics.OverlapSphere(shootingPoint.transform.position, explosionRadius);
        foreach (Collider h in objects)
        {
            Rigidbody r = h.GetComponent<Rigidbody>();
            if (r != null)
            {
                r.AddExplosionForce(sliderRef.sliderValue * shootingForce, transform.position, explosionRadius);
            }
        }
        //go.GetComponent<Rigidbody>().AddForce((transform.position - go.transform.position) * sliderRef.sliderValue * shootingForce, ForceMode.Impulse);
    }

    private void RotateCannon()
    {
        if (!canCannonRotate) return;
        f += Time.deltaTime;
        transform.localRotation = Quaternion.Euler(new Vector3(0, 0, Mathf.Rad2Deg * Mathf.Sin(f)));
    }



}
