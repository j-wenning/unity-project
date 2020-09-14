namespace _
{
    public enum StateTag
    {
        Interruptible,
        Interruptor
    }

    public enum StateQualifier
    {
        // Player Enums
        [Alt("Idle")]
        Player_Idle,
        [Alt("Walk")]
        Player_Walk,
        [Alt("Dash")]
        Player_Dash,
        [Alt("Attack_Basic")]
        Player_Attack_Basic,
        [Alt("Attack_Basic_0")]
        Player_Attack_Basic_0,
        [Alt("Attack_Basic_1")]
        Player_Attack_Basic_1,
        [Alt("Attack_Basic_2")]
        Player_Attack_Basic_2,
        Player_Attack_Basic_End
    }
}
