namespace _
{
    public enum StateTag
    {
        Interruptible,
    }

    public enum StateQualifier
    {
        [Alt("Idle")]
        Player_Idle,
        [Alt("Walk")]
        Player_Walk,
        [Alt("Dash")]
        Player_Dash
    }
}
