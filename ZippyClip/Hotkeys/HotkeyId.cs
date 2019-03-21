namespace ZippyClip.Hotkeys
{
    public struct HotkeyId
    {
        int Id;

        public HotkeyId(int id)
        {
            Id = id;
        }

        public int Value => Id;
    }
}
