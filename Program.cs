using System;
using System.Drawing;

string[] menuOptions = {"1. Consultar Tarefas","2. Nova Tarefa","3. Gerenciar Tarefas","4. Sair do Programa"};
int choiceIndex = 0;

while(true)
{
    Console.BackgroundColor = ConsoleColor.DarkCyan;
    Console.ForegroundColor = ConsoleColor.Black;
    Console.Clear();
    Console.WriteLine("                       -------** 1001 TAREFAS **-------");
    Console.WriteLine("Selecione a opção desejada. Use as setas do teclado para mover o cursor e ENTER para selecionar.\n");

    for(int i = 0; i < menuOptions.Length; i++)
    {
        if(choiceIndex == i)
        {
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write(">");
        }
        else
        {
            Console.BackgroundColor = ConsoleColor.DarkCyan;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.Write(" ");
        }
        Console.WriteLine(menuOptions[i]);
    }
    
    ConsoleKeyInfo MoveCursor = Console.ReadKey(true);

    if(MoveCursor.Key == ConsoleKey.UpArrow)
    {
        if(choiceIndex == 0)
        {
            choiceIndex = menuOptions.Length - 1;
        }
        else if(choiceIndex > 0)
        {
            choiceIndex--;
        }
    }
    else if(MoveCursor.Key == ConsoleKey.DownArrow)
    {
        if(choiceIndex == menuOptions.Length - 1)
        {
            choiceIndex = 0;
        }
        else
        {
            choiceIndex++;
        }
    }
}

