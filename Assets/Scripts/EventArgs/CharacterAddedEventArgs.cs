namespace EventArgs
{
    public class CharacterAddedEventArgs : System.EventArgs
    {
        public char Character { get; }
        public int Index { get; }

        public CharacterAddedEventArgs(char character, int index)
        {
            Character = character;
            Index = index;
        }
    }
}