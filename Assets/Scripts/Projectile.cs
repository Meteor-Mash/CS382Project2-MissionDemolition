using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Projectile : MonoBehaviour {
    private Rigidbody rigid;

    void Awake() {
        rigid = GetComponent<Rigidbody>();
    }

    // True while the projectile is still moving
    public bool awake {
        get { return !rigid.IsSleeping(); }
    }
}