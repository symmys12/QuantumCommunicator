using Microsoft.Xna.Framework;

namespace QuantumCommunicator.Helper 
{
    public class NamedCoordinate
    {
        public string Name { get; set; }
        public Vector2 Coordinates { get; set; }

        public NamedCoordinate(string name, Vector2 coordinates)
        {
            Name = name;
            Coordinates = coordinates;
        }
    }
}