using System.Runtime.Serialization;
using System.Text;
using Hospedagem.Models;

Console.OutputEncoding = Encoding.UTF8;

// Cria os modelos de hóspedes e cadastra na lista de hóspedes
List<Pessoa> hospedes = new();

Console.WriteLine(
    "Seja bem-vindo ao sistema de hospedagem!\n" + "\nDigite o nome do locatário (hóspede):"
);

string nomeLocatario;

do // Loop enquanto o nome é nulo, vazio ou espaço em branco.
{
    nomeLocatario = Console.ReadLine();
    Console.Clear();
    if (string.IsNullOrWhiteSpace(nomeLocatario))
    {
        Console.WriteLine("Nome do locatário inválido. Por favor, digite um nome.");
    }
} while (string.IsNullOrWhiteSpace(nomeLocatario));

Pessoa locatario = new(nome: nomeLocatario); // Cria o locatário e o adiciona na lista de hóspedes
hospedes.Add(locatario);

Console.Clear();
Console.WriteLine("Quantos hóspedes serão hospedados? Informe o número.");

int quantidadeHospedes = int.Parse(Console.ReadLine()); // Obtém a quantidade de hóspedes

for (int i = 0; i < quantidadeHospedes; i++) // Loop para cadastrar os hóspedes
{
    string nome;
    string sobrenome;

    // Loop enquanto o nome é nulo, vazio ou espaço em branco.
    do
    {
        Console.WriteLine($"Digite o nome do hóspede {i + 1}:");
        nome = Console.ReadLine();
        Console.Clear();
        if (string.IsNullOrWhiteSpace(nome))
        {
            Console.WriteLine("Nome inválido. Por favor, digite um nome.");
        }
    } while (string.IsNullOrWhiteSpace(nome));

    // Loop enquanto o sobrenome é nulo, vazio ou espaço em branco.
    do
    {
        Console.WriteLine($"Digite o sobrenome do hóspede {i + 1}:");
        sobrenome = Console.ReadLine();
        Console.Clear();
        if (string.IsNullOrWhiteSpace(sobrenome))
        {
            Console.WriteLine("Sobrenome inválido. Por favor, digite um sobrenome.");
        }
    } while (string.IsNullOrWhiteSpace(sobrenome));

    Console.Clear();
    Pessoa hospede = new Pessoa(nome: nome, sobrenome: sobrenome);
    hospedes.Add(hospede);
}

// Cria a suíte
Suite suite = new Suite(tipoSuite: "comum", capacidade: 2, valorDiaria: 30);
Suite suite2 = new Suite(tipoSuite: "prime", capacidade: 4, valorDiaria: 50);
Suite suite3 = new Suite(tipoSuite: "executiva", capacidade: 6, valorDiaria: 90);
Suite suite4 = new Suite(tipoSuite: "master", capacidade: 10, valorDiaria: 130);

// Cria um dicionário para acessar facilmente suites por tipo
Dictionary<string, Suite> suites = new Dictionary<string, Suite>();
suites.Add("comum", suite);
suites.Add("prime", suite2);
suites.Add("executiva", suite3);
suites.Add("master", suite4);

// Obtém o número de dias reservados
Console.Clear();
Console.WriteLine("Perfeito! Quantos dias serão reservados? Informe o número.");
int diasReservados = int.Parse(Console.ReadLine());

// Obtém o tipo de suite
Console.WriteLine("Qual é o tipo de suite? Ex: Comum, Prime, Executiva, Master");
string tipoSuite = Console.ReadLine().ToLower();

// Procura a suite selecionada usando o dicionário
Suite suiteSelecionada;
if (tipoSuite == "comum")
{
    suiteSelecionada = suite;
}
else if (tipoSuite == "prime")
{
    suiteSelecionada = suite2;
}
else if (tipoSuite == "executiva")
{
    suiteSelecionada = suite3;
}
else if (tipoSuite == "master")
{
    suiteSelecionada = suite4;
}
else
{
    Console.WriteLine("Desculpe. Esse tipo de suíte não existe.");
    return;
}

// Verifica a capacidade da suite e a disponibilidade dos hóspedes
if (suites.TryGetValue(tipoSuite, out Suite _))
{
    // Valida a capacidade
    if (hospedes.Count <= suiteSelecionada.Capacidade)
    {
        // Cria uma reserva com a suite selecionada e os hospedes
        Reserva reserva = new Reserva(diasReservados);
        reserva.CadastrarSuite(suiteSelecionada);
        reserva.CadastrarHospedes(hospedes);

        // Calcula e disponibiliza o valor final, exibindo os dados da reserva
        decimal valorFinal = reserva.CalcularValorDiaria();
        Console.Clear();
        Console.WriteLine(
            $"\nHóspede principal: " //
                + $"{nomeLocatario}\t".ToUpper()
                + $"\tNúmero de hóspedes totais: {reserva.ObterQuantidadeHospedes()}\n"
        );

        // Exibe os acompanhantes, a suite e o valor final da reserva
        for (int i = 1; i < hospedes.Count; i++)
        {
            Console.WriteLine($"Acompanhantes: {hospedes[i].NomeCompleto}\n");
        }
        Console.WriteLine(
            $"Alocada a suite {suiteSelecionada.TipoSuite}, com valor diária de {suiteSelecionada.ValorDiaria}.\n"
        );
        Console.WriteLine(
            $"O valor final da reserva com {reserva.DiasReservados} dias reservados é: {valorFinal:G}"
        );
    }
    else // Caso a capacidade da suite seja menor que a quantidade de hóspedes cadastrados
    {
        throw new Exception(
            $"Desculpe, a suite {tipoSuite} tem capacidade máxima para {suiteSelecionada.Capacidade} hóspede(s). Por favor, escolha uma suite com maior capacidade."
        );
    }
}
else
{
    Console.WriteLine("Tipo de suite inválido.");
}
