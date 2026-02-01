using UnityEngine;
using System.Collections;

public class Player_Movement : MonoBehaviour
{

    public Material flashMaterial;
    public float flashDuration = 0.15f;

    private Renderer[] renderers;
    private Material[][] originalMaterials;

    private int healthPoints = 6;

    private Rigidbody rb;
    public float speed = 20f;

    private Vector3 newPosition;
    private Vector3 direction;

    public GameObject attackPrefab;
    public GameObject deadCatPrefab;
    private float spawnDistance = 1.5f;

    public Animator animator;

    public string attackStateName = "Attack";
    public int frameToTrigger = 5;
    public float clipFrameRate = 24f;
    private bool triggered = false;

    private bool isKnockedBack = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
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

    public void SpawnObject(GameObject prefabToSpawn)
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
        SpawnObject(attackPrefab);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy Hitbox")
        {
            Flash();

            Vector3 knockDir = (transform.position - other.transform.position).normalized;
            StartCoroutine(Knockback(knockDir));


            healthPoints -= 1;
            if (healthPoints == 0)
            {
                SpawnObject(deadCatPrefab);
                gameObject.SetActive(false);
            }
        }

    }

    IEnumerator Knockback(Vector3 dir)
    {
        isKnockedBack = true;
        rb.linearVelocity = Vector3.zero; // reset velocity
        rb.AddForce(dir * 15f, ForceMode.Impulse);
        yield return new WaitForSeconds(0.2f); // duration of knockback
        isKnockedBack = false;
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

        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        if (horizontalInput != 0 || verticalInput != 0)
        {
            animator.SetBool("Running", true);
        } 
        else
        {
            animator.SetBool("Running", false);
        }

        Vector3 movement = new Vector3(horizontalInput, 0f, verticalInput);

        if (movement.magnitude > 1f)
        {
            movement.Normalize();
        }

        if (!isKnockedBack)
        {
            rb.linearVelocity = movement * speed;
        }

        // 1. Create a ray from the camera through the mouse position
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        // 2. Define a horizontal plane at the character's current Y
        Plane plane = new Plane(Vector3.up, transform.position);

        // 3. Check if the ray hits the plane
        if (plane.Raycast(ray, out float distance))
        {
            // 4. Get the point on the plane where the mouse is pointing
            Vector3 hitPoint = ray.GetPoint(distance);

            // 5. Compute direction from character to mouse
            direction = hitPoint - transform.position;
            direction.y = 0f; // lock rotation to Y axis

            // 6. Ignore tiny movements
            if (direction.sqrMagnitude < 0.0001f)
                return;

            // 7. Rotate to face the mouse
            transform.rotation = Quaternion.LookRotation(direction, Vector3.up);
        }

        newPosition = transform.position + transform.forward * spawnDistance;

        CheckAnimationFrame(attackStateName, frameToTrigger, OnAttackFrame);

        if (Input.GetMouseButtonDown(0))
        {
            animator.SetTrigger("Attack");
        }

    }
}
