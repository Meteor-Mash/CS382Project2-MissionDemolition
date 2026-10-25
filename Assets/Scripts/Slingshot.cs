using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slingshot : MonoBehaviour
{
    [Header("Inscribed")]
    public GameObject projectilePrefab;
    public float velocityMult = 10f;
    public GameObject projLinePrefab;
    public float bandWidth = 0.15f;
    public Material bandMat;
    public AudioClip pullBackClip;
    public AudioClip shootClip;

    [Header("Dynamic")]
    public GameObject launchPoint;
    public Vector3 launchPos;
    public GameObject projectile;
    public bool aimingMode;
    public bool gameOver = false;
    private Transform leftArm;
    private Transform rightArm;
    private LineRenderer leftBand;
    private LineRenderer rightBand;
    private AudioSource audioSource;

    void Awake() {
        Transform launchPointTrans = transform.Find("LaunchPoint");
        launchPoint = launchPointTrans.gameObject;
        launchPoint.SetActive(false);
        launchPos = launchPointTrans.position;

        audioSource = GetComponent<AudioSource>();

        leftArm = transform.Find("LeftArm");
        rightArm = transform.Find("RightArm");

        leftBand = MakeBandSegment("LeftBand", bandMat);
        rightBand = MakeBandSegment("RightBand", bandMat);
        UpdateBand(launchPos);
    }

    LineRenderer MakeBandSegment(string name, Material mat)
    {
        GameObject go = new GameObject(name);
        go.transform.SetParent(transform, false);

        LineRenderer lr = go.AddComponent<LineRenderer>();
        lr.material = mat;
        lr.startWidth = lr.endWidth = bandWidth;
        lr.numCapVertices = 4;
        lr.useWorldSpace = true;
        lr.positionCount = 2;
        return lr;
    }
    
    void UpdateBand(Vector3 projPos)
    {
        leftBand.SetPosition(0, ArmTip(leftArm));
        leftBand.SetPosition(1, projPos);
        rightBand.SetPosition(0, ArmTip(rightArm));
        rightBand.SetPosition(1, projPos);
    }

    Vector3 ArmTip(Transform arm)
    {
        return arm.position + arm.up * arm.lossyScale.y;
    }

    void OnMouseEnter() {
        if (!gameOver) { launchPoint.SetActive(true); }
    }

    void OnMouseExit() {
        launchPoint.SetActive(false);
    }

    void OnMouseDown() {
        if (!gameOver)
        {
            audioSource.clip = pullBackClip;
            audioSource.Play();
            aimingMode = true;
            projectile = Instantiate(projectilePrefab) as GameObject;
            projectile.transform.position = launchPos;
            projectile.GetComponent<Rigidbody>().isKinematic = true;
        }
    }

    void Update() {
        if(!aimingMode) return;

        Vector3 mousePos2D = Input.mousePosition;
        mousePos2D.z = -Camera.main.transform.position.z;
        Vector3 mousePos3D = Camera.main.ScreenToWorldPoint(mousePos2D);

        Vector3 mouseDelta = mousePos3D - launchPos;
        float maxMagnitude = this.GetComponent<SphereCollider>().radius;
        if(mouseDelta.magnitude > maxMagnitude) {
            mouseDelta.Normalize();
            mouseDelta *= maxMagnitude;
        }
        Vector3 projPos = launchPos + mouseDelta;
        projectile.transform.position = projPos;
        UpdateBand(projPos);

        if(Input.GetMouseButtonUp(0)) {
            aimingMode = false;
            Rigidbody projRB = projectile.GetComponent<Rigidbody>();
            projRB.isKinematic = false;
            projRB.collisionDetectionMode = CollisionDetectionMode.Continuous;
            projRB.velocity = -mouseDelta * velocityMult;

            FollowCam.SWITCH_VIEW(FollowCam.eView.slingshot);
            
            FollowCam.POI = projectile; 
            Instantiate<GameObject>(projLinePrefab, projectile.transform);
            projectile = null;
            MissionDemolition.SHOT_FIRED();
            UpdateBand(launchPos);
            audioSource.clip = shootClip;
            audioSource.Play();
        }
    }
}
