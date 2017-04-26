using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WizardController : MonoBehaviour {

	public float jetpackForce = 75.0f;
	public float forwardMovementSpeed = 3.0f;
	public Rigidbody2D rb;
	public Transform groundCheckTransform;
	private bool grounded;
	public LayerMask groundCheckLayerMask;
	private uint coins = 0;
	private uint blueOrbe = 0;
	private int meters = 0;
	public Texture2D coinIconTexture;
	public AudioClip coinCollectSound;

	public Texture2D blueOrbeTexture;

	public ParticleSystem jetpack;

	private Animator animator;

	private uint forestsGenerated = 0;

	public bool isDead = false;
	public bool isCombat = false;
	public bool isVictory = false;
	public bool needsBoost = false;

	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody2D> ();
		animator = GetComponent<Animator>() ;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void FixedUpdate () 
	{
		bool jetpackActive = Input.GetButton("Fire1");

		if (jetpackActive)
		{
			rb.AddForce(new Vector2(0, jetpackForce));
		}

		Vector2 newVelocity = rb.velocity;
		newVelocity.x = forwardMovementSpeed;
		rb.velocity = newVelocity;

		UpdateGroundedStatus ();
		AdjustJetpack(jetpackActive);

		if (GetComponent<GeneratorScript> ().forestsGenerated % 5 == 0 
			&& forestsGenerated != GetComponent<GeneratorScript> ().forestsGenerated 
			&& isCombat == false) {

			forwardMovementSpeed += 0.3f;
			forestsGenerated = GetComponent<GeneratorScript> ().forestsGenerated;
		}

		meters = (int)transform.position.x;

	}

	void DisplayCoinsCount()
	{
		Rect coinIconRect = new Rect(25, 35, 32, 32);
		GUI.DrawTexture(coinIconRect, coinIconTexture);                         

		GUIStyle style = new GUIStyle();
		style.fontSize = 30;
		style.fontStyle = FontStyle.Bold;
		style.normal.textColor = Color.yellow;

		Rect labelRect = new Rect(coinIconRect.xMax, coinIconRect.y, 60, 32);
		GUI.Label(labelRect, coins.ToString(), style);
	}

	void DisplayBlueOrbesCount()
	{
		Rect blueOrbeIconRect = new Rect(200, 35, 32, 32);
		GUI.DrawTexture(blueOrbeIconRect, blueOrbeTexture);                         

		GUIStyle style = new GUIStyle();
		style.fontSize = 30;
		style.fontStyle = FontStyle.Bold;
		style.normal.textColor = Color.yellow;

		Rect labelRect = new Rect(blueOrbeIconRect.xMax, blueOrbeIconRect.y, 32, 32);
		GUI.Label(labelRect, blueOrbe.ToString() + "/3", style);
	}

	void DisplayDistance()
	{
		Rect blueOrbeIconRect = new Rect(0, 0, 32, 32);                      

		GUIStyle style = new GUIStyle();
		style.fontSize = 30;
		style.fontStyle = FontStyle.Bold;
		style.normal.textColor = Color.white;

		Rect labelRect = new Rect(blueOrbeIconRect.xMax, blueOrbeIconRect.y, 32, 32);
		GUI.Label(labelRect, meters.ToString()+"m", style);
	}

	void OnGUI()
	{
		DisplayCoinsCount();
		DisplayBlueOrbesCount ();
		DisplayDistance ();
	}

	void OnTriggerEnter2D(Collider2D collider)
	{
		if (collider.gameObject.CompareTag ("coins")) {
			CollectCoin(collider);

		}
		if (collider.gameObject.CompareTag ("spells")) {
			Destroy (collider.gameObject);
			UnityEngine.SceneManagement.SceneManager.LoadScene("GameOver");
		}
		if (collider.gameObject.CompareTag ("ressources")) {
			if (collider.gameObject.name == "water_orbe(Clone)")
				++blueOrbe;
			Destroy (collider.gameObject);
		}

		if (blueOrbe == 3) {
			isCombat = true;
			GetComponent<GeneratorScript> ().isCombat = true;
		}

			
	}

	void CollectCoin(Collider2D coinCollider)
	{
		coins++;

		AudioSource.PlayClipAtPoint(coinCollectSound, transform.position);

		Destroy(coinCollider.gameObject);
	}

	void AdjustJetpack (bool jetpackActive)
	{
		jetpack.enableEmission = !grounded;
		jetpack.emissionRate = jetpackActive ? 300.0f : 0.0f; 
	}

	void UpdateGroundedStatus()
	{
		//1
		grounded = Physics2D.OverlapCircle(groundCheckTransform.position, 0.1f, groundCheckLayerMask);

		//2
		animator.SetBool("grounded", grounded);
	}
}
