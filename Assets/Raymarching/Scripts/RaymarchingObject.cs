using UnityEngine;

[ExecuteInEditMode, RequireComponent(typeof(Renderer))]
public class RaymarchingObject : MonoBehaviour
{
    [SerializeField] string shaderName = "Raymarching/Object";

    private Material material_;
    private int scaleId_;

    void Awake()
    {
        material_ = new Material(Shader.Find(shaderName));
        GetComponent<Renderer>().material = material_;
        scaleId_ = Shader.PropertyToID("_Scale");
    }
    
    void Update()
    {
        material_.SetVector(scaleId_, transform.localScale);
    }
}