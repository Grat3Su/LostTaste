using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SemUmbrellaSkill : Skill, IAttackSkill
{
    [field: SerializeField]
    public int Damage { get; set; }

    [field: SerializeField]
    private Vector3 Scale = new Vector3(5.0f, 5.0f, 5.0f);

    protected override void Init() { }

    protected override IEnumerator SkillCoroutine()
    {
        Root = transform.root;
        GameObject player = transform.root.gameObject;
        PlayerController playerController = player.GetComponent<PlayerController>();
        SemUmbrellaItem item = gameObject.GetComponent<SemUmbrellaItem>();

        yield return null;  // 쿨타임 실행을 위해 1틱 대기

        playerController.StateMachine.ChangeState(new IdleState(playerController));
        
        item.IsOpen = true;
        Managers.Sound.Play("umbrella_open");

        yield return new WaitForSeconds(1.0f);

        ParticleSystem ps = Managers.Effect.Play(Define.Effect.SemUmbrellaSkillEffect, player.transform);
        Transform skillObj = Managers.Resource.Instantiate("Skills/SkillObject").transform;
        skillObj.GetComponent<SkillObject>().SetUp(Root, Damage, _seq);

        skillObj.localScale = Scale;
        skillObj.position = Root.position;

        Washout(player, Color.black);

        yield return new WaitForSeconds(0.2f);

        Washout(player, Color.white);
        Managers.Resource.Destroy(skillObj.gameObject);

        yield return new WaitForSeconds(0.8f);

        item.IsOpen = false;

        yield return new WaitForSeconds(3.0f);
        Managers.Effect.Stop(ps);
    }

    public void Washout(GameObject gameObject, Color color)
    {
        Renderer renderer = gameObject.GetComponent<Renderer>();
        if (renderer != null)
            renderer.material.color = color;

        for (int i=0; i < gameObject.transform.childCount; i++)
        {
            GameObject childObject = gameObject.transform.GetChild(i).gameObject;
            Washout(childObject, color);
        }
    }
}
