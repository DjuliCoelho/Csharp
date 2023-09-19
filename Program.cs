namespace Lista3
{
	class Voo
	{
		public int      Id;
		public DateTime HorarioPartida;
		public string   LocalDestino;
		public int      TotalAssentos;
		public int      AssentosOcupados;
		
		public Voo(int id, DateTime horarioPartida, string localDestino, int totalAssentos)
		{
			Id                  = id;
			HorarioPartida      = horarioPartida;
			LocalDestino        = localDestino;
			TotalAssentos       = totalAssentos;
			AssentosOcupados    = 0;
		}
	}
	
	class Passageiro
	{
		public int    Id;
		public string Nome;
		
		public Passageiro(int id, string nome)
		{
			Id   = id;
			Nome = nome;
		}
	}
	
	class Reserva
	{
		public int Id;
		public int IdPassageiro;
		public int IdVoo;
		
		public Reserva(int id, int idPassageiro, int idVoo)
		{
			Id           = id;
			IdPassageiro = idPassageiro;
			IdVoo        = idVoo;
		}
	}
	
	class Aeroporto
	{
		public int              UniqueId;
		public List<Voo>        Voos;
		public List<Passageiro> Passageiros;
		public List<Reserva>    Reservas;
		
		public Aeroporto()
		{
			UniqueId     = 0;
			Voos         = new List<Voo>();
			Passageiros  = new List<Passageiro>();
			Reservas     = new List<Reserva>();
		}
		
		public int GetUniqueId()
		{
			UniqueId++;
			return UniqueId;
		}
		
		public Passageiro GetPassageiroPorId(int id)
		{
			foreach(var passageiro in Passageiros)
			{
				if(passageiro.Id == id)
				{
					return passageiro;
				}
			}
			throw new Exception("passageiro com id " + id + " nao encontrado");
		}
		
		public Passageiro GetPassageiroPorNome(string nome)
		{
			foreach(var passageiro in Passageiros)
			{
				if(passageiro.Nome == nome)
				{
					return passageiro;
				}
			}
			throw new Exception("passageiro com nome " + nome + " nao encontrado");
		}
		
		public Voo GetVooPorId(int id)
		{
			foreach(var voo in Voos)
			{
				if(voo.Id == id)
				{
					return voo;
				}
			}
			throw new Exception("voo com id " + id + " nao encontrado");
		}
		
		public Voo GetVooPorHorario(DateTime horario)
		{
			foreach(var voo in Voos)
			{
				if(voo.HorarioPartida == horario)
				{
					return voo;
				}
			}
			throw new Exception("nenhum voo programado para " + horario.ToString());
		}
		
		public void CadastrarPassageiro(string nome)
		{
			var passageiro = new Passageiro(GetUniqueId(), nome);
			Passageiros.Add(passageiro);
		}
		
		public void CadastrarVoo(DateTime horarioPartida, string localDestino, int totalAssentos)
		{
			var voo = new Voo(GetUniqueId(), horarioPartida, localDestino, totalAssentos);
			Voos.Add(voo);
		}
		
		public void ReservarVoo(string nome, DateTime horario)
		{
			var passageiro = GetPassageiroPorNome(nome);
			var voo = GetVooPorHorario(horario);
			if(voo.AssentosOcupados == voo.TotalAssentos)
			{
				throw new Exception("o voo agendado para " + voo.HorarioPartida + " nao possui mais assentos disponiveis");
			}
			voo.AssentosOcupados++;
			var reserva = new Reserva(GetUniqueId(), passageiro.Id, voo.Id); 
			Reservas.Add(reserva);
		}
		
		public string ListarReservas()
		{
			string output = "";
			foreach(var reserva in Reservas)
			{
				var passageiro = GetPassageiroPorId(reserva.IdPassageiro);
				var voo = GetVooPorId(reserva.IdVoo);
				output += passageiro.Nome + ", " + voo.HorarioPartida.ToString() + ", " + voo.LocalDestino + ", " + voo.AssentosOcupados + "/" + voo.TotalAssentos + "\n";
			}
			return output;
		}
		
		public void CancelarVoo(DateTime horario)
		{
			 try
    {
        // Encontre o voo a ser cancelado com base no horário
        Voo vooCancelado = GetVooPorHorario(horario);

        // Remove o voo da lista de voos
        Voos.Remove(vooCancelado);

        // Encontre outros voos com o mesmo destino
        var voosComMesmoDestino = Voos.FindAll(v => v.LocalDestino == vooCancelado.LocalDestino);

        // Encontre todas as reservas associadas ao voo cancelado
        var reservasDoVooCancelado = Reservas.FindAll(r => r.IdVoo == vooCancelado.Id);

        // Mova os passageiros das reservas do voo cancelado para os próximos voos com o mesmo destino
        foreach (var reserva in reservasDoVooCancelado)
        {
            var passageiro = GetPassageiroPorId(reserva.IdPassageiro);
            foreach (var voo in voosComMesmoDestino)
            {
                if (voo.AssentosOcupados < voo.TotalAssentos)
                {
                    voo.AssentosOcupados++;
                    var novaReserva = new Reserva(GetUniqueId(), passageiro.Id, voo.Id);
                    Reservas.Add(novaReserva);
                    break;
                }
            }
        }

        // Remova as reservas do voo cancelado
        Reservas.RemoveAll(r => r.IdVoo == vooCancelado.Id);

        Console.WriteLine("Voo cancelado com sucesso!");
    }
    catch (Exception error)
    {
        Console.WriteLine(error.Message);
    }
		}
	}
	// coemntando para eu não bigodarrrr
	class Program
	{
		static void Main(string[] args)
		{
			try
			{
				Aeroporto aeroporto = new Aeroporto();
				
				aeroporto.CadastrarVoo(new DateTime(2022, 09, 26, 13, 0, 0), "Sao Paulo",      8);
				aeroporto.CadastrarVoo(new DateTime(2022, 09, 26, 14, 0, 0), "Rio de Janeiro", 8);
				aeroporto.CadastrarVoo(new DateTime(2022, 09, 26, 15, 0, 0), "Porto Alegre",   8);
				aeroporto.CadastrarVoo(new DateTime(2022, 09, 26, 16, 0, 0), "Fortaleza",      8);
				aeroporto.CadastrarVoo(new DateTime(2022, 09, 26, 17, 0, 0), "Manaus",         8);
				aeroporto.CadastrarVoo(new DateTime(2022, 09, 27,  8, 0, 0), "Porto Alegre",   8);
				aeroporto.CadastrarVoo(new DateTime(2022, 09, 27,  9, 0, 0), "Sao Paulo",      8);
				aeroporto.CadastrarVoo(new DateTime(2022, 09, 27, 10, 0, 0), "Sao Paulo",      8);
				aeroporto.CadastrarVoo(new DateTime(2022, 09, 27, 11, 0, 0), "Rio de Janeiro", 8);
				aeroporto.CadastrarVoo(new DateTime(2022, 09, 27, 12, 0, 0), "Fortaleza",      8);
				
				aeroporto.CadastrarPassageiro("Aldair");
				aeroporto.CadastrarPassageiro("Bebeto");
				aeroporto.CadastrarPassageiro("Branco");
				aeroporto.CadastrarPassageiro("Cafu");
				aeroporto.CadastrarPassageiro("Parreira");
				aeroporto.CadastrarPassageiro("Dunga");
				aeroporto.CadastrarPassageiro("Gilmar");
				aeroporto.CadastrarPassageiro("Jorginho");
				aeroporto.CadastrarPassageiro("Junior");
				aeroporto.CadastrarPassageiro("Leonardo");
				aeroporto.CadastrarPassageiro("Marcio");
				aeroporto.CadastrarPassageiro("Mauro");
				aeroporto.CadastrarPassageiro("Mazinho");
				aeroporto.CadastrarPassageiro("Muller");
				aeroporto.CadastrarPassageiro("Paulo");
				aeroporto.CadastrarPassageiro("Rai");
				aeroporto.CadastrarPassageiro("Ricardo");
				aeroporto.CadastrarPassageiro("Romario");
				aeroporto.CadastrarPassageiro("Ronaldao");
				aeroporto.CadastrarPassageiro("Ronaldo");
				aeroporto.CadastrarPassageiro("Taffarel");
				aeroporto.CadastrarPassageiro("Viola");
				aeroporto.CadastrarPassageiro("Zagallo");
				aeroporto.CadastrarPassageiro("Zetti");
				aeroporto.CadastrarPassageiro("Zinho");
				
				aeroporto.ReservarVoo("Aldair",   new DateTime(2022, 09, 26, 14, 0, 0));
				aeroporto.ReservarVoo("Bebeto",   new DateTime(2022, 09, 26, 13, 0, 0));
				aeroporto.ReservarVoo("Branco",   new DateTime(2022, 09, 26, 13, 0, 0));
				aeroporto.ReservarVoo("Cafu",     new DateTime(2022, 09, 26, 14, 0, 0));
				aeroporto.ReservarVoo("Parreira", new DateTime(2022, 09, 26, 15, 0, 0));
				aeroporto.ReservarVoo("Dunga",    new DateTime(2022, 09, 26, 14, 0, 0));
				aeroporto.ReservarVoo("Gilmar",   new DateTime(2022, 09, 26, 15, 0, 0));
				aeroporto.ReservarVoo("Jorginho", new DateTime(2022, 09, 26, 14, 0, 0));
				aeroporto.ReservarVoo("Junior",   new DateTime(2022, 09, 26, 14, 0, 0));
				aeroporto.ReservarVoo("Leonardo", new DateTime(2022, 09, 26, 14, 0, 0));
				aeroporto.ReservarVoo("Marcio",   new DateTime(2022, 09, 26, 16, 0, 0));
				aeroporto.ReservarVoo("Mauro",    new DateTime(2022, 09, 26, 13, 0, 0));
				aeroporto.ReservarVoo("Mazinho",  new DateTime(2022, 09, 26, 13, 0, 0));
				aeroporto.ReservarVoo("Muller",   new DateTime(2022, 09, 26, 13, 0, 0));
				aeroporto.ReservarVoo("Paulo",    new DateTime(2022, 09, 26, 16, 0, 0));
				aeroporto.ReservarVoo("Rai",      new DateTime(2022, 09, 26, 13, 0, 0));
				aeroporto.ReservarVoo("Ricardo",  new DateTime(2022, 09, 27,  9, 0, 0));
				aeroporto.ReservarVoo("Romario",  new DateTime(2022, 09, 26, 16, 0, 0));
				aeroporto.ReservarVoo("Ronaldao", new DateTime(2022, 09, 26, 14, 0, 0));
				aeroporto.ReservarVoo("Ronaldo",  new DateTime(2022, 09, 26, 13, 0, 0));
				aeroporto.ReservarVoo("Taffarel", new DateTime(2022, 09, 26, 16, 0, 0));
				aeroporto.ReservarVoo("Viola",    new DateTime(2022, 09, 26, 13, 0, 0));
				aeroporto.ReservarVoo("Zagallo",  new DateTime(2022, 09, 26, 15, 0, 0));
				aeroporto.ReservarVoo("Zetti",    new DateTime(2022, 09, 26, 16, 0, 0));
				aeroporto.ReservarVoo("Zinho",    new DateTime(2022, 09, 27,  9, 0, 0));
				
				Console.WriteLine(aeroporto.ListarReservas());
				
				aeroporto.CancelarVoo(new DateTime(2022, 09, 26, 14, 0, 0));
				aeroporto.CancelarVoo(new DateTime(2022, 09, 26, 13, 0, 0));
				
				Console.WriteLine(aeroporto.ListarReservas());
			}
			catch(Exception error)
			{
				Console.WriteLine(error.Message);
			}
		}
	}
}