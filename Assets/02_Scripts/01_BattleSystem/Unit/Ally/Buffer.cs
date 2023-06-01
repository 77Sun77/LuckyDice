using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buffer : Ally
{
    [Header("Buffer")]
    public bool IsSteadyHealing;
    public float buffValue;

    private void Start()
    {
        first_Setting();
    }

    private void OnEnable()
    {
        if (IsSteadyHealing)
            return;
        StartCoroutine(OnEnable_Cor());
    }

    IEnumerator OnEnable_Cor()
    {
        yield return EnemyGenerator.instance;

        EnemyGenerator.instance.OnWaveStart += HealAllies;
        EnemyGenerator.instance.OnWaveEnd += HealAllies;
    }

    public void OnDisable()
    {
        EnemyGenerator.instance.OnWaveStart -= HealAllies;
        EnemyGenerator.instance.OnWaveEnd -= HealAllies;

        UnApplyDefenseBuff();
    }

    void Update()
    {
        #region legacy Code
        //int layerMask = 1 << LayerMask.NameToLayer("Unit");

        //Vector2 pos = transform.position;

        //pos.x -= 0.875f; // ĭ x/2
        //Vector2 range = detectRange; // ���ο� ����
        //range.x += (detectRange.x * 0.75f) + 1.25f; // ĭ x*ĭ �߰� ����(�÷��̾� : 1, ĭ : 1.75) + (ĭx-0.5)

        //RaycastHit2D[] hits = Physics2D.BoxCastAll(pos, range, 0, Vector2.zero, 0, layerMask);

        //if (hits.Length != 0)
        //{
        //    List<Unit> units = new List<Unit>();
        //    foreach (RaycastHit2D hit in hits)
        //    {
        //        if (hit.collider.gameObject != gameObject) units.Add((Unit)hit.collider.GetComponent(typeof(Unit)));
        //    }

        //    if(this.units.Count != 0)
        //    {
        //        foreach (Unit unit in this.units)
        //        {
        //            if (!units.Contains(unit)) unit.isBuff = false;
        //        }
        //    }
        //    this.units.Clear();
        //    foreach (Unit unit in units)
        //    {
        //        unit.isBuff = true;
        //        this.units.Add(unit);
        //    }

        //}
        #endregion
        Search_Targets();

        if (!IsSteadyHealing)
            return;

        switch (Rating)
        {
            case 1:
                break;
            case 2:
                GiveDefenseBuff();
                break;
            case 3:
                GiveDefenseBuff();
                break;
        }

        if (GameManager.instance.IsInBattle)
        {
            HealAllies();
        }

        time -= Time.deltaTime;
    }

    protected override void Search_Targets()//�Ʊ��� Ÿ������ �ﵵ�� ������
    {
        targets.Clear();

        foreach (var Tile in detectRange_List.GetTileInRange(pawn.X, pawn.Y))
        {
            if (Tile.Ally != null) targets.Add(Tile.Ally);
        }
        isTargetDetected = targets.Count != 0;
    }

    public void HealAllies()
    {
        if (!isTargetDetected || time >= 0)
            return;

        time = delayTime;

        List<Unit> skillTargets = new();

        AOEPos = new Vector2(pawn.X, pawn.Y);

        foreach (var _tile in AOERange_List.GetTileInRange((int)AOEPos.x, (int)AOEPos.y))
        {
            if (_tile.Ally != null)
            {
                skillTargets.Add(_tile.Ally);
            }
            //������ �ӽ� �ڵ�
            _tile.Do_AOE_Effect(Color.green);
        }

        foreach (var Ally in skillTargets)
        {
            Ally.HealHP(damage);
        }
    }

    List<Unit> skillTargets_Last = new();
    //���� ���̺� ����,�ı��� ���� ó������ �������

    public void GiveDefenseBuff()
    {
        List<Unit> skillTargets = new();

        AOEPos = new Vector2(pawn.X, pawn.Y);

        foreach (var _tile in AOERange_List.GetTileInRange((int)AOEPos.x, (int)AOEPos.y))
        {
            if (_tile.Ally != null)
            {
                skillTargets.Add(_tile.Ally);
            }
        }

        foreach (var Ally in skillTargets)
        {
            Ally.TakeDefenseBuff(buffValue);
        }

        for (int i = 0; i < skillTargets.Count; i++)
        {
            if (skillTargets_Last.Contains(skillTargets[i]))
                skillTargets_Last.Remove(skillTargets[i]);     
        }
       
        foreach (var unit_Last in skillTargets_Last)
        {
            unit_Last.isBuff = false;
        }

        skillTargets_Last = skillTargets;
    }

    public void UnApplyDefenseBuff()
    {
        foreach (var target in skillTargets_Last)
        {
            target.isBuff = false;
        }
    }

}
