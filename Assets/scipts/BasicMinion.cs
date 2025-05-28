namespace scipts
{
    public class BasicMinion : MinionCard
    {
        protected override void Awake()
        {
            base.Awake();
        
            // 设置卡牌基本信息
            CardName = "普通随从";
            Description = "一个没有任何特殊效果的随从。";
            ManaCost = 3;  // 3费随从
            Attack = 3;    // 3点攻击力
            Health = 3;    // 3点生命值
            MaxHealth = 3; // 最大生命值也是3
        }
    
        // 由于没有任何特殊效果，我们不需要重写任何方法
        // 所有基础功能都由MinionCard基类提供
    }
}