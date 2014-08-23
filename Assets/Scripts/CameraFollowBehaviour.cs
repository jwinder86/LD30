using UnityEngine;
using System.Collections;

public class CameraFollowBehaviour : MonoBehaviour {

	public Transform toFollow;

	private Vector3 offset;

	// Use this for initialization
	void Start () {
		offset = transform.position - toFollow.position;
	}
	
	// Update is called once per frame
	void LateUpdate () {
		transform.position = toFollow.position + offset;
	}
}
