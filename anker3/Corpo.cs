using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace anker3
{
    public class Corpo
    {
        public string Nome { get; set; }
        public double Massa { get; set; }
        public double Densidade { get; set; }
        public double PosX { get; set; }
        public double PosY { get; set; }
        public double VelX { get; set; }
        public double VelY { get; set; }
        public double Raio { get; set; }

        public Corpo(string Nome, double Massa, double Densidade, double PosX, double PosY, double VelX, double VelY)
        {
            this.Nome = Nome;
            this.Massa = Massa;
            this.Densidade = Densidade;
            this.PosX = PosX;
            this.PosY = PosY;
            this.VelX = VelX;
            this.VelY = VelY;
            this.Raio = Math.Pow((3 * Massa / (4 * Math.PI * Densidade)), 1.0 / 3.0);
        }
    }
}
