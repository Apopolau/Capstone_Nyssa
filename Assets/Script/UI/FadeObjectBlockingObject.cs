using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Source: https://www.youtube.com/watch?v=vmLIy62Gsnk
//Github: https://github.com/llamacademy/urp-fading-standard-shaders/tree/main
public class FadeObjectBlockingObject : MonoBehaviour
{
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private Transform target;
    [SerializeField] private Camera mainCamera;
    [SerializeField][Range(0f, 1f)] private float fadedAlpha = 0.33f;
    [SerializeField] private bool retainShadows = true;
    [SerializeField] private Vector3 targetPositionOffset = Vector3.up;
    [SerializeField] private float fadeSpeed = 1;

    [Header("Read Only Data")]
    [SerializeField]
    private List<ObjectFader> objectsBlockingView = new List<ObjectFader>();
    private Dictionary<ObjectFader, Coroutine> runningCoroutines = new Dictionary<ObjectFader, Coroutine>();

    private RaycastHit[] Hits = new RaycastHit[10];

    private void Start()
    {
        StartCoroutine(CheckForObjects());
    }

    private IEnumerator CheckForObjects()
    {
        while (true)
        {
            int hits = Physics.RaycastNonAlloc(
                mainCamera.transform.position,
                (target.transform.position + targetPositionOffset - mainCamera.transform.position).normalized,
                Hits,
                Vector3.Distance(mainCamera.transform.position, target.transform.position + targetPositionOffset),
                layerMask);
            if(hits > 0)
            {
                for(int i = 0; i < hits; i++)
                {
                    ObjectFader objectFader = GetFadingObjectFromHit(Hits[i]);
                    if(objectFader != null && !objectsBlockingView.Contains(objectFader)){
                        if (runningCoroutines.ContainsKey(objectFader))
                        {
                            if(runningCoroutines[objectFader] != null)
                            {
                                StopCoroutine(runningCoroutines[objectFader]);
                            }

                            runningCoroutines.Remove(objectFader);
                        }
                        runningCoroutines.Add(objectFader, StartCoroutine(FadeObjectOut(objectFader)));
                        objectsBlockingView.Add(objectFader);
                    }
                }
            }
            FadeObjectsNoLongerBeingHit();

            ClearHits();

            yield return null;
        }
    }

    private IEnumerator FadeObjectOut(ObjectFader objectFader)
    {
        foreach(Material material in objectFader.materials)
        {
            material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
            material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
            material.SetInt("_ZWrite", 0);
            material.SetInt("_Surface", 1);

            material.renderQueue = (int)UnityEngine.Rendering.RenderQueue.Transparent;

            material.SetShaderPassEnabled("DepthOnly", false);
            material.SetShaderPassEnabled("SHADOWCASTER", retainShadows);

            material.SetOverrideTag("RenderType", "Transparent");

            material.EnableKeyword("_SURFACE_TYPE_TRANSPARENT");
            material.EnableKeyword("_ALPHAPREMULTIPLY_ON");
        }
        float time = 0;

        while (objectFader.materials[0].color.a > fadedAlpha)
        {
            foreach (Material material in objectFader.materials)
            {
                if (material.HasProperty("_Color"))
                {
                    material.color = new Color(
                        material.color.r,
                        material.color.g,
                        material.color.b,
                        Mathf.Lerp(objectFader.initialAlpha, fadedAlpha, time * fadeSpeed)
                        );
                }
            }

            time += Time.deltaTime;
            yield return null;
        }

        if (runningCoroutines.ContainsKey(objectFader))
        {
            StopCoroutine(runningCoroutines[objectFader]);
            runningCoroutines.Remove(objectFader);
        }
    }

    private IEnumerator FadeObjectIn(ObjectFader objectFader)
    {
        float time = 0;

        while (objectFader.materials[0].color.a < objectFader.initialAlpha)
        {
            foreach (Material material in objectFader.materials)
            {
                if (material.HasProperty("_Color"))
                {
                    material.color = new Color(
                        material.color.r,
                        material.color.g,
                        material.color.b,
                        Mathf.Lerp(fadedAlpha, objectFader.initialAlpha, time * fadeSpeed)
                        );
                }
            }

            time += Time.deltaTime;
            yield return null;
        }

        foreach (Material material in objectFader.materials)
        {
            material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
            material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
            material.SetInt("_ZWrite", 1);
            material.SetInt("_Surface", 0);

            material.renderQueue = (int)UnityEngine.Rendering.RenderQueue.Geometry;

            material.SetShaderPassEnabled("DepthOnly", true);
            material.SetShaderPassEnabled("SHADOWCASTER", true);

            material.SetOverrideTag("RenderType", "Opaque");

            material.DisableKeyword("_SURFACE_TYPE_TRANSPARENT");
            material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
        }

        if (runningCoroutines.ContainsKey(objectFader))
        {
            StopCoroutine(runningCoroutines[objectFader]);
            runningCoroutines.Remove(objectFader);
        }
    }

    private void FadeObjectsNoLongerBeingHit()
    {
        List<ObjectFader> objectsToRemove = new List<ObjectFader>(objectsBlockingView.Count);

        foreach(ObjectFader objectFader in objectsBlockingView)
        {
            bool objectIsBeingHit = false;
            for(int i = 0; i < Hits.Length; i++)
            {
                ObjectFader hitFadingObject = GetFadingObjectFromHit(Hits[i]);
                if(hitFadingObject != null && objectFader == hitFadingObject)
                {
                    objectIsBeingHit = true;
                    break;
                }
            }
            if (!objectIsBeingHit)
            {
                if (runningCoroutines.ContainsKey(objectFader))
                {
                    if (runningCoroutines[objectFader] != null)
                    {
                        StopCoroutine(runningCoroutines[objectFader]);
                    }
                    runningCoroutines.Remove(objectFader);
                }

                runningCoroutines.Add(objectFader, StartCoroutine(FadeObjectIn(objectFader)));
                objectsToRemove.Add(objectFader);
            }
        }

        foreach(ObjectFader removeObject in objectsToRemove)
        {
            objectsBlockingView.Remove(removeObject);
        }
    }

    private void ClearHits()
    {
        System.Array.Clear(Hits, 0, Hits.Length);
    }

    private ObjectFader GetFadingObjectFromHit(RaycastHit hit)
    {
        return hit.collider != null ? hit.collider.GetComponent<ObjectFader>() : null;
    }
}
