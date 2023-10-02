using System;
using System.Collections.Generic;
using System.Linq;

namespace Prova1
{
    class Quiosque
    {
        public string Empresa;
        public bool ServeBebida;
        public bool ServeSalgado;
        public bool ServeDoce;
        public string Horario;

        public Quiosque(string empresa, bool serveBebida, bool serveSalgado, bool serveDoce, string horario)
        {
            Empresa = empresa;
            ServeBebida = serveBebida;
            ServeSalgado = serveSalgado;
            ServeDoce = serveDoce;
            Horario = horario;
        }
    }

    class Bloco
    {
        public string Cor;
        public string AlimentoPreferido;
        public string Horario;
        public int NumeroQuiosques;

        public Bloco(string cor, string alimentoPreferido, string horario)
        {
            Cor = cor;
            AlimentoPreferido = alimentoPreferido;
            Horario = horario;
            NumeroQuiosques = 0;
        }
    }

    class Alocacao
    {
        public string EmpresaQuiosque;
        public string CorBloco;

        public Alocacao(string empresaQuiosque, string corBloco)
        {
            EmpresaQuiosque = empresaQuiosque;
            CorBloco = corBloco;
        }
    }

    class Controle
    {
        public List<Quiosque> Quiosques;
        public List<Bloco> Blocos;
        public List<Alocacao> Alocacoes;

        public Controle()
        {
            Quiosques = new List<Quiosque>();
            Blocos = new List<Bloco>();
            Alocacoes = new List<Alocacao>();
        }

        //(0.5 pontos)
        //retorna numero de quiosques que abrem apenas a noite
       public int GetNumeroQuiosquesApenasNoite()
        {
            return Quiosques.Count(q => q.Horario == "Noite"); //retornando os Quiosques(q)
        }

        //(0.5 pontos)
        //retorna string com os nomes dos quiosques (separados por vírgula) que servem salgados pela manhã
        public string GetEmpresasSalgadoManha()
        {
            var quiosquesSalgadoManha = Quiosques.Where(q => q.Horario == "Manha" && q.ServeSalgado).Select(q => q.Empresa);
            return string.Join(", ", quiosquesSalgadoManha);
        }

        //(1 ponto)
        //retorna o alimento preferido pequisado nos blocos 
        public string GetAlimentoMaisPreferido()
        {
            // Cria um dicionário para contar a ocorrência de cada tipo de alimento.
            var alimentoContagem = new Dictionary<string, int>();

            // Percorre através de todos os blocos.
            foreach (var bloco in Blocos)
            {
                 // Verifica se o tipo de alimento já existe no dicionário (Dictionary) de contagem.
                if (!alimentoContagem.ContainsKey(bloco.AlimentoPreferido))
                {
                    alimentoContagem[bloco.AlimentoPreferido] = 1;
                }
                else
                {
                    alimentoContagem[bloco.AlimentoPreferido]++;
                }
            }

            // Obtém o tipo de alimento preferido ao ordenar o dicionário por contagem decrescente(OrderByDescending)
            // e pega o primeiro elemento (que será o tipo de alimento com a maior contagem na pesquisa dosblocos).
            //kv de Keyvalue
            var alimentoMaisPreferido = alimentoContagem.OrderByDescending(kv => kv.Value).FirstOrDefault().Key; 
            //primeiro elemento na contagem mais alta(FirstOrDefault)
            
            return alimentoMaisPreferido;

            //Retorna o alimento preferidop
        }

        //(2 pontos)
        //aloca os quiosques nos blocos obedecendo as seguintes condições:
        //- no máximo dois quiosques por bloco
        //- o quiosque deve servir o alimento preferido do bloco
        //- o bloco tem que estar aberto em todo horário de funcionamento do quiosque (mas não tem problema se o quiosque estiver fechado em parte do horário do bloco)
        //(se quiser, faça funções separadas pra testar cada uma dessas condições)
        //(se não conseguir testar a terceira condição, tente alocar corretamente seguindo pelo menos as duas primeiras)
        //(não é necessário lançar exceção caso algum quiosque não possa ser alocado)
        public void AlocarTodoMundo()
        {
            foreach (var bloco in Blocos)
            {
                var quiosquesDisponiveis = Quiosques
                    .Where(q =>
                        q.ServeSalgado == (bloco.AlimentoPreferido == "Salgado") &&
                        q.Horario.Contains(bloco.Horario) &&
                        !Alocacoes.Any(a => a.CorBloco == bloco.Cor && a.EmpresaQuiosque == q.Empresa))
                    .ToList();
                    //Verificando se o quiosque serve o alimento preferido do bloco


                //para não yter mais que dois no mesmo bloco
                if (quiosquesDisponiveis.Count >= 2)
                {
                    AlocarQuiosque(bloco, quiosquesDisponiveis[0]);
                    AlocarQuiosque(bloco, quiosquesDisponiveis[1]); //V
                }
                else if (quiosquesDisponiveis.Count == 1)
                {
                    AlocarQuiosque(bloco, quiosquesDisponiveis[0]);
                }
            }
        }

        private void AlocarQuiosque(Bloco bloco, Quiosque quiosque)
        {
            Alocacoes.Add(new Alocacao(quiosque.Empresa, bloco.Cor));
            bloco.NumeroQuiosques++;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var controle = new Controle();

            controle.Quiosques.Add(new Quiosque("Bobs", true, true, false, "Manha"));
            controle.Quiosques.Add(new Quiosque("Burger King", true, true, false, "Ambos"));
            controle.Quiosques.Add(new Quiosque("Cabra Café", true, false, false, "Manha"));
            controle.Quiosques.Add(new Quiosque("Cacau Show", false, false, true, "Noite"));
            controle.Quiosques.Add(new Quiosque("Freddo", false, false, true, "Manha"));
            controle.Quiosques.Add(new Quiosque("Giraffas", true, true, false, "Manha"));
            controle.Quiosques.Add(new Quiosque("McDonalds", true, true, false, "Ambos"));
            controle.Quiosques.Add(new Quiosque("Pizza Hut", false, true, false, "Noite"));
            controle.Quiosques.Add(new Quiosque("Ultra Coffee", true, false, true, "Ambos"));
            controle.Quiosques.Add(new Quiosque("Zuka", false, false, true, "Noite"));

            controle.Blocos.Add(new Bloco("Amarelo", "Salgado", "Ambos"));
            controle.Blocos.Add(new Bloco("Azul", "Doce", "Noite"));
            controle.Blocos.Add(new Bloco("Bege", "Salgado", "Noite"));
            controle.Blocos.Add(new Bloco("Branco", "Salgado", "Manha"));
            controle.Blocos.Add(new Bloco("Cinza", "Doce", "Ambos"));
            controle.Blocos.Add(new Bloco("Laranja", "Salgado", "Ambos"));
            controle.Blocos.Add(new Bloco("Marrom", "Salgado", "Manha"));
            controle.Blocos.Add(new Bloco("Verde", "Bebida", "Manha"));
            controle.Blocos.Add(new Bloco("Vermelho", "Doce", "Manha"));
            controle.Blocos.Add(new Bloco("Roxo", "Bebida", "Noite"));

            controle.AlocarTodoMundo();

            Console.WriteLine("O numero de quiosques que abrem apenas a noite eh: " + controle.GetNumeroQuiosquesApenasNoite());
            Console.WriteLine("Os quiosques que servem salgados pela manha sao: " + controle.GetEmpresasSalgadoManha());
            Console.WriteLine("O tipo de alimento mais preferido pelos blocos eh: " + controle.GetAlimentoMaisPreferido());
            Console.WriteLine("Lista de alocacoes:");

            foreach (var alocacao in controle.Alocacoes)
            {
                Console.WriteLine("O quiosque " + alocacao.EmpresaQuiosque + " foi alocado no bloco " + alocacao.CorBloco);
            }
        }
    }
}
