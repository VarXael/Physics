using UnityEngine;

public class MeshDeformerInput : MonoBehaviour {
	
	public float force = 10f;
	public float maxForceValue = 1500f;
	public float accumulatedForce;
	public float forceOffset = 0.1f;
	public bool isTouchingButton;
	public Cannon cannonRef;
	
	void Update () {
		if (Input.GetMouseButton(0)) {
			HandleInput();
		}
        if (isTouchingButton && Input.GetMouseButtonUp(0))
        {
			isTouchingButton = false;
			cannonRef.Shoot();
		}
		accumulatedForce = CalculateAccumulatedForce();
	}

	void HandleInput () {
		Ray inputRay = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;
		
		if (Physics.Raycast(inputRay, out hit)) {
			MeshDeformer deformer = hit.collider.GetComponent<MeshDeformer>();
			if (deformer) {
				isTouchingButton = true;
				Vector3 point = hit.point;
				point += hit.normal * forceOffset;
				deformer.AddDeformingForce(point, force);
			}
		}
	}

	private float CalculateAccumulatedForce()
    {
        if (isTouchingButton)
        {
			return accumulatedForce = Mathf.Clamp(accumulatedForce + force, 0, maxForceValue);
		}
		else return accumulatedForce = Mathf.Clamp(accumulatedForce - force,0,maxForceValue);
	}
}