using UnityEngine;
using System.Collections;

public class RecursiveTree : MonoBehaviour {

	// Use this for initialization
	void Start () {
		ScaleTree ();
	}
	
	// Update is called once per frame
	void Update () {

	}

	void OnDrawGizmos() {
		Gizmos.color = Color.black;

		if (!isPaused) {
			langle += lincrement;
			rangle += rincrement;
		}

		if (is2D) {
			CreateTree (new Vector2 (0, -slength), 0, slength, 0);
		} else {
			CreateTree3D (new Vector3(0, -slength, 0), new Vector3(0, 1, 0), slength, 0);
		}
	}

	public void Toggle2D() {
		is2D = !is2D;
	}

	public float lincrement = 0f, rincrement = 0f;
	public float langle = 30f, rangle = 30f;
	public float llength = 0.8f, rlength = 0.8f;
	public float slength = 1f;
	public int treeDepth = 8;
	public bool isPaused = false;
	public bool is2D = true;

	public float String2Float(string s) {
		float num;
		if (float.TryParse (s, out num)) {
			return num;
		} else {
			return 0f;
		}
	}

	public void TogglePause(bool t) {
		isPaused = t;
	}

	public void SetLIncrement(string i) {
		lincrement = String2Float(i) / 100f;
	}

	public void SetRIncrement(string i) {
		rincrement = String2Float(i) / 100f;
	}

	public void SetLAngle(string a) {
		langle = String2Float(a);
	}

	public void SetRAngle(string a) {
		rangle = String2Float(a);
	}

	public void SetLLength(string l) {
		llength = String2Float(l);
		ScaleTree ();
	}

	public void SetRLength(string l) {
		rlength = String2Float(l);
		ScaleTree ();
	}

	public void SetTreeDepth(float depth) {
		treeDepth = (int)depth;
		ScaleTree ();
	}
	
	private void CreateTree(Vector2 pos, float angle, float length, int depth) {
		if (depth >= treeDepth) {
			return;
		}

		Vector2 newPos = new Vector2 (pos.x + length * Mathf.Sin (Mathf.Deg2Rad * angle),
		                              pos.y + length * Mathf.Cos (Mathf.Deg2Rad * angle));
		Vector2 newPos2 = new Vector2 (pos.x + length * Mathf.Sin (Mathf.Deg2Rad * angle),
		                               pos.y + length * Mathf.Cos (Mathf.Deg2Rad * angle));

		Gizmos.DrawLine (new Vector3 (pos.x, pos.y, 0), new Vector3 (newPos.x, newPos.y, 0));

		CreateTree (newPos, angle + rangle, length * rlength, depth + 1);
		CreateTree (newPos2, angle - langle, length * llength, depth + 1);
	}

	private void CreateTree3D(Vector3 pos, Vector3 dir, float length, int depth) {
		if (depth >= treeDepth) {
			return;
		}
		
		Vector3 newPos = pos + dir * length;
		
		Gizmos.DrawLine (pos, newPos);
		
		float xAngle = Mathf.Atan2 (dir.x, dir.y);
		float zAngle = Mathf.Atan2 (dir.y, dir.z);

		// x axis branches
		CreateTree3D (newPos, new Vector3 (Mathf.Sin (xAngle + Mathf.Deg2Rad * langle), 
		                                 Mathf.Cos (xAngle + Mathf.Deg2Rad * langle), 0),
		            length * llength, depth + 1);
		CreateTree3D (newPos, new Vector3 (Mathf.Sin (xAngle - Mathf.Deg2Rad * langle), 
		                                 Mathf.Cos (xAngle - Mathf.Deg2Rad * langle), 0),
		            length * llength, depth + 1);

		// z axis branches
		CreateTree3D (newPos, new Vector3 (0, Mathf.Cos (zAngle + Mathf.Deg2Rad * rangle),
		                                   Mathf.Sin (zAngle + Mathf.Deg2Rad * rangle)),
		              length * rlength, depth + 1);
		CreateTree3D (newPos, new Vector3 (0, Mathf.Cos (zAngle - Mathf.Deg2Rad * rangle), 
		                                   Mathf.Sin (zAngle - Mathf.Deg2Rad * rangle)),
		              length * rlength, depth + 1);
	}

	private void ScaleTree() {

		if (treeDepth == 1 || (llength == 0 && rlength == 0)) {
			Camera.main.orthographicSize = slength;
			return;
		}

		float totalLength = 0;
		float branchLength = rlength >= llength ? rlength : llength;
		
		for (int i = 0; i < treeDepth; i++) {
			totalLength += slength * Mathf.Pow(branchLength, i);
		}
		
		Camera.main.orthographicSize = totalLength - slength;
	}
}
