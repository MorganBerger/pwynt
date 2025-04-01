using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticlesContainer : MonoBehaviour
{
    public ParticleSystem[] systems;

    void Awake() {
        systems = GetComponentsInChildren<ParticleSystem>();
        Stop();
    }
    
    void Start() {
        
    }

    public void Stop() {
        print("Stoping particle systems");
        foreach (var system in systems) {
            system.Stop();
        }
    }

    public void Play() {
        print("Playing particle systems");
        foreach (var system in systems) {
            system.Play();
        }
    }
}
