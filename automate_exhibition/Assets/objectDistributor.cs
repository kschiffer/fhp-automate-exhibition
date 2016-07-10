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

	public object[] getPlace(bool atWall, float overrideMinDistance) {

		Vector3 randomPoint = new Vector3();
		Vector3 direction = new Vector3();
		bool isValidPoint = false;
		int i = 0;
        Bounds wallBound = new Bounds(Vector3.zero, Vector3.zero);
        float distance = (overrideMinDistance == 0) ? minDistance : overrideMinDistance;


        while (!isValidPoint){

            i++;
            if (atWall)
            {
                GameObject[] walls = GameObject.FindGameObjectsWithTag("wall");
                int randomIndex = (int)Mathf.Floor(Random.Range(0, walls.Length));
                GameObject wall = walls[randomIndex];
                wallBound = wall.GetComponent<Renderer>().bounds;

                randomPoint = new Vector3(
                    Random.Range(wall.transform.position.x - wallBound.extents.x, wall.transform.position.x + wallBound.extents.x),
                    headHeight,
                    Random.Range(wall.transform.position.z + wallBound.extents.z, wall.transform.position.z - wallBound.extents.z)
                );
            } else
            {
                Vector3 roomSize = this.GetComponent<generateRoom>().getRoomSize();
                randomPoint = new Vector3(
                    Random.Range(-(roomSize.x/2.5f), roomSize.x/2.5f),
                    headHeight,
                    Random.Range(-(roomSize.z / 2.5f), roomSize.z / 2.5f)
                );
            }

			bool noViolations = true;

			foreach (Vector3 place in places) {
				if (Vector3.Distance(place, randomPoint) < distance)
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

            if (atWall)
            {
                Vector3 roomSize = this.GetComponent<generateRoom>().getRoomSize();
                Bounds roomBounds = new Bounds(Vector3.zero, (roomSize));

                if (wallBound.extents.x > wallBound.extents.z)
                {
                    direction = Vector3.forward;
                    if (!roomBounds.Contains(randomPoint + direction * 3))
                    {
                        direction = Vector3.back;
                        Debug.Log("outside bounds");
                    }   
                }
                else
                {
                    direction = Vector3.left;
                    if (!roomBounds.Contains(randomPoint + direction * 3))
                    {
                        direction = Vector3.right;
                        Debug.Log("outside bounds");
                    }
                }
            } else
            {
                direction = Vector3.zero;
            }
		}

		if (i>100) {
			return null;
		}

		places.Add(randomPoint);

		return new object[2] {randomPoint, direction};
	}
}
