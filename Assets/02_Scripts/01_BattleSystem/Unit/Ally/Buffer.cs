using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buffer : Ally
{
    List<Unit> units = new List<Unit>();
    private void Start()
    {
        first_Setting();
    }

    private void OnEnable()
    {
        StartCoroutine(OnEnable_Cor());
    }

    IEnumerator OnEnable_Cor()
    {
        yield return EnemyGenerator.instance;

        EnemyGenerator.instance.OnWaveStart += HealAllies;
        EnemyGenerator.instance.OnWaveEnd += HealAllies;
    }


    private void OnDestroy()
    {
        EnemyGenerator.instance.OnWaveStart -= HealAllies;
        EnemyGenerator.instance.OnWaveEnd -= HealAllies;
    }

    void Update()
    {
        #region legacy Code
        //int layerMask = 1 << LayerMask.NameToLayer("Unit");

        //Vector2 pos = transform.position;

        //pos.x -= 0.875f; // Ä­ x/2
        //Vector2 range = detectRange; // »õ·Î¿î º¯¼ö
        //range.x += (detectRange.x * 0.75f) + 1.25f; // Ä­ x*Ä­ Ãß°¡ ¹üÀ§(ÇÃ·¹ÀÌ¾î : 1, Ä­ : 1.75) + (Ä­x-0.5)

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
        if (isTargetDetected && time<0)
        {
            if(GameManager.instance.IsInBattle)
            {
                HealAllies();
                time = delayTime;
            }
        }
        time -= Time.deltaTime;

    }

    protected override void Search_Targets()//¾Æ±ºÀ» Å¸°ÙÀ¸·Î »ïµµ·Ï ÀçÁ¤ÀÇ
    {
        targets.Clear();

        foreach (var Tile in GetTileInRange(pawn.X, pawn.Y, detectRange_List))
        {
            if (Tile.Ally != null) targets.Add(Tile.Ally);
        }
        isTargetDetected = targets.Count != 0;
    }
    
    public void HealAllies()
    {
        List<Unit> _targets = new();

        AOEPos = new Vector2(pawn.X, pawn.Y);

        foreach (var _tile in GetTileInRange((int)AOEPos.x, (int)AOEPos.y, AOERange_List))
        {
            if (_tile.Ally != null)
            {
                _targets.Add(_tile.Ally);
            }
            //µð¹ö±ë¿ë ÀÓ½Ã ÄÚµå
            var tileSR = _tile.GetComponent<SpriteRenderer>();
            Color OriginColor = tileSR.color;
            StartCoroutine(Do_AOE_Effect(_tile, Color.green));
        }

        foreach (var Ally in _targets)
        {
            Ally.HealHP(damage);
        }
    }

}
