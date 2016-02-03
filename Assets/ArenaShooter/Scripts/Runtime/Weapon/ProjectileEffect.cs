using UnityEngine;
using System.Collections;

public interface ProjectileEffect {

    void OnActivate(Vector3 direction);
    void OnDeactivate();

    void CleanUp();
    bool IsCleanedUp { get; }
    float CleanUpTime { get; }
}
