using UnityEngine;
using System.Collections;

public class objectGenerator : MonoBehaviour {

	public int minObjects;
	public int maxObjects;
	public GameObject pictureFrame;

	public void generateObjects(int type) {
		if (type == 1) {
			// Installations

			int objectCount = (int) Mathf.Floor(Random.Range(minObjects,maxObjects));

			for (int i = 0; i < objectCount; i++) {
				generateInstallation();
			}
		}
	}

	public void generatePicture() {

		int objectCount = (int) Mathf.Floor(Random.Range(minObjects,maxObjects));
			
		for (int i = 0; i < objectCount; i++) {
			var pos = this.GetComponent<objectDistributor>().getPlace();
			if (pos != null) {
				Vector3 direction = (Vector3) pos[1];
				Quaternion newDirection = Quaternion.FromToRotation(Vector3.back, (Vector3) pos[1]);
				Debug.Log(newDirection);
				Debug.Log(direction);

				Instantiate(pictureFrame, (Vector3) pos[0], newDirection);
			}
		}
	}

	public int maxInstallationObjects;
	public int minInstallationObjects;
	public int minInstallationSize;
	public int maxInstallationSize;
	public int minIndividualSize;
	public int maxIndividualSize;

	public void generateInstallation() {

		int installationObjectCount = (int) Mathf.Floor(Random.Range(minInstallationObjects,maxInstallationObjects));
		int installationSize = (int) Mathf.Floor(Random.Range(minInstallationSize,maxInstallationSize));
		PrimitiveType[] primitives = new PrimitiveType[4] { 
			PrimitiveType.Capsule, 
			PrimitiveType.Cube, 
			PrimitiveType.Cylinder,
			PrimitiveType.Sphere
		};

		var place = this.GetComponent<objectDistributor>().getPlace();
		if (place != null) {
			for (int i = 0; i < installationObjectCount; i++) {
				Vector3 origin = (Vector3) place[0];
				Vector3 direction = (Vector3) place[1];
				origin = origin + (direction.normalized * installationSize);
				Debug.Log(origin);
				float individualSize = Random.Range(minIndividualSize,maxIndividualSize);
				Vector3 individualPos = new Vector3(
					origin.x - (installationSize / 2) + (Random.value * installationSize),
					origin.y + (Random.value * 5),
					origin.z - (installationSize / 2) + (Random.value * installationSize)
				);
				GameObject obj = GameObject.CreatePrimitive(primitives[(int) Random.Range(0,3)]);
				obj.transform.localScale = new Vector3(individualSize, individualSize, individualSize);
				obj.transform.position = individualPos;
				obj.transform.rotation = Random.rotation;

			}
		}

	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
