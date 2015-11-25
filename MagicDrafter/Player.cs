using System;

namespace MagicDrafter
{
    public class Player : BaseObject
    {
        public Player()
        {

        }

        public Player(string piName)
        {
            Name = piName;
            FixUpperLetterName();
            ID = Guid.NewGuid().ToString();
        }

        private void FixUpperLetterName()
        {
            Name = Name.Substring(0, 1).ToUpper() + Name.Substring(1);
        }

        private string ivId;

        public string ID
        {
            get { return ivId; }
            set
            {
                ivId = value;
                FirePropertyChanged();
            }
        }

        private string ivName;

        public string Name
        {
            get { return ivName; }
            set
            {
                ivName = value;
                FirePropertyChanged();
            }
        }


        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            Player other;
            try
            {
                other = (Player)obj;
            }
            catch (Exception)
            {
                return false;
            }

            return other.ID == ID;
        }

        public int Points { get; set; }
        public float OpponentWinPercent { get; set; }
        public float GameWinPercent { get; set; }
        public float OpponentGameWinPercent { get; set; }
        public int Rank { get; set; }

        public override int GetHashCode()
        {
            return Name.GetHashCode();
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
