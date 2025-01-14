using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class IceKingSpikePattern : Pattern
{
    private IceKingController _controller;
    private Transform _sectorLoc;

    protected override void Init()
    {
        PatternName = "Spike";
        _controller = GetComponent<IceKingController>();
    }

    IEnumerator IceSpike(int attackDamage)
    {
        yield return new WaitForSeconds(1.55f);
        Root = _controller.transform;

        // SkillObject에서 관리
        _sectorLoc = Managers.Resource.Instantiate("Skills/SkillObject").transform;
        _sectorLoc.GetComponent<SkillObject>().SetUp(Root, attackDamage, _seq);

        _sectorLoc.localScale = new Vector3(10.0f, 5.0f, 10.0f);    // 5.0f
        _sectorLoc.position = Root.transform.TransformPoint(Vector3.zero);
        _sectorLoc.position = new Vector3(_sectorLoc.position.x, Root.position.y, _sectorLoc.position.z);
        _sectorLoc.rotation = Root.rotation;

        ParticleSystem _particleSystem = Managers.Effect.Play(Define.Effect.IceKingSpikeEffect, Root);
        _particleSystem.transform.position = _sectorLoc.transform.position;

        yield return new WaitForSeconds(0.2f);

        Managers.Resource.Destroy(_sectorLoc.gameObject);

        yield return new WaitForSeconds(0.7f);
        
        Managers.Effect.Stop(_particleSystem);
    }

    public override void DeActiveCollider()
    {
    }
    public override void SetCollider(int attackDamage)
    {
        StartCoroutine(IceSpike(attackDamage));
    }

    public override void SetCollider()
    {
        throw new System.NotImplementedException();
    }
}

