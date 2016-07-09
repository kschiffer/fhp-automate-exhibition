using UnityEngine;
using System.Collections;

public class objectDistributor : MonoBehaviour {

	private ArrayList places = new ArrayList();
	public float minDistance;
	public float headHeight;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public object[] getPlace() {

		Vector3 randomPoint = new Vector3();
		Vector3 direction = new Vector3();
		bool isValidPoint = false;
		int i = 0;

		while (!isValidPoint){

			i++;
			GameObject[] walls = GameObject.FindGameObjectsWithTag("wall");
			int randomIndex = (int) Mathf.Floor(Random.Range(0,walls.Length));
			GameObject wall = walls[randomIndex];
			Bounds wallBound = wall.GetComponent<Renderer>().bounds;

			randomPoint = new Vector3(
				Random.Range(wall.transform.position.x - wallBound.extents.x, wall.transform.position.x + wallBound.extents.x),
				headHeight,
				Random.Range(wall.transform.position.z + wallBound.extents.z, wall.transform.position.z - wallBound.extents.z)
			);

			bool noViolations = true;

			foreach (Vector3 place in places) {
				if (Vector3.Distance(place, randomPoint) < minDistance)
				{
					//Debug.Log("Violation Found: " + Vector3.Distance(place, randomPoint) + " > " + minDistance);
					noViolations = false;
					break;
				}
			}

			if (i>100)
				Debug.Log("Had to skip");

			if (noViolations || i > 100) {
				isValidPoint = true;
			}

			if (wallBound.extents.x > wallBound.extents.z) {
				direction = Vector3.forward;
			} else {
				direction = Vector3.left;
			}
		}

		if (i>100) {
			return null;
		}

		places.Add(randomPoint);

		return new object[2] {randomPoint, direction};
	}
}
