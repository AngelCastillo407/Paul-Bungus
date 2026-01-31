using UnityEngine;
using System.Collections;

public class Enemy_Movement : MonoBehaviour
{
    public Material flashMaterial;
    public float flashDuration = 0.15f;

    private Renderer[] renderers;
    private Material[][] originalMaterials;

    private Rigidbody rb;

    private int healthPoints = 2;

    private Vector3 direction;

    public Transform mainCharacter;

    private float moveSpeed = 2f;

    public Animator animator;
    public string attackStateName = "Attack";
    public int frameToTrigger = 5;
    public float clipFrameRate = 24f;
    private bool triggered = false;

    public GameObject prefabToSpawn;
    private float spawnDistance = 1.5f;

    private Vector3 newPosition;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        StartCoroutine(AttackMindlessly());
    }

    IEnumerator AttackMindlessly()
    {
        while (true)
        {
            Attack();
            yield return new WaitForSeconds(2f);
        }
    }

    void Attack()
    {
        animator.SetTrigger("Attack");
    }

    public void SpawnObject()
    {
        Instantiate(prefabToSpawn, newPosition, transform.rotation);
    }

    private void CheckAnimationFrame(string stateName, int targetFrame, System.Action callback)
    {
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

        if (stateInfo.IsName(stateName))
        {
            float clipLength = stateInfo.length;
            float currentTime = stateInfo.normalizedTime * clipLength;
            int currentFrame = Mathf.FloorToInt(currentTime * clipFrameRate);

            if (!triggered && currentFrame >= targetFrame)
            {
                triggered = true;
                callback?.Invoke();
            }
        }
        else
        {
            triggered = false;
        }
    }

    void OnAttackFrame()
    {
        SpawnObject();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Hitbox")
        {
            Flash();

            Vector3 knockDir = transform.position - other.transform.position;
            rb.AddForce(knockDir.normalized * 10f, ForceMode.Impulse);

            healthPoints -= 1;

            if (healthPoints == 0)
            {
                Destroy(gameObject, 0.4f);
                return;
            }

            StartCoroutine(AttackMindlessly());
        }

    }

    void Awake()
    {
        renderers = GetComponentsInChildren<Renderer>(true);
        originalMaterials = new Material[renderers.Length][];

        for (int i = 0; i < renderers.Length; i++)
        {
            originalMaterials[i] = renderers[i].materials;
        }
    }

    public void Flash()
    {
        StopAllCoroutines();
        StartCoroutine(FlashRoutine());
    }

    private IEnumerator FlashRoutine()
    {
        for (int i = 0; i < renderers.Length; i++)
        {
            Material[] mats = renderers[i].materials;
            for (int j = 0; j < mats.Length; j++)
            {
                mats[j] = new Material(flashMaterial);
            }
            renderers[i].materials = mats;
        }

        yield return new WaitForSeconds(flashDuration);

        for (int i = 0; i < renderers.Length; i++)
        {
            renderers[i].materials = originalMaterials[i];
        }
    }

    void Update()
    {
        Vector3 direction = mainCharacter.position - transform.position;
        direction.y = 0f;

        if (direction.sqrMagnitude < 0.0001f)
            return;

        transform.rotation = Quaternion.LookRotation(direction, Vector3.up);

        transform.position += transform.forward * moveSpeed * Time.deltaTime;

        newPosition = transform.position + transform.forward * spawnDistance;

        CheckAnimationFrame(attackStateName, frameToTrigger, OnAttackFrame);
    }
}
