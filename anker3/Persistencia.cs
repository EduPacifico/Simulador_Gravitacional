using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace anker3
{
    public abstract class Persistencia
    {
        public abstract void Salvar(string caminho, Universo universo, int qtdInteracoes, double tempInteracao);
    }
}
