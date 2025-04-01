using System.Collections;
using UnityEngine;

public class LifeSpawnerManager: MonoBehaviour {

    public LifeSpawner spawner;
    public IEnumerator routine;

    void Start() {
        routine = Drop();
    }

    public void StartRain() {
        StartCoroutine(routine);
    }
    public void StopRain() {
        StopCoroutine(routine);
    }

    public float dropDelay = 0.5f;

    private void RandomizeSpawn() {
        System.Random rand = new System.Random();

        float posX = rand.Next(0, 4) - 1.5f;
        float posZ = rand.Next(0, 4) - 1.5f;

        spawner.transform.localPosition = new Vector3(posX, 0, posZ);
    }

    private IEnumerator Drop() {
        while (true) {
            RandomizeSpawn();
            spawner.Spawn();
            yield return new WaitForSeconds(dropDelay);
        }
    }

}