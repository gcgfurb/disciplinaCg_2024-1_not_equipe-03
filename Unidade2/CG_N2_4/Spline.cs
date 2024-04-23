#define CG_Debug

using CG_Biblioteca;
using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;

namespace gcgcg
{
    internal class Spline : Objeto
    {
        private char rotulo;
        private Shader shaderCiano = new Shader("Shaders/shader.vert", "Shaders/shaderCiano.frag");
        private Shader shaderAmarelo = new Shader("Shaders/shader.vert", "Shaders/shaderAmarela.frag");
        private Shader shaderVermelho = new Shader("Shaders/shader.vert", "Shaders/shaderVermelha.frag");
        private Shader shaderBranco = new Shader("Shaders/shader.vert", "Shaders/shaderBranca.frag");
        private Ponto[] pontosControle = new Ponto[4];
        private int numPontosBezier = 10;
        private int pontoSelecionado = 0;
        private Ponto4D[] pontosOrigem = {
            new(-0.5, -0.5),
            new(-0.5, 0.5),
            new(0.5, 0.5),
            new(0.5, -0.5)   
        };
        private List<Ponto4D> listaPontos = new List<Ponto4D>();

        public Spline(Objeto paiRef, char _rotulo) : base(paiRef, ref _rotulo)
        {
            this.rotulo = _rotulo;
            foreach (var ponto in pontosOrigem)
            {
                listaPontos.Add(ponto);
            }

            PrimitivaTipo = PrimitiveType.Lines;
      
            DesenharPoliedro();
            DesenharPontosControle();
            DesenharSpline();

            Atualizar();
        }

        public void Atualizar()
        {
            base.ObjetoAtualizar();
        }

        private void DesenharPontosControle() {
            for(var i = 0; i < pontosControle.Length; i++) 
            {
                pontosControle[i] = new Ponto(this, ref rotulo, listaPontos[i]);
                base.FilhoAdicionar(pontosControle[i]);
                if (i == pontoSelecionado) 
                {
                    pontosControle[i].shaderObjeto = shaderVermelho;
                } else 
                {
                    pontosControle[i].shaderObjeto = shaderBranco;
                }
                pontosControle[i].ObjetoAtualizar();
            }

            for(var i = 0; i <= numPontosBezier; ++i) 
            {
                PontosAdicionar(new Ponto4D());
            }
        }

        private void DesenharPoliedro() {
            SegReta segReta1 = new SegReta(this, ref rotulo, listaPontos[0], listaPontos[1]);
            segReta1.shaderObjeto = shaderCiano;
            SegReta segReta2 = new SegReta(this, ref rotulo, listaPontos[1], listaPontos[2]);
            segReta2.shaderObjeto = shaderCiano;
            SegReta segReta3 = new SegReta(this, ref rotulo, listaPontos[2], listaPontos[3]);
            segReta3.shaderObjeto = shaderCiano;
        }

        private void DesenharSpline() {
            Ponto4D pontoInicial = listaPontos[0];

            for(var t = 0; t <= numPontosBezier; t++) 
            {
                Ponto4D p1p2 = CalcularPontoCurva(listaPontos[0], listaPontos[1], t);
                Ponto4D p2p3 = CalcularPontoCurva(listaPontos[1], listaPontos[2], t);
                Ponto4D p3p4 = CalcularPontoCurva(listaPontos[2], listaPontos[3], t);
                Ponto4D p1p2p3 = CalcularPontoCurva(p1p2, p2p3, t);
                Ponto4D p2p3p4 = CalcularPontoCurva(p2p3, p3p4, t);
                Ponto4D p1p2p3p4 = CalcularPontoCurva(p1p2p3, p2p3p4, t);
                
                SegReta spline = new SegReta(this, ref rotulo, pontoInicial, p1p2p3p4);
                spline.shaderObjeto = shaderAmarelo;
                pontoInicial = p1p2p3p4;
            }
        }

      private Ponto4D CalcularPontoCurva(Ponto4D p1, Ponto4D p2, int t)
        {
            Ponto4D pontoCalculado = new Ponto4D();
            pontoCalculado.X = p1.X + (p2.X - p1.X) * t / numPontosBezier;
            pontoCalculado.Y = p1.Y + (p2.Y - p1.Y) * t / numPontosBezier;
            return pontoCalculado;
        }

        public void SplineQtdPto(int inc)
        {
           this.numPontosBezier += inc;
           base.LimparObjetosLista();
           DesenharSpline();
           DesenharPontosControle();
           DesenharPoliedro();
           Atualizar();
        }

        public void AtualizarSpline(Ponto4D ptoInc, bool proximo = false)
        {
            if (proximo) 
            {
                pontosControle[pontoSelecionado].shaderObjeto = shaderBranco;
                pontoSelecionado = pontoSelecionado >= 3 ? 0 : ++pontoSelecionado;
                pontosControle[pontoSelecionado].shaderObjeto = shaderVermelho;
            }

            pontosControle[pontoSelecionado].PontosAlterar(pontosControle[pontoSelecionado].PontosId(0) + ptoInc, 0);
            pontosControle[pontoSelecionado].ObjetoAtualizar();
            listaPontos[pontoSelecionado] = new Ponto4D(pontosControle[pontoSelecionado].PontosId(0) + ptoInc);
            base.LimparObjetosLista();
            DesenharPontosControle();
            DesenharSpline();
            DesenharPoliedro();
            Atualizar();
        }

        public void Reset() 
        {
            for (int i = 0; i < listaPontos.Count; i++)
            {
                listaPontos[i] = pontosOrigem[i];
            }

            numPontosBezier = 10;
            pontoSelecionado = 0;

            base.LimparObjetosLista();

            DesenharPontosControle();
            DesenharSpline();
            DesenharPoliedro();
            Atualizar();
        }

        #if CG_Debug
        public override string ToString()
        {
            string retorno;
            retorno = "__ Objeto Spline _ Tipo: " + PrimitivaTipo + " _ Tamanho: " + PrimitivaTamanho + "\n";
            retorno += ImprimeToString();
            return retorno;
        }
        #endif
    }
}
