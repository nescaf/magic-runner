using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleSortingLayerFix : MonoBehaviour {

	public Renderer pr;

	// Use this for initialization
	void Start () {
		pr = GetComponent<Renderer> ();
		pr.sortingLayerName = "Player";
		pr.sortingOrder = -1;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
