﻿using UnityEngine;
using Unity.Profiling;
using System.Collections.Generic;
using Obi;
using Five.String;

[RequireComponent(typeof(ObiActor))]
public class RodChecker : MonoBehaviour
{
    [SerializeField]
    private GameObject _box2DCheckPrefab;
    [SerializeField]
    private Rod _hostRod;

    private static ProfilerMarker m_DrawParticlesPerfMarker = new ProfilerMarker("DrawParticles");

    public bool render = true;
    public Shader shader;
    public Color particleColor = Color.white;
    public float radiusScale = 1;

    private Material material;
    private ParticleImpostorRendering impostors;

    public IEnumerable<Mesh> ParticleMeshes => impostors.Meshes;

    public List<Mesh> listMeshes
    {
        get => impostors.listMeshes;
        set => impostors.listMeshes = value;
    }

    public Material ParticleMaterial => material;

    private bool _isCheckFree;
    public bool isCheckFree
    {
        get => _isCheckFree;
        set => _isCheckFree = value;
    }

    private List<RodCheckBoxCollider2D> _rodCheckBoxCollider2DList = new List<RodCheckBoxCollider2D>();

    private int count = 0;
    private int _rodCheckBoxCollider2DListCount = 0;
    private Collider2D[] colliders;

    [SerializeField]
    private MeshRenderer _meshRenderer;

    private void Start()
    {
        if (!_meshRenderer) _meshRenderer = GetComponent<MeshRenderer>();
        DMCGameUtilities_OnChangeMaterialRope(DMCGameUtilities.MaterialRopeCurrent);
    }

    public void OnEnable()
    {
        impostors = new ParticleImpostorRendering();
        GetComponent<ObiActor>().OnInterpolate += DrawParticles;
        DMCGameUtilities.OnChangeMaterialRope += DMCGameUtilities_OnChangeMaterialRope;
    }

    public void OnDisable()
    {
        GetComponent<ObiActor>().OnInterpolate -= DrawParticles;
        DMCGameUtilities.OnChangeMaterialRope -= DMCGameUtilities_OnChangeMaterialRope;

        if (impostors != null)
            impostors.ClearMeshes();
        DestroyImmediate(material);
    }
    private void DMCGameUtilities_OnChangeMaterialRope(int index)
    {
        var mat = new Material(MenuTheme.instance._materialRope[index]);
        _meshRenderer.materials = new Material[1];
        _meshRenderer.material = mat;
    }

    private void CreateMaterialIfNeeded()
    {
        if (shader != null)
        {
            if (!shader.isSupported)
                Debug.LogWarning("Particle rendering shader not suported.");

            if (material == null || material.shader != shader)
            {
                DestroyImmediate(material);
                material = new Material(shader);
                material.hideFlags = HideFlags.HideAndDontSave;
            }
        }
    }

    private void DrawParticles(ObiActor actor)
    {
        using (m_DrawParticlesPerfMarker.Auto())
        {
            if (!isActiveAndEnabled || !actor.isActiveAndEnabled || actor.solver == null)
            {
                impostors.ClearMeshes();
                return;
            }

            CreateMaterialIfNeeded();

            impostors.UpdateMeshes(actor);

            DrawParticles();
        }
    }

    private void DrawParticles()
    {
        if (material != null)
        {
            material.SetFloat("_RadiusScale", radiusScale);
            material.SetColor("_Color", particleColor);

            // Send the meshes to be drawn:
            if (render)
            {
                foreach (Mesh mesh in impostors.Meshes)
                {
                    Graphics.DrawMesh(mesh, Matrix4x4.identity, material, gameObject.layer);
                }
            }
        }
    }

    private void FixedUpdate()
    {
        //if (!TangleMasterGame.instance.isPlayable) return;
        foreach (Mesh mesh in ParticleMeshes)
        {
            count = 0;
            _rodCheckBoxCollider2DListCount = _rodCheckBoxCollider2DList.Count;
            for (int i = 0; i < mesh.vertices.Length; i += 4)
            {
                if (count < _rodCheckBoxCollider2DListCount)
                {
                    _rodCheckBoxCollider2DList[count].transform.position = new Vector3(mesh.vertices[i].x, mesh.vertices[i].y, 5f);
                }
                else
                {
                    CreateBoxCollider2D().transform.position = new Vector3(mesh.vertices[i].x, mesh.vertices[i].y, 5f);
                }
                count += 1;
            }

            if (count < _rodCheckBoxCollider2DListCount)
            {
                for (int i = count; i < _rodCheckBoxCollider2DListCount; i++)
                {
                    _rodCheckBoxCollider2DList[i].gameObject.SetActive(false);
                }
            }
        }
    }

    private void LateUpdate()
    {
        if (!isCheckFree) return;
        if (!_hostRod.isPluggerBusy && !_hostRod.isFree && _hostRod.curRodState != RodState.unplugged)
        {
            if (CheckFree()) _hostRod.IsFree();
        }
    }

    private bool CheckFree()
    {
        foreach (var rc in _rodCheckBoxCollider2DList)
        {
            if (!rc.gameObject.activeInHierarchy) continue;
            colliders = Physics2D.OverlapBoxAll(rc.transform.position, rc.bc2D.size, 0f);
            if (colliders.Length > 1)
            {
                for (int i = 0; i < colliders.Length; i++)
                {
                    if (colliders[i].gameObject.GetComponent<RodCheckBoxCollider2D>().hostRope != _hostRod)
                    {
                        //FiveDebug.Log(_hostRod.name + " - Not Free At: " + i);
                        return false;
                    }
                }
            }
        }
        //FiveDebug.Log(_hostRod.name + " - Free!");
        return true;
    }

    private Vector3 shadownSize = new Vector3(0.1f, 0.1f, 0.1f);

    private RodCheckBoxCollider2D CreateBoxCollider2D()
    {
        //FiveDebug.Log(_hostRod.name + " - CreateBoxCollider2D");
        RodCheckBoxCollider2D rodCheckBoxCollider2D = Instantiate(_box2DCheckPrefab, transform).GetComponent<RodCheckBoxCollider2D>();
        rodCheckBoxCollider2D.hostRope = _hostRod;
        rodCheckBoxCollider2D.bc2D.size = shadownSize;
        _rodCheckBoxCollider2DList.Add(rodCheckBoxCollider2D);
        return rodCheckBoxCollider2D;
    }

    //#if UNITY_EDITOR
    //    private void OnDrawGizmos()
    //    {
    //        if (!Application.isPlaying) return;
    //        foreach (Mesh mesh in ParticleMeshes)
    //        {
    //            for (int i = 1; i < mesh.vertices.Length; i += 4)
    //            {
    //                Gizmos.color = Color.cyan;
    //                Vector3 tarPos = new Vector3(mesh.vertices[i].x, mesh.vertices[i].y, 5f);
    //                Gizmos.DrawLine(mesh.vertices[i], tarPos);
    //                Gizmos.DrawCube(tarPos, shadownSize);
    //            }
    //        }
    //    }
    //#endif
}
