using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardCardRow : MonoBehaviour
{
    public Battalion acceptedType;

    [SerializeField]
    private GameObject hightlightPlane;

    void Awake() {
        hightlightPlane = transform.GetChild(0).gameObject;
        hightlightPlane.SetActive(false);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other) {
        print("Row '" + this.gameObject + "' has collided with '" + other.gameObject + "'");
		if (other.gameObject.GetComponent<Card>()) {
			hightlightPlane.SetActive(true);
		}
	}
	
	void OnTriggerExit(Collider other) {
        print("Row '" + this.gameObject + "' exited collision with '" + other.gameObject + "'");
        hightlightPlane.SetActive(false);
	}
}
