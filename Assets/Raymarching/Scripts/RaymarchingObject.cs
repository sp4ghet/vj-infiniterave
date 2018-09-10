using UnityEngine;

[ExecuteInEditMode, RequireComponent(typeof(Renderer))]
public class RaymarchingObject : MonoBehaviour
{
    [SerializeField] string shaderName = "Raymarching/Object";

    private Material material_;
    private int scaleId_;
    private float lerp;
    private int hitId_;

    void Awake()
    {
        material_ = new Material(Shader.Find(shaderName));
        GetComponent<Renderer>().material = material_;
        scaleId_ = Shader.PropertyToID("_Scale");
        hitId_ = Shader.PropertyToID("_Hit");
    }
    
    void Update()
    {
        lerp = GlobalState.I.RayMarchLerp;
        //float addSize = AudioReactive.Instance.RmsLow;
        //if (addSize > 25) {
        //    transform.localScale = Vector3.one * 4f + Vector3.one * Mathf.Abs(addSize / 25);
        //}
        //else {
            //transform.localScale = Vector3.one * 4f;
        //}
        material_.SetVector(scaleId_, transform.localScale);
        material_.SetFloat(hitId_, lerp);
    }
}