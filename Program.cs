// static void MenuPrincipal()
// {
//     Console.WriteLine("---ESCOLHA UMA OPÇÃO---");
//     Console.WriteLine("[C]onsultar Tarefas");
//     Console.WriteLine("[N]ova Tarefa");
//     Console.WriteLine("[G]erenciar Tarefas");
//     Console.WriteLine("[S]air");
//     ConsoleKeyInfo escolha = Console.ReadKey();
//     switch(escolha.KeyChar)
//     {
//         case 'A':
//         case 'a':
//             break;
        
//         case "N":
//             break;
//         case "G":
//             break;
//         case "S":
//             break;
//     }
// }

List<string> menuOptions = new List<string> {"1. Consultar Tarefas","2. Nova Tarefa","3. Gerenciar Tarefas","4. Sair do Programa"};
int choiceIndex = 0;

while(true)
{
    Console.Clear();
    Console.WriteLine("                       -------** 1001 TAREFAS **-------");
    Console.WriteLine("Selecione a opção desejada. Use as setas do teclado para mover o cursor e ENTER para selecionar.\n\n");

    for(int i = 0; i < menuOptions.Count; i++)
    {
        Console.WriteLine(menuOptions[i]);
    }
    Console.ReadKey();
}