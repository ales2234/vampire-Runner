using UnityEngine;

public class RunningTexture : MonoBehaviour
{
    [SerializeField] private Transform segmentA;
    [SerializeField] private Transform segmentB;
    [SerializeField] private float scrollSpeed = 2f;
    [SerializeField] private float recycleX = -15f;
    [SerializeField] private float overlap = 0.02f;
    [SerializeField] private Spawner spawner;
    [SerializeField] private bool matchSpawnerSpeed = true;
    [SerializeField] private float spawnerSpeedMultiplier = 1f;

    private SpriteRenderer rendererA;
    private SpriteRenderer rendererB;
    private float segmentSpacing;

    private void Awake()
    {
        if (segmentA == null || segmentB == null)
            SetupSegmentsFromChild();

        rendererA = segmentA.GetComponent<SpriteRenderer>();
        rendererB = segmentB.GetComponent<SpriteRenderer>();
        segmentSpacing = GetSpriteWidth(rendererA) - overlap;
    }

    private void Start()
    {
        if (spawner == null)
            spawner = FindFirstObjectByType<Spawner>();

        AlignSegments();
    }

    private void Update()
    {
        if (segmentA == null || segmentB == null)
            return;

        if (GameManager.Instance != null && !GameManager.Instance.isGameOver)
            return;

        float speed = scrollSpeed;
        if (matchSpawnerSpeed && spawner != null)
            speed = spawner._spawnForce * spawnerSpeedMultiplier;

        Vector3 move = Vector3.left * speed * Time.deltaTime;
        segmentA.position += move;
        segmentB.position += move;

        RecycleIfNeeded(segmentA, rendererA, segmentB);
        RecycleIfNeeded(segmentB, rendererB, segmentA);
    }

    private void SetupSegmentsFromChild()
    {
        SpriteRenderer source = GetComponentInChildren<SpriteRenderer>();
        if (source == null)
        {
            Debug.LogWarning("RunningTexture: assign segmentA and segmentB, or add a child SpriteRenderer.");
            return;
        }

        segmentA = source.transform;
        segmentB = Instantiate(source.gameObject, transform).transform;
        segmentB.name = source.name + "_copy";
    }

    private void AlignSegments()
    {
        if (rendererA == null || rendererB == null)
            return;

        segmentSpacing = GetSpriteWidth(rendererA) - overlap;
        segmentB.position = new Vector3(
            segmentA.position.x + segmentSpacing,
            segmentA.position.y,
            segmentA.position.z
        );
    }

    private void RecycleIfNeeded(Transform segment, SpriteRenderer renderer, Transform other)
    {
        if (renderer.bounds.max.x < recycleX)
        {
            segment.position = new Vector3(
                other.position.x + segmentSpacing,
                segment.position.y,
                segment.position.z
            );
        }
    }

    private static float GetSpriteWidth(SpriteRenderer renderer)
    {
        Sprite sprite = renderer.sprite;
        float width = sprite.rect.width / sprite.pixelsPerUnit;
        return width * Mathf.Abs(renderer.transform.lossyScale.x);
    }
}
