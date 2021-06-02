using System.Collections;
using UnityEngine;

public class ElectricFence : SmallFry
{
    public float LifeTime;
    public GameObject DeathColliderPrefab;
    public GameObject ElectricParticlePrefab;

	public bool Predetermined;
	public Vector3 PredeterminedHelperPosition;

    private GameObject Helper;
    private GameObject DeathCollider;
    private GameObject Electric;

    public override void Init()
    {
        base.Init();
        DetermineStartTheta();
        StartCoroutine(Activate());
    }

    private void CreateCollider()
    {
        Vector3 deathColliderOffset = (Helper.transform.position - transform.position) / 2.0f;
        float rotationEulerZ = Vector3.SignedAngle(deathColliderOffset, Vector3.right, Vector3.forward);
        if (rotationEulerZ < 180.0f)
        {
            rotationEulerZ = 360.0f - rotationEulerZ;   //magic
        }
        Vector3 deathColliderRotationEuler = new Vector3(0.0f, 0.0f, rotationEulerZ);
        DeathCollider = Instantiate(DeathColliderPrefab, transform.position + deathColliderOffset, Quaternion.Euler(deathColliderRotationEuler));

		float deathColliderOffsetMagnitude = deathColliderOffset.magnitude;
        BoxCollider2D deathColliderBox = DeathCollider.GetComponent<BoxCollider2D>();
        deathColliderBox.size = new Vector2(deathColliderOffsetMagnitude * 2.0f, 0.3f);

        ParticleSystem deathColliderParticle = DeathCollider.transform.GetChild(0).GetComponent<ParticleSystem>();
        ParticleSystem.ShapeModule particleShape = deathColliderParticle.shape;
        particleShape.radius = deathColliderOffsetMagnitude;
    }

    private IEnumerator DeathCountdown(float delay)
    {
        yield return new WaitForSeconds(delay);
        HandleDeath();
    }

    public override void HandleDeath()
    {
        Destroy(Helper);
        Destroy(DeathCollider);
        Destroy(Electric);
        base.HandleDeath();
    }

    private IEnumerator Activate()
    {
        Helper = Instantiate(gameObject, GetHelperSpawnPosition(), Quaternion.identity);
        
        CreateCollider();

        yield return new WaitForSeconds(1.5f);

        DeathCollider.GetComponent<Collider2D>().enabled = true;
        DeathCollider.transform.GetChild(0).gameObject.SetActive(false);
        GetComponent<Collider2D>().enabled = true;
        Helper.GetComponent<Collider2D>().enabled = true;
        
        StartCoroutine(ElectricParticle());
        StartCoroutine(DeathCountdown(LifeTime));
    }

    private IEnumerator ElectricParticle()
    {
        Electric = Instantiate(ElectricParticlePrefab, transform.position, Quaternion.identity);
        float lerpParameter = 0.0f;
        float startTime = Time.time;

        Electric.transform.position = Helper.transform.position;
        yield return null;

        while (true)
        {
            lerpParameter = Mathf.PingPong(Time.time - startTime, 1.0f);
            Vector3 electricPosition = Vector3.Lerp(transform.position, Helper.transform.position, lerpParameter * lerpParameter);
            Electric.transform.position = electricPosition;
            yield return null;
        }
    }

    private  Vector3 GetHelperSpawnPosition()
    {
		if (Predetermined)
		{
			return PredeterminedHelperPosition;
		}

        float helperTheta = (CurrentTheta + 180.0f + Random.Range(-50.0f, 50.0f)) % 360.0f;
        float xCoord = Mathf.Cos(helperTheta * Mathf.Deg2Rad) * 15.0f;
        float yCoord = Mathf.Sin(helperTheta * Mathf.Deg2Rad) * 10.0f;
        return new Vector3(xCoord, yCoord, 0.0f);
    }
}
