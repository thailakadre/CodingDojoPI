using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NuvemVulcao
{
    public class EspacoAereo
    {
        public const string NUVEM = "*";
        public const string AEROPORTO = "A";

        public string[,] Representacao { get; set; }
        private int LimiteColuna { get; set; }
        private int LimiteLinha { get; set; }
        private int ContadorDeDias { get; set; }
        private int QtdeDeNuvens { get; set; }
        private int QtdeDeAeroportos { get; set; }
        private int QtdeDeAeroportosCobertosPelasNuvens { get; set; }
        private Previsao ResultadoDaPrevisao { get; set; }

        public EspacoAereo(int colunas, int linhas)
        {
            Representacao = new string[colunas, linhas];
            LimiteColuna = colunas;
            LimiteLinha = linhas;
            QtdeDeAeroportos = 0;
            QtdeDeAeroportosCobertosPelasNuvens = 0;
            ContadorDeDias = 0;
            QtdeDeNuvens = 0;
            ResultadoDaPrevisao = new Previsao();
        }

        public void PosicionarNuvem(int coluna, int linha)
        {
            if (PosicaoInvalida(coluna, linha))
                throw new ArgumentException("Fora do limite da Matriz");
                
            Representacao[coluna, linha] = NUVEM;
            QtdeDeNuvens++;
        }

        public void PosicionarAeroporto(int coluna, int linha)
        {
            if (PosicaoInvalida(coluna, linha))
                throw new ArgumentException("Fora do limite da Matriz");

            if (Representacao[coluna, linha] != NUVEM)
            {
                Representacao[coluna, linha] = AEROPORTO;
                QtdeDeAeroportos++;
            }
        }

        private bool PosicaoInvalida(int coluna, int linha)
        {
            return !(coluna < LimiteColuna && linha < LimiteLinha
                            && coluna >= 0 && linha >= 0);
        }

        public void Expandir()
        {
            var representacaoAtual = new string[LimiteColuna, LimiteLinha];
            Array.Copy(Representacao, representacaoAtual, Representacao.Length);

            for(var coluna = 0; coluna < LimiteColuna; coluna++)
                for (var linha = 0; linha < LimiteLinha; linha++)
                {
                    if(representacaoAtual[coluna, linha] == NUVEM)
                    {
                       List<Tuple<int, int>> posicoes = RetornaPosicoesAdjentes(coluna, linha);

                       foreach (var posicao in posicoes)
                       {
                           if (!PosicaoInvalida(posicao.Item1, posicao.Item2))
                           {
                               if (Representacao[posicao.Item1, posicao.Item2] == AEROPORTO)
                               {
                                   QtdeDeAeroportosCobertosPelasNuvens++;

                                   if (QtdeDeAeroportosCobertosPelasNuvens == 1)
                                       ResultadoDaPrevisao.QtdeDiaPrimeiroAeroporto = ContadorDeDias;
                                   
                                   if (QtdeDeAeroportosCobertosPelasNuvens == QtdeDeAeroportos)
                                       ResultadoDaPrevisao.QtdeDiaTodosAeroportos = ContadorDeDias;
                               }
                               Representacao[posicao.Item1, posicao.Item2] = NUVEM;
                           }
                       }
                    }
                }

        }

        public Previsao CalcularDias()
        {
            if (QtdeDeNuvens > 0 && QtdeDeAeroportos > 0)
            {
                while (QtdeDeAeroportos != QtdeDeAeroportosCobertosPelasNuvens)
                {
                    ContadorDeDias++;
                    Expandir();
                }
            }
            else
            {
                throw new ArgumentException("Para cálculo é necessário existir pelo menos uma nuvem e um aeroporto");
            }

            return ResultadoDaPrevisao;
        }

        private List<Tuple <int, int>> RetornaPosicoesAdjentes(int coluna, int linha)
        {
            List<Tuple<int, int>> result = new List<Tuple<int, int>>();

            result.Add(new Tuple<int, int>(coluna,linha + 1));
            result.Add(new Tuple<int, int>(coluna, linha - 1));
            result.Add(new Tuple<int, int>(coluna + 1, linha));
            result.Add(new Tuple<int, int>(coluna - 1, linha));

            return result;
        }
    }
}
