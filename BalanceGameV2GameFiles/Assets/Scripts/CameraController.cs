using System.Collections;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private float cameraSpeed = 5f;
    [SerializeField] private float cameraSensitivityHorizontal = 5f;
    [SerializeField] private float cameraSensitivityVertical = 5f;
    [SerializeField] private float maximalRotation = 85f;

    private float horizontal;
    private float vertical;
    private bool stopRotation;

    public float flyTime = 0.8f;
    public float fixedRadius = 5f;
    private Vector3 targetPosition;
    private Quaternion targetRotation;
    private bool isMoving;
    private float elapsedTime;
    private KeyboardShortcut[] shortcutArray;
    private GameObject currentTarget;
    private Coroutine moveCoroutine;

    private void Start()
    {
        LoadData();
    }

    private void Update()
    {
        ProcessInput();
        UpdateCameraPosition();
        UpdateCameraRotation();

        if (currentTarget != null && !isMoving)
        {
            transform.position = targetPosition - (transform.forward * fixedRadius);
        }
    }

    private void ProcessInput()
    {
        if (Input.GetKeyDown(KeyCode.Space) && currentTarget != null)
        {
            StopFollowingTarget();
        }

        foreach (KeyboardShortcut shortcut in shortcutArray)
        {
            if (Input.GetKeyDown(shortcut.keyInput))
            {
                GameObject target = currentTarget == null
                    ? FindClosestTarget(shortcut.stringValue)
                    : FindSecondClosestTarget(shortcut.stringValue);

                if (target != null)
                {
                    if (moveCoroutine != null)
                    {
                        StopCoroutine(moveCoroutine);
                    }

                    currentTarget = target;
                    targetPosition = target.transform.position;
                    targetRotation = Quaternion.LookRotation(targetPosition - transform.position);
                    moveCoroutine = StartCoroutine(MoveCamera());
                    break;
                }
            }
        }
    }

    private void UpdateCameraPosition()
    {
        Vector3 movement = new Vector3(
            Input.GetAxisRaw("Horizontal"),
            (Input.GetKey(KeyCode.Space) ? 1 : 0) - (Input.GetKey(KeyCode.LeftShift) ? 1 : 0),
            Input.GetAxisRaw("Vertical"));

        transform.position += transform.TransformDirection(movement) * (cameraSpeed * Time.deltaTime);
    }

    private void UpdateCameraRotation()
    {
        if (Input.GetMouseButtonDown(1)) stopRotation = true;
        if (Input.GetMouseButtonUp(1)) stopRotation = false;

        if (stopRotation) return;

        horizontal += cameraSensitivityHorizontal * Input.GetAxis("Mouse X");
        vertical = Mathf.Clamp(vertical - cameraSensitivityVertical * Input.GetAxis("Mouse Y"), -maximalRotation, maximalRotation);

        transform.eulerAngles = new Vector3(vertical, horizontal, 0f);
    }

    private void LoadData()
    {
        string filePath = Path.Combine(Application.dataPath, "SaveFiles", "Shortcuts.txt");
        if (!File.Exists(filePath)) return;

        BinaryFormatter formatter = new BinaryFormatter();
        using (FileStream stream = new FileStream(filePath, FileMode.Open))
        {
            ShortcutArrayData data = (ShortcutArrayData)formatter.Deserialize(stream);
            shortcutArray = data.shortcutArray;
        }
    }
    private GameObject FindClosestTarget(string tag, int ignoreIndex = -1)
    {
        GameObject[] targets = GameObject.FindGameObjectsWithTag(tag);
        GameObject closest = null;
        float closestDistance = Mathf.Infinity;
        Vector3 currentPosition = transform.position;

        for (int i = 0; i < targets.Length; i++)
        {
            if (i == ignoreIndex) continue;

            float distance = Vector3.Distance(targets[i].transform.position, currentPosition);
            if (distance < closestDistance)
            {
                closest = targets[i];
                closestDistance = distance;
            }
        }

        return closest;
    }

    private GameObject FindSecondClosestTarget(string tag)
    {
        GameObject closest = FindClosestTarget(tag);
        if (closest == null) return null;

        int closestIndex = System.Array.IndexOf(GameObject.FindGameObjectsWithTag(tag), closest);
        return FindClosestTarget(tag, closestIndex);
    }

    private IEnumerator MoveCamera()
    {
        isMoving = true;
        elapsedTime = 0f;

        Vector3 startPosition = transform.position;
        Quaternion startRotation = transform.rotation;

        while (elapsedTime < flyTime)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / flyTime;

            transform.position = Vector3.Lerp(startPosition, targetPosition - (transform.forward * fixedRadius), t);
            transform.rotation = Quaternion.Slerp(startRotation, targetRotation, t);

            yield return null;
        }

        isMoving = false;
    }

    private void StopFollowingTarget()
    {
        currentTarget = null;
    }
}
