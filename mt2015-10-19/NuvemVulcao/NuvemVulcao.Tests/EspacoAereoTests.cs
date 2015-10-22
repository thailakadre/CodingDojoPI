using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NuvemVulcao.Tests
{
    [TestFixture]
    public class EspacoAereoTests
    {

        EspacoAereo subject;

        [SetUp]
        public void Setup()
        { 
            subject = new EspacoAereo(8, 7);
        }

        [Test]
        public void Matriz_eh_inicializada_com_o_tamanho_informado()
        {
            Assert.AreEqual(subject.Representacao, new string[8, 7]);
        }

        [Test]
        public void Nuvem_eh_posicionada_na_posicao_correta()
        {
            subject.PosicionarNuvem(0, 0);

            Assert.AreEqual(EspacoAereo.NUVEM, subject.Representacao[0, 0]);
        }

        [TestCase(10,10)]
        [TestCase(5, 10)]
        [TestCase(-9, 5)]
        [TestCase(8, 7)]
        [TestCase(7, 8)]
        [ExpectedException(typeof(ArgumentException))]
        public void Nuvem_nao_eh_posicionada_posicao_invalida(int coluna, int linha)
        {
            subject.PosicionarNuvem(coluna, linha);
        }

        [Test]
        public void Aeroporto_eh_posicionado_na_posicao_correta()
        {
            subject.PosicionarAeroporto(0, 0);

            Assert.AreEqual(EspacoAereo.AEROPORTO, subject.Representacao[0, 0]);
        }

        [TestCase(10, 10)]
        [TestCase(5, 10)]
        [TestCase(-9, 5)]
        [TestCase(8, 7)]
        [TestCase(7, 8)]
        [ExpectedException(typeof(ArgumentException))]
        public void Aeroporto_nao_eh_posicionad_posicao_invalida(int coluna, int linha)
        {
            subject.PosicionarAeroporto(coluna, linha);
        }
        
        [Test]
        public void Aeroporto_nao_pode_sobreescrever_uma_nuvem()
        {
            subject.PosicionarNuvem(1, 1);
            subject.PosicionarAeroporto(1, 1);

            Assert.AreEqual(EspacoAereo.NUVEM, subject.Representacao[1, 1]);
        }

        [Test]
        public void Nuvem_pode_sobreescrever_um_aeroporto()
        {
            subject.PosicionarAeroporto(1, 1);
            subject.PosicionarNuvem(1, 1);

            Assert.AreEqual(EspacoAereo.NUVEM, subject.Representacao[1, 1]);
        }

        [Test]
        public void Expandir_nuvem_em_todas_as_direcoes()
        {
            subject.PosicionarNuvem(3, 3);

            subject.Expandir();

            Assert.AreEqual(EspacoAereo.NUVEM, subject.Representacao[3, 3]);
            Assert.AreEqual(EspacoAereo.NUVEM, subject.Representacao[2, 3]);
            Assert.AreEqual(EspacoAereo.NUVEM, subject.Representacao[3, 2]);
            Assert.AreEqual(EspacoAereo.NUVEM, subject.Representacao[3, 4]);
            Assert.AreEqual(EspacoAereo.NUVEM, subject.Representacao[4, 3]);

            VerificarSeNaoEhNuvem(51);
        }

        [Test]
        public void Expandir_nuvem_em_uma_direcao_invalida_para_coluna()
        {
            subject.PosicionarNuvem(0, 3);

            subject.Expandir();

            Assert.AreEqual(EspacoAereo.NUVEM, subject.Representacao[0, 3]);
            Assert.AreEqual(EspacoAereo.NUVEM, subject.Representacao[0, 2]);
            Assert.AreEqual(EspacoAereo.NUVEM, subject.Representacao[0, 4]);
            Assert.AreEqual(EspacoAereo.NUVEM, subject.Representacao[1, 3]);

            VerificarSeNaoEhNuvem(52);
        }

        [Test]
        public void Expandir_nuvem_em_uma_direção_invalida()
        {
            subject.PosicionarNuvem(0, 0);

            subject.Expandir();

            Assert.AreEqual(EspacoAereo.NUVEM, subject.Representacao[0, 1]);
            Assert.AreEqual(EspacoAereo.NUVEM, subject.Representacao[1, 0]);
            Assert.AreEqual(EspacoAereo.NUVEM, subject.Representacao[0, 0]);

            VerificarSeNaoEhNuvem(53);
        }

        
        private void VerificarSeNaoEhNuvem(int esperadoNaoNuvem)
        {
            int totalItensNaoNuvem = 0;
            foreach (var item in subject.Representacao)
            {
                if (item != EspacoAereo.NUVEM)
                    totalItensNaoNuvem++;
            }

            Assert.AreEqual(esperadoNaoNuvem, totalItensNaoNuvem);
        }

        [Test]
        public void Calcular_dias_com_apenas_um_aeroporto()
        {
            subject.PosicionarNuvem(0, 0);
            subject.PosicionarAeroporto(7, 6);

            var resultadoDaPrevisao = subject.CalcularDias();

            var qtdeDeDiasParaCobrirAeroporto = 13;

            Assert.AreEqual(qtdeDeDiasParaCobrirAeroporto, resultadoDaPrevisao.QtdeDiaPrimeiroAeroporto);
            Assert.AreEqual(qtdeDeDiasParaCobrirAeroporto, resultadoDaPrevisao.QtdeDiaTodosAeroportos);
        }

        [Test]
        public void Calcular_dias_com_mais_de_um_aeroporto()
        {
            subject.PosicionarNuvem(3, 3);
            subject.PosicionarAeroporto(0, 2);
            subject.PosicionarAeroporto(7, 6);
            subject.PosicionarAeroporto(3, 5);

            var resultadoDaPrevisao = subject.CalcularDias();

            var qtdeDeDiasParaCobrirTodosAeroportos = 7;
            var qtdeDeDiasParaCobrirPrimeiroAeroporto = 2;

            Assert.AreEqual(qtdeDeDiasParaCobrirPrimeiroAeroporto, resultadoDaPrevisao.QtdeDiaPrimeiroAeroporto);
            Assert.AreEqual(qtdeDeDiasParaCobrirTodosAeroportos, resultadoDaPrevisao.QtdeDiaTodosAeroportos);
        }

        [Test]
        public void Calcular_dias_com_mais_de_uma_nuvem()
        {

            subject.PosicionarNuvem(0, 3);
            subject.PosicionarNuvem(6, 1);
            subject.PosicionarNuvem(4, 4);
            subject.PosicionarAeroporto(7, 3);

            var resultadoDaPrevisao = subject.CalcularDias();

            var qtdeDeDiasParaCobrirTodosAeroportos = 3;
            var qtdeDeDiasParaCobrirPrimeiroAeroporto = 3;

            Assert.AreEqual(qtdeDeDiasParaCobrirPrimeiroAeroporto, resultadoDaPrevisao.QtdeDiaPrimeiroAeroporto);
            Assert.AreEqual(qtdeDeDiasParaCobrirTodosAeroportos, resultadoDaPrevisao.QtdeDiaTodosAeroportos);
        }


        [Test]
        public void Calcular_dias_com_mais_de_uma_nuvem_e_mais_de_um_aeroporto()
        {

            subject.PosicionarNuvem(0, 3);
            subject.PosicionarNuvem(6, 1);
            subject.PosicionarNuvem(4, 4);
            subject.PosicionarAeroporto(7, 3);
            subject.PosicionarAeroporto(2, 0);
            subject.PosicionarAeroporto(1, 6);

            var resultadoDaPrevisao = subject.CalcularDias();

            var qtdeDeDiasParaCobrirTodosAeroportos = 5;
            var qtdeDeDiasParaCobrirPrimeiroAeroporto = 3;

            Assert.AreEqual(qtdeDeDiasParaCobrirPrimeiroAeroporto, resultadoDaPrevisao.QtdeDiaPrimeiroAeroporto);
            Assert.AreEqual(qtdeDeDiasParaCobrirTodosAeroportos, resultadoDaPrevisao.QtdeDiaTodosAeroportos);
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void Calcular_dias_sem_aeroporto_sem_nuvens()
        {
            var resultadoDaPrevisao = subject.CalcularDias();
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void Calcular_dias_sem_aeroporto_com_nuvens()
        {
            subject.PosicionarNuvem(0, 3);
            subject.PosicionarNuvem(6, 1);

            var resultadoDaPrevisao = subject.CalcularDias();
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void Calcular_dias_com_aeroporto_sem_nuvens()
        {
            subject.PosicionarAeroporto(7, 3);
            subject.PosicionarAeroporto(2, 0);

            var resultadoDaPrevisao = subject.CalcularDias();
        
        }
        
    }
}
