using System.Data;
using UnityEngine;

public class LifeSpawner: MonoBehaviour {

    public GameObject lifeGO; 

    public Material[] colors;

    public void Spawn() {
        GameObject life = Instantiate(lifeGO);

        life.transform.SetParent(transform.parent, false);
        life.transform.position = transform.position;
        
        System.Random rand = new System.Random();

        int randx = rand.Next(0, 360);
        int randy = rand.Next(0, 360);
        int randz = rand.Next(0, 360);

        int randColor = rand.Next(0, 3);

        print(randColor);
        print("color: " + colors[randColor]);
        print("rotation: (" + randx + ", " + randy + ", " + randz + ")");

        life.GetComponent<Renderer>().materials = new Material[1] { colors[randColor] };

        life.transform.eulerAngles = new Vector3(
            randx, randy, randz
        );
    }
}