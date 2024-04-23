#define CG_Debug

using CG_Biblioteca;
using OpenTK.Graphics.OpenGL4;
using System;

namespace gcgcg
{
    internal class SrPalito : Objeto
    {
        private double _raio = 0.5;
        private double _angulo = 45;

        private Ponto4D pontoPe = new Ponto4D();
        private Ponto4D pontoCabeca = new Ponto4D();

        public SrPalito(Objeto _paiRef, ref char _rotulo) : base(_paiRef, ref _rotulo)
        {
            PrimitivaTipo = PrimitiveType.Lines;
            PrimitivaTamanho = 20;

            // pé criado na origem (ponto vazio) 
            // cabeça criada de acordo com angulo e raio
            pontoCabeca = Matematica.GerarPtosCirculo(_angulo, _raio);

            PontosAdicionar(pontoPe);
            PontosAdicionar(pontoCabeca);

            Atualizar();
        }

        public void Atualizar()
        {
            // substituir o pé 
            base.PontosAlterar(pontoPe, 0);
            // atualizar a cabeçå com o angulo e raio atual
            pontoCabeca = Matematica.GerarPtosCirculo(_angulo, _raio);
            // normalizar cordenada da cabeça com a do pé
            pontoCabeca.X += pontoPe.X;
            // substituir a cabeça
            base.PontosAlterar(pontoCabeca, 1);
            base.ObjetoAtualizar();
        }

        public void Andar(double distancia) {
            // diminuir/aumentar eixo x do pé
            pontoPe = new Ponto4D(pontoPe.X + distancia, pontoPe.Y);
            Atualizar();
        }

        public void MudarTamanho(double raio) {
            _raio += raio;
            Atualizar();
        }

        public void Girar(double angulo) {
            _angulo += angulo;
            Atualizar();
        }

        #if CG_Debug
        public override string ToString()
        {
            string retorno;
            retorno = "__ Objeto SrPalito _ Raio: " + _raio + " _ Ângulo: " + _angulo + "\n";
            retorno += base.ImprimeToString();
            return retorno;
        }
        #endif
    }
}
