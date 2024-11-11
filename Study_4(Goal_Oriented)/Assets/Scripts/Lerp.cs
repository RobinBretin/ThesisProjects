using UnityEngine;
using TMPro;
using static UnityEngine.ParticleSystem;

public class Lerp : MonoBehaviour
{
    [SerializeField]
    private Transform StartPoint;
    [SerializeField]
    private Transform EndPoint;
    private Vector3 HendPoint;
    [SerializeField]
    private float MoveSpeed;
    [SerializeField]
    private MinMaxCurve Curve;

    private float distance;

    private float remainingDistance;

    private void Start()
    {
        HendPoint = new Vector3(EndPoint.transform.position.x, transform.position.y, EndPoint.transform.position.z);
        distance = Vector3.Distance(StartPoint.position, HendPoint);
        remainingDistance = distance;
    }
    void FixedUpdate()
    {
        GoTo(transform, EndPoint, MoveSpeed);

    }

    private void GoTo (Transform StartPoint, Transform EndPoint, float MoveSpeed)
    {

        if (remainingDistance > 0)
        {
            transform.position = Vector3.Lerp(StartPoint.position, HendPoint, Curve.Evaluate(1- (remainingDistance / distance)));
            remainingDistance -= MoveSpeed * Time.deltaTime;
        }
    }

    private void Land()
    {
        transform.position = new Vector3(transform.position.x, Mathf.Lerp(transform.position.y, 0, Time.deltaTime), transform.position.z);

} 



}
