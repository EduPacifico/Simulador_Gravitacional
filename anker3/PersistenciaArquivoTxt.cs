using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace anker3
{
    public class PersistenciaArquivoTxt : Persistencia
    {
        public override void Salvar(string caminho, Universo universo, int qtdInteracoes, double tempInteracao)
        {
            using (StreamWriter sw = new StreamWriter(caminho))
            {
                //Primeira linha
                sw.WriteLine($"{universo.Quantidade};{qtdInteracoes};{tempInteracao}");
                //Resto das linhas
                foreach(var corpo in universo.corpos)
                {
                    if (corpo == null) continue;
                    sw.WriteLine(
                        $"{corpo.Nome};" +
                        $"{corpo.Massa.ToString()};" +
                        $"{corpo.Raio.ToString()};" +
                        $"{corpo.PosX.ToString()};" +
                        $"{corpo.PosY.ToString()};" +
                        $"{corpo.VelX.ToString()};" +
                        $"{corpo.VelY.ToString()}"
                    );
                }
            }
        }
    }
}
