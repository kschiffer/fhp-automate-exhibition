using UnityEngine;
using System.Collections;

public class generateRoom : MonoBehaviour {

	public int maxWidth = 40;
	public int minWidth = 10;
	public int maxLength = 40;
	public int minLength = 10;
	public int maxHeight = 5;
	public int minHeight = 3;
	public float wallThickness = .1f;
	public Material wallMaterial;
	public Material groundMaterial;
	public int maxSupplementaryWalls = 10;
	public int minSupplementaryWalls = 3;
	public int minObjects = 10;
	public int maxObjects = 30;

	private class WallValues {
		public bool otherDirection;
		public bool oppositeSide;
		public float currentDivision;
	}

	void generateBasicRoom() {
		float width;
		float height;
		float length;


		// Calculate Room Dimensions

		width = Mathf.Floor(Random.Range(minWidth, maxWidth));
		height = Mathf.Floor(Random.Range(minHeight,maxHeight));
		length = Mathf.Floor(Random.Range(minLength,maxLength));
		Debug.Log(width);
		Debug.Log(height);
		Debug.Log(length);


		// Generate Ground

		GameObject ground = GameObject.CreatePrimitive(PrimitiveType.Cube);
		ground.transform.localScale = new Vector3(width, wallThickness, length);
		ground.GetComponent<MeshRenderer>().material = groundMaterial;


		// Generate Room Walls

		GameObject wall1 = GameObject.CreatePrimitive(PrimitiveType.Cube);
		GameObject wall2 = GameObject.CreatePrimitive(PrimitiveType.Cube);
		GameObject wall3 = GameObject.CreatePrimitive(PrimitiveType.Cube);
		GameObject wall4 = GameObject.CreatePrimitive(PrimitiveType.Cube);

		wall1.GetComponent<MeshRenderer>().material = wallMaterial;
		wall2.GetComponent<MeshRenderer>().material = wallMaterial;
		wall3.GetComponent<MeshRenderer>().material = wallMaterial;
		wall4.GetComponent<MeshRenderer>().material = wallMaterial;

		wall1.transform.localScale = new Vector3(wallThickness, height, length);
		wall2.transform.localScale = new Vector3(wallThickness, height, length);
		wall1.transform.position = new Vector3(width/2,height/2,0);
		wall2.transform.position = new Vector3(-(width/2),height/2,0);

		wall3.transform.localScale = new Vector3(width, height, wallThickness);
		wall4.transform.localScale = new Vector3(width, height, wallThickness);
		wall3.transform.position = new Vector3(0,height/2,length/2);
		wall4.transform.position = new Vector3(0,height/2,-(length/2));

		wall1.tag = "wall";
		wall2.tag = "wall";
		wall3.tag = "wall";
		wall4.tag = "wall";

		Debug.Log(width*length);


		// --- Supplementary Walls --- //

		float supplementaryWallsCount = Mathf.Floor(Random.Range(minSupplementaryWalls,maxSupplementaryWalls));
		float sizeCoefficient = width*length / 800;
		float xDivisions = Mathf.Ceil(Random.Range(4+sizeCoefficient,6+sizeCoefficient));
		float wallLength;
		WallValues[] lastWallValues = new WallValues[(int) supplementaryWallsCount];

		for (int i = 0; i < supplementaryWallsCount; i++) {

			bool otherDirection = (Random.value >= .5);
			bool oppositeSide = (Random.value >= .5);
			float currentDivision = Mathf.Ceil(Random.Range(0,xDivisions-1));
			bool alreadyExists = false;

			Debug.Log(otherDirection);
			Debug.Log(oppositeSide);
			Debug.Log(currentDivision);
			Debug.Log(xDivisions);


			for (int j = 0; j < i; j++) {
				WallValues wallValues = lastWallValues[j];
				if (
					wallValues.otherDirection == otherDirection && 
					wallValues.oppositeSide == oppositeSide && 
					wallValues.currentDivision == currentDivision
				) {
					alreadyExists = true;
					break;
				}
			}
			if (alreadyExists){
				i = i - 1;
				Debug.Log("Already there");
				continue;
			}


			lastWallValues[i] = new WallValues(){
				otherDirection = otherDirection,
				oppositeSide = oppositeSide,
				currentDivision = currentDivision
			};

			GameObject wall = GameObject.CreatePrimitive(PrimitiveType.Cube);
			wall.GetComponent<MeshRenderer>().material = wallMaterial;
			wall.tag = "wall";

			if (!otherDirection) {

				// one direction

				float xPosUnit = currentDivision / xDivisions * width;
				Debug.Log(xPosUnit, wall);

				wallLength = Mathf.Floor(length*Random.Range(.1f,.6f));
				wall.transform.localScale = new Vector3(wallThickness, height, wallLength);

				if (oppositeSide)
					wall.transform.position = new Vector3(-(width/2) + xPosUnit, height/2, -(length/2) + wallLength/2);
				else
					wall.transform.position = new Vector3(-(width/2) + xPosUnit, height/2, (length/2) - wallLength/2);

			} else {

				// other direction

				Debug.Log("other direction", wall);

				float yPosUnit = currentDivision / xDivisions * length;
				Debug.Log(yPosUnit, wall);

				wallLength = Mathf.Floor(width*Random.Range(.1f,.6f));
				wall.transform.localScale = new Vector3(wallLength, height, wallThickness);

				if (oppositeSide)
					wall.transform.position = new Vector3(-(width/2) + wallLength/2, height/2, -(length/2) + yPosUnit);
				else
					wall.transform.position = new Vector3((width/2) - wallLength/2, height/2, -(length/2) + yPosUnit);
			}
		}

	}

	// Use this for initialization
	void Start () {
		generateBasicRoom();
		this.GetComponent<objectGenerator>().generateObjects(1);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
