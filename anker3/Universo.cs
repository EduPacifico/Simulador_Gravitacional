using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using anker3;

namespace anker3
{
    public class Universo
    {
        public int Quantidade { get; set; }
        public Corpo[] corpos { get; set; }
        public Universo(int Quantidade)
        {
            this.Quantidade = Quantidade;
            this.corpos = new Corpo[Quantidade];
        }
        public void CalcularInteracao(double TempoInteração, int NumeroInteracao)
        {
            double G = 6.674184 * (Math.Pow(10, -11));

            double[] aceleracaoX = new double[NumeroInteracao];
            double[] aceleracaoY = new double[NumeroInteracao];

            for (int i = 0; i < NumeroInteracao; i++)
            {
                if (corpos[i] == null) continue;

                for (int j = 0; j < NumeroInteracao; j++)
                {
                    if (corpos[j] == null) continue;
                    if (corpos[j] == corpos[i]) continue;

                    double dx = corpos[j].PosX - corpos[i].PosX; //dx = distancia em x
                    double dy = corpos[j].PosY - corpos[i].PosY; //dy = distancia em y
                    double distancia = Math.Sqrt(dx * dx + dy * dy);

                    if (distancia <= corpos[i].Raio + corpos[j].Raio)
                    {
                        Colisao(i, j);
                        continue;
                    }

                    double forca = (G * corpos[i].Massa * corpos[j].Massa) / (distancia * distancia);

                    double aceX_i = forca * dx / (distancia * corpos[i].Massa);
                    double aceY_i = forca * dy / (distancia * corpos[i].Massa);

                    double aceX_j = -forca * dx / (distancia * corpos[j].Massa);
                    double aceY_j = -forca * dy / (distancia * corpos[j].Massa);

                    aceleracaoX[i] += aceX_i;
                    aceleracaoY[i] += aceY_i;
                    aceleracaoX[j] += aceX_j;
                    aceleracaoY[j] += aceY_j;
                }

            }
            //alterar posicao e velocidade

            for (int i = 0; i < NumeroInteracao; i++)
            {
                if (corpos[i] == null) continue;

                //v = v0+a*t
                corpos[i].VelX = corpos[i].VelX + aceleracaoX[i] * TempoInteração;
                corpos[i].VelY = corpos[i].VelY + aceleracaoY[i] * TempoInteração;
                //s = s0 + v0*t + a/2*t²
                corpos[i].PosX = corpos[i].PosX + corpos[i].VelX * TempoInteração + ((aceleracaoX[i] / 2) * TempoInteração * TempoInteração);
                corpos[i].PosY = corpos[i].PosY + corpos[i].VelY * TempoInteração + ((aceleracaoY[i] / 2) * TempoInteração * TempoInteração);
            }
        }
        public void Colisao(int i, int j)
        {
            double massaTotal = corpos[i].Massa + corpos[j].Massa;

            double velX = (corpos[i].VelX * corpos[i].Massa + corpos[j].VelX * corpos[j].Massa) / massaTotal;
            double velY = (corpos[i].VelY * corpos[i].Massa + corpos[j].VelY * corpos[j].Massa) / massaTotal;

            double volumeTotal = (corpos[i].Massa / corpos[i].Densidade) + (corpos[j].Massa / corpos[j].Densidade);
            double densidade = massaTotal / volumeTotal;

            double posX = (corpos[i].PosX + corpos[j].PosX) / 2.0;
            double posY = (corpos[i].PosY + corpos[j].PosY) / 2.0;

            corpos[i] = new Corpo(corpos[i].Nome + "+" + corpos[j].Nome, massaTotal, densidade, posX, posY, velX, velY);

            corpos[j] = null;
        }

        public void gerarCorpos(int quantidadeCorpos, double Xmaximo, double Ymaximo, double massaMaxima, double massaMinima)
        {
            var rand = new Random();
            for (int i = 0; i < quantidadeCorpos; i++)
            {
                String Nome = "Corpo" + (i + 1);
                double Massa = massaMinima + (rand.NextDouble() * (massaMaxima - massaMinima));
                double Densidade = rand.NextDouble();
                double PosX = 0 + (rand.NextDouble() * (Xmaximo - 0));
                double PosY = 0 + (rand.NextDouble() * (Ymaximo - 0));
                double VelX = 0;
                double VelY = 0;

                Corpo corpo = new Corpo(Nome, Massa, Densidade, PosX, PosY, VelX, VelY);

                this.corpos[i] = corpo;
            }
        }
    }
}
