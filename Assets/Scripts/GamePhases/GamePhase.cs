namespace Assets.Scripts
{

    public enum Phase {
        Building, Prepare, Fight, End,
    }

    class GamePhase {
        private Phase _gamePhase;

        public GamePhase () {
            _gamePhase = Phase.Building;
        }

        public void MoveToNextGamePhase() {

            switch (Current) {
                case Phase.Building:
                    _gamePhase = Phase.Prepare;
                    break;
                case Phase.Prepare:
                    _gamePhase = Phase.Fight;
                    break;
                case Phase.Fight:
                    _gamePhase = Phase.End;
                    break;
                case Phase.End:
                    _gamePhase = Phase.Building;
                    break;
            }
        }

        public Phase Current {
            get {
                return _gamePhase;
            }
        }
    }
}
