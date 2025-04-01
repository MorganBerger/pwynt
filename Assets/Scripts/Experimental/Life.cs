using UnityEngine;

public class Life : MonoBehaviour
{
    GameObject splitedPrefab;

    // Start is called before the first frame update
    void Start() {
        splitedPrefab = Resources.Load<GameObject>("Prefabs/Lives/splitedDiamond"); 
    }

    // Update is called once per frame
    void Update() {
         
    }

    void CreateSplits() {
        Rigidbody mainRb = GetComponent<Rigidbody>();
        Material mainMat = GetComponent<Renderer>().materials[0];
        GameObject splits = Instantiate(splitedPrefab);

        splits.transform.SetParent(transform.parent, false);
        splits.transform.position = transform.position;
        splits.transform.rotation = transform.rotation;

        foreach (Transform t in splits.transform) {
            Renderer render = t.GetComponent<Renderer>();
            render.materials = new Material[1] { mainMat } ;

            Rigidbody rb = t.GetComponent<Rigidbody>();
            rb.velocity = mainRb.velocity;
        }

        Destroy(gameObject);
    }

    bool hasCollisionned = false;


    void OnCollisionEnter(Collision collision) {
        if (!hasCollisionned) {
            print("COLLISION");
            hasCollisionned = true;
            CreateSplits();
        }
        // }
    }
    // oncol
}
