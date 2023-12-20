using System;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class MetaballsController : MonoBehaviour
{
    static readonly int MetaballsCount = 8;
    public List<Renderer> _renderers = new List<Renderer>();
    public List<Transform> _transforms = new List<Transform>();
    MaterialPropertyBlock _materialPropertyBlock;

    readonly Vector4[] _particlesPos = new Vector4[MetaballsCount];
    readonly float[] _particlesSize = new float[MetaballsCount];
    
    static readonly int NumParticles = Shader.PropertyToID("_NumParticles");
    static readonly int ParticlesSize = Shader.PropertyToID("_ParticlesSize");
    static readonly int ParticlesPos = Shader.PropertyToID("_ParticlesPos");

    void OnEnable()
    {
        _materialPropertyBlock = new MaterialPropertyBlock();
        _materialPropertyBlock.SetInt(NumParticles, MetaballsCount);
        
        //_renderer = _particleSystem.GetComponent<Renderer>();

        foreach(Renderer renderer in _renderers)
        {
            renderer.SetPropertyBlock(_materialPropertyBlock);
        }

    }

    void OnDisable()
    {
        _materialPropertyBlock.Clear();
        _materialPropertyBlock = null;
        foreach (Renderer renderer in _renderers)
        {
            renderer.SetPropertyBlock(null);
        }
    }

    void Update()
    {

        int i = 0;
        foreach (var transform in _transforms)
        {
            _particlesPos[i] = transform.position;
            _particlesSize[i] = transform.localScale.x * 2;
            ++i;
            
        }
        
        _materialPropertyBlock.SetVectorArray(ParticlesPos, _particlesPos);
        _materialPropertyBlock.SetFloatArray(ParticlesSize, _particlesSize);
        //_materialPropertyBlock.SetInt(NumParticles, _numParticles);

        foreach (Renderer renderer in _renderers)
        {
            renderer.SetPropertyBlock(_materialPropertyBlock);
        }
    }
}
