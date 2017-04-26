using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneratorScript : MonoBehaviour {

	public GameObject[] availableForests;
	public List<GameObject> currentForests;
	private float screenWidthInPoints;

	public GameObject[] availableObjects;    
	public List<GameObject> objects;

	public GameObject[] availableEnemySpells;
	public List<GameObject> enemySpells;

	public GameObject[] availableRessources;
	public List<GameObject> ressources;

	public float objectsMinDistance = 5.0f;    
	public float objectsMaxDistance = 10.0f;

	public float spellsMinDistance = 30.0f;    
	public float spellsMaxDistance = 35.0f;

	public float spellsMinY = -1.0f;
	public float spellsMaxY = 10.0f;

	public float objectsMinY = 0.0f;
	public float objectsMaxY = 10.0f;

	public float enemySpellSpeed = 5.0f;
	public uint forestsGenerated = 0;

	public GameObject dragon;

	public bool isCombat = false;
	public bool dragonCreated = false;

	// Use this for initialization
	void Start () {
		float height = 2.0f * Camera.main.orthographicSize;
		screenWidthInPoints = height * Camera.main.aspect;
	}

	// Update is called once per frame
	void Update () {
		MoveSpells();
	}

	void FixedUpdate () 
	{    
		GenerateForestIfRequired();
		GenerateSpellsIfRequired();

		isCombat = GetComponent<WizardController> ().isCombat;

		if (isCombat == false) {
			GenerateObjectsIfRequired ();
			GenerateRessourcesIfRequired ();
		}

		if (isCombat == true && dragonCreated == false) {
			dragonCreated = true;
			dragon = (GameObject)Instantiate (dragon);
			dragon.transform.position = new Vector3(transform.position.x + 8
				,transform.position.y,0); 
		}

		if (dragonCreated == true) {
			dragon.transform.position = new Vector3(transform.position.x + 8
				,transform.position.y,0); 	
		}


	}

	void MoveSpells()
	{
		foreach (var spell in enemySpells)
		{
			if (spell != null) {
				Vector3 currentPosition = spell.transform.position;
				Vector3 targetPostion = new Vector3 (currentPosition.x - 2.0f, currentPosition.y, currentPosition.z);
				Vector3 destination = targetPostion - currentPosition;
				spell.transform.Translate (destination.x * enemySpellSpeed * Time.deltaTime,
					destination.y * enemySpellSpeed * Time.deltaTime,
					destination.z * enemySpellSpeed * Time.deltaTime,
					Space.World);
			}
		}
	}

	void AddForest(float farhtestForestEndX)
	{
		//1
		int randomForestIndex = Random.Range(0, availableForests.Length);

		//2
		GameObject forest = (GameObject)Instantiate(availableForests[randomForestIndex]);

		//3
		float forestWidth = forest.transform.FindChild("floor").localScale.x;

		//4
		float forestCenter = farhtestForestEndX + forestWidth * 0.5f;

		//5
		forest.transform.position = new Vector3(forestCenter, 0, 0);

		//6
		currentForests.Add(forest);
		++forestsGenerated;
	}

	void AddObject(float lastObjectX)
	{
		//1
		int randomIndex = Random.Range(0, availableObjects.Length);

		//2
		GameObject obj = (GameObject)Instantiate(availableObjects[randomIndex]);

		//3
		float objectPositionX = lastObjectX + Random.Range(objectsMinDistance, objectsMaxDistance);
		float randomY = Random.Range(objectsMinY, objectsMaxY);
		obj.transform.position = new Vector3(objectPositionX,randomY,0); 


		//4
		objects.Add(obj);            
	}

	void AddSpell(float lastSpellX)
	{
		//1
		int randomIndex = Random.Range(0, availableEnemySpells.Length);

		//2
		GameObject spell = (GameObject)Instantiate(availableEnemySpells[randomIndex]);

		//3
		float spellPositionX = lastSpellX + currentForests[0].transform.FindChild("floor").localScale.x * 2;//Random.Range(objectsMinDistance, objectsMaxDistance);
		float playerY = transform.position.y;
		spell.transform.position = new Vector3(spellPositionX,playerY,0); 


		//4
		enemySpells.Add(spell);
	}

	void AddRessource(float lastRessourceX)
	{
		//1
		int randomIndex = Random.Range(0, availableRessources.Length);

		//2
		GameObject ressource = (GameObject)Instantiate(availableRessources[randomIndex]);

		//3
		float ressourcePositionX = lastRessourceX + currentForests[0].transform.FindChild("floor").localScale.x * 3;
		float randomY = Random.Range(spellsMinY, spellsMaxY);
		ressource.transform.position = new Vector3(ressourcePositionX,randomY,0); 


		//4
		ressources.Add(ressource);
	}

	void GenerateSpellsIfRequired()
	{
		//1
		float playerX = transform.position.x;        
		float removeSpellsX = playerX - screenWidthInPoints;
		float addSpellX = playerX;
		float farthestSpellX = 0;
		bool hasToCreate = true;

		//2
		List<GameObject> spellsToRemove = new List<GameObject>();

		foreach (var spell in enemySpells)
		{
			if (spell != null) {
				//3
				float spellX = spell.transform.position.x;

				//4
				farthestSpellX = Mathf.Max (farthestSpellX, spellX);

				//5
				if (spellX < removeSpellsX)
					spellsToRemove.Add (spell);
				
			}

		}

		//6
		foreach (var spell in spellsToRemove)
		{
			enemySpells.Remove(spell);
			Destroy(spell);
		}

		if (farthestSpellX <= playerX)
			farthestSpellX = currentForests [0].transform.FindChild ("floor").localScale.x + currentForests [0].transform.FindChild ("floor").transform.position.x;

		//7
		if (farthestSpellX < addSpellX)
			AddSpell(farthestSpellX);
	}

	void GenerateRessourcesIfRequired()
	{
		//1
		float playerX = transform.position.x;        
		float removeRessourceX = playerX - screenWidthInPoints;
		float addRessourceX = playerX ;
		float farthestRessourceX = 0;

		//2
		List<GameObject> ressourcesToRemove = new List<GameObject>();

		foreach (var ressource in ressources)
		{
			if (ressource != null) {
				//3
				float ressourceX = ressource.transform.position.x;

				//4
				farthestRessourceX = Mathf.Max(farthestRessourceX, ressourceX);

				//5
				if (ressourceX < removeRessourceX)            
					ressourcesToRemove.Add(ressource);
			}

		}

		//6
		foreach (var ressource in ressourcesToRemove)
		{
			ressources.Remove(ressource);
			Destroy(ressource);
		}

		if (farthestRessourceX <= playerX) {
			farthestRessourceX = currentForests [0].transform.FindChild ("floor").localScale.x + currentForests [0].transform.FindChild ("floor").transform.position.x;
		}

		//7
		if (farthestRessourceX < addRessourceX)
			AddRessource(farthestRessourceX);
	}

	void GenerateObjectsIfRequired()
	{
		//1
		float playerX = transform.position.x;        
		float removeObjectsX = playerX - screenWidthInPoints;
		float addObjectX = playerX + screenWidthInPoints;
		float farthestObjectX = 0;

		//2
		List<GameObject> objectsToRemove = new List<GameObject>();

		foreach (var obj in objects)
		{
			//3
			float objX = obj.transform.position.x;

			//4
			farthestObjectX = Mathf.Max(farthestObjectX, objX);

			//5
			if (objX < removeObjectsX)            
				objectsToRemove.Add(obj);
		}

		//6
		foreach (var obj in objectsToRemove)
		{
			objects.Remove(obj);
			Destroy(obj);
		}

		//7
		if (farthestObjectX < addObjectX)
			AddObject(farthestObjectX);
	}

	void GenerateForestIfRequired()
	{
		//1
		List<GameObject> forestsToRemove = new List<GameObject>();

		//2
		bool addForests = true;        

		//3
		float playerX = transform.position.x;

		//4
		float removeForestX = playerX - screenWidthInPoints;        

		//5
		float addForestX = playerX + screenWidthInPoints;

		//6
		float farthestForestEndX = 0;

		foreach(var forest in currentForests)
		{
			//7
			float forestWidth = forest.transform.FindChild("floor").localScale.x;
			float forestStartX = forest.transform.position.x - (forestWidth * 0.5f);    
			float forestEndX = forestStartX + forestWidth;                            

			//8
			if (forestStartX > addForestX)
				addForests = false;

			//9
			if (forestEndX < removeForestX)
				forestsToRemove.Add(forest);

			//10
			farthestForestEndX = Mathf.Max(farthestForestEndX, forestEndX);
		}

		//11
		foreach(var forest in forestsToRemove)
		{
			currentForests.Remove(forest);
			Destroy(forest);            
		}

		//12
		if (addForests)
			AddForest(farthestForestEndX);
	}
}
