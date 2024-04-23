#define CG_Debug

using CG_Biblioteca;
using OpenTK.Graphics.OpenGL4;
using System;

namespace gcgcg
{
    internal class Circulo : Objeto
    {
        public Circulo(Objeto _paiRef, ref char _rotulo) : this(_paiRef, ref _rotulo, 0.5, new Ponto4D(0, 0)) { }

        public Circulo(Objeto _paiRef, ref char _rotulo, double _raio, Ponto4D ptoDeslocamento) : base(_paiRef, ref _rotulo)
        {
            PrimitivaTipo = PrimitiveType.Points;
            PrimitivaTamanho = 5;

            int pontos = 72;
            int angulo = 360 / pontos;
            double raio = 0.5;

            for (int i = 0; i < 360; i += angulo)
            {
                Ponto4D pontoGerado = Matematica.GerarPtosCirculo(i, raio);
                base.PontosAdicionar(pontoGerado);
            }
            Atualizar();
        }

        private void Atualizar()
        {
            ObjetoAtualizar();
        }

#if CG_Debug
        public override string ToString()
        {
            string retorno;
            retorno = "__ Objeto Circulo _ Tipo: " + PrimitivaTipo + " _ Tamanho: " + PrimitivaTamanho + "\n";
            retorno += ImprimeToString();
            return (retorno);
        }
#endif
    }
}
