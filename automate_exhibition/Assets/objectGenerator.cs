using UnityEngine;
using System.Collections;

public class objectGenerator : MonoBehaviour {

	public int minObjects;
	public int maxObjects;
	public GameObject pictureFrame;
    public int minBenches = 0;
    public int maxBenches = 5;
    public GameObject ExhibitionBench;

    public void generateObjects(int type) {
        int objectCount = (int)Mathf.Floor(Random.Range(minObjects, maxObjects));
        for (int i = 0; i < objectCount; i++)
        {
            switch (type)
            {
                case 1:
                    // Installations
                    generateInstallation();
                    i++; // installations can consume more space

                    break;
                case 2:
                    // Sculpture
                    generateSculpture();
                    break;
                case 3:
                    if (Random.value < 0.7f)
                        generateSculpture();
                    else
                    {
                        generateInstallation();
                        i++;
                    }
                    break;
            }
        }

        // Generate Exhibition Benches
        for (int i = 0; i < (int)Mathf.Floor(Random.Range(minBenches, maxBenches)); i++)
        {
            var benchPosition = this.GetComponent<objectDistributor>().getPlace(false, 4);
            if (benchPosition != null)
            {
                Vector3 pos = (Vector3)benchPosition[0];
                pos.y = 0.5f;
                GameObject eb = (GameObject)Instantiate(ExhibitionBench, pos, Quaternion.identity);
            }
            else break;
        }
    }

	public void generatePicture() {

		int objectCount = (int) Mathf.Floor(Random.Range(minObjects,maxObjects));
			
		for (int i = 0; i < objectCount; i++) {
			var pos = this.GetComponent<objectDistributor>().getPlace(true,0);
			if (pos != null) {
				Vector3 direction = (Vector3) pos[1];
				Quaternion newDirection = Quaternion.FromToRotation(Vector3.back, (Vector3) pos[1]);
				Debug.Log(newDirection);
				Debug.Log(direction);

				Instantiate(pictureFrame, (Vector3) pos[0], newDirection);
			}
		}
	}

    private Vector3 getObjBounds(GameObject obj)
    {
        Vector3 center = Vector3.zero;
        Bounds myBounds = new Bounds(center, Vector3.zero);

        foreach (Transform child in obj.transform)
        {
            center += child.GetComponent<Renderer>().bounds.center;
        }

        center /= obj.transform.childCount; //center is average center of children

        foreach (Transform child in obj.transform)
        {
            myBounds.Encapsulate(child.GetComponent<Renderer>().bounds);
        }

        return myBounds.size;
    }

    private void addLight(Vector3 target)
    {
        GameObject lightGameObject = new GameObject("The Light");
        Light lightComp = lightGameObject.AddComponent<Light>();
        lightComp.type = LightType.Spot;
        lightComp.color = Random.ColorHSV(0, 1, 0.8f, 1, 0.8f, 1, 1, 1);
        lightGameObject.transform.position = new Vector3(target.x, 5, target.z);
        lightGameObject.transform.localRotation = Quaternion.Euler(new Vector3(90, 0, 0));
    }

	public int maxInstallationObjects;
	public int minInstallationObjects;
	public int minInstallationSize;
	public int maxInstallationSize;
	public float minIndividualSize;
	public float maxIndividualSize;

	public void generateInstallation() {

		int installationObjectCount = (int) Mathf.Floor(Random.Range(minInstallationObjects,maxInstallationObjects));
		int installationSize = (int) Mathf.Floor(Random.Range(minInstallationSize,maxInstallationSize));
		PrimitiveType[] primitives = new PrimitiveType[4] { 
			PrimitiveType.Capsule, 
			PrimitiveType.Cube, 
			PrimitiveType.Cylinder,
			PrimitiveType.Sphere
		};

        GameObject[] objectsPool = Resources.LoadAll<GameObject>("ExhibitionModels");

        var place = this.GetComponent<objectDistributor>().getPlace(true,0);
		if (place != null) {
			for (int i = 0; i < installationObjectCount; i++) {
				Vector3 origin = (Vector3) place[0];
				Vector3 direction = (Vector3) place[1];
				origin = origin + (direction.normalized * installationSize * 1.5f);
				float individualSize = Random.Range(minIndividualSize,maxIndividualSize);
				Vector3 individualPos = new Vector3(
					origin.x - (installationSize / 2) + (Random.value * installationSize),
					origin.y - 1 + (Random.value * 3),
					origin.z - (installationSize / 2) + (Random.value * installationSize)
				);
                //GameObject obj = GameObject.CreatePrimitive(primitives[(int) Random.Range(0,3)]);
                GameObject obj = (GameObject)Instantiate(objectsPool[Random.Range(0, objectsPool.Length)]);

                Vector3 currentSize = getObjBounds(obj);

                //obj.transform.localScale = new Vector3(1 / myBounds.size.x, 1 / myBounds.size.y, 1 / myBounds.size.z);

                obj.transform.localScale = new Vector3(1 / currentSize.x * individualSize, 1 / currentSize.y * individualSize, 1 / currentSize.z * individualSize);
				obj.transform.position = individualPos;
				obj.transform.rotation = Random.rotation;

			}
		}
	}

    public float podestWidth;
    public float headHeight; 

    private void generateSculpture()
    {

        var place = this.GetComponent<objectDistributor>().getPlace(true,0);
        if (place != null)
        {
            GameObject[] objectsPool = Resources.LoadAll<GameObject>("ExhibitionModels");
            GameObject sculpture = (GameObject)Instantiate(objectsPool[Random.Range(0, objectsPool.Length)]);
            Vector3 currentSize = getObjBounds(sculpture);
            float sizeMax = Mathf.Max(currentSize.x, currentSize.y, currentSize.z);
            float individualSize = 1.0f;
            float scaleFactor = 1 / sizeMax * individualSize;
            sculpture.transform.localScale = new Vector3(scaleFactor, scaleFactor, scaleFactor);

            GameObject podest = GameObject.CreatePrimitive(PrimitiveType.Cube);
            podest.transform.localScale = new Vector3(podestWidth, headHeight - (currentSize.y * scaleFactor), podestWidth);

            Vector3 origin = (Vector3)place[0];
            Vector3 direction = (Vector3)place[1];
            origin = origin + (direction.normalized * 3);

            podest.transform.position = new Vector3(origin.x, podest.transform.localScale.y / 2, origin.z);
            sculpture.transform.position = new Vector3(origin.x, (podest.transform.localScale.y) + (currentSize.y * scaleFactor / 2), origin.z);

            addLight(origin);
        }
    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
