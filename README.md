# --- 1001 Tasks --- 
**English speaker? [Click here!](#english-version)** 
# Introdução

1001 Tasks é um programa para acompanhamento de tarefas diárias, atuando como agenda pessoal. Nele é possível cadastrar, acompanhar e gerenciar tarefas e compromissos de maneira simples. 

Este projeto foi criado para praticar conceitos de POO e persistência de dados usando JSON em aplicações .NET CLI. A ideia de projeto veio do site roadmap.sh, cuja proposta se encontra nos projetos de roadmap de [Desenvolvedor Back-End](https://roadmap.sh/backend). No momento em que desenvolvi o programa e estou escrevendo esta documentação, estou começando na stack .NET, então achei uma boa ideia iniciar o projeto e tirar o máximo possível das tecnologias e dos meus conhecimentos.

Para mais detalhes da proposta base: [Task Tracker CLI / Roadmap.sh](https://roadmap.sh/projects/task-tracker)

# Funções Básicas
Hoje, o 1001 Tasks atende os usuários com as seguintes funcionalidades:
- Criar Tarefas detalhadas (Além do título, inserir descrição e data do compromisso)
- Acompanhar suas Tarefas através de Status: "Pendente", "Atrasado" ou "Feito".
- Consultar Tarefas agendadas, utilizando filtros específicos para isso
- Gerenciar suas Tarefas, editando ou removendo com facilidade e segurança.
- Tudo isso em uma interface CLI com seleção dinâmica, tornando mais prática e fácil sua utilização por usuários pouco experientes em tecnologia.

# Tecnologias Usadas
- Aplicação em Console totalmente desenvolvida em C#, no framework .NET 10
- Para persistência de dados, é utilizado um arquivo JSON

# Como Rodar?
Abra o Terminal e digite:
```
git clone https://github.com/joelmcfilho/1001tarefas.git
cd 1001tarefas
dotnet run
```
Caso não tenha as dependências de GIT e os runtimes necessários e queira testar o programa diretamente, disponibizei neste repositório uma build pronta compactada em .ZIP. Basta fazer o download do arquivo, descompactar na sua área de trabalho, e rodar o arquivo executável. 

Nota: Na pasta "Release" do repositório é possível encontrar o arquivo executável final do programa.

Link: [Google Drive](https://drive.google.com/file/d/16kySB7UpqCaxeM1-jVE-zlubQyV-rNFo/view?usp=drive_link)

# Estrutura do Projeto
```
/Enums -> Guarda o Enum de Status de Tarefa
/Models -> Estrutura das Entidades
/Repository -> Camada de acesso e persistência em JSON
/Sources -> Regras de Negócio
Program.cs -> Ponto de entrada do programa
```
# Funcionamento do Sistema
## Fluxo Básico
### Inicialização
O programa irá iniciar já mostrando o Menu Principal, além de informações úteis para o usuário no cabeçalho como: 
- Dia de Hoje (Sistema)
- Número de Tarefas agendadas para Hoje
- Número de Tarefas em Atraso

O usuário irá navegar nas opções do Menu Principal utilizando as teclas CIMA e BAIXO do teclado, e selecionar as opções desejadas.

Toda vez que o programa inicia, ele irá verificar se o arquivo "tasks.json" e a pasta Data existem. Caso não exista, ele irá criar uma no caminho:  `%AppData\Roaming\1001Tasks\tasks.json`

Além disso, o programa ao inicializar também verifica se há alguma tarefa com data anterior a data atual marcada como "Pendente", e a classifica automaticamente como "Atrasada".

Através do Menu Principal, o usuário conseguirá acessar as funcionalidades principais do programa apertando a tecla ENTER. Também poderá encerrar o programa apertando a tecla ESC ou selecionando a opção `"6. Exit Program"`.

### Criar Nova Tarefa

Supondo que o usuário não tenha nenhuma tarefa, com seu banco de dados em branco (pode ser a primeira utilização, por exemplo), ele irá desejar criar uma tarefa. Para isso, deverá escolher a opção `"3. Create New Task"`.

O programa irá pergunta a ele o Título da sua tarefa, uma descrição, e uma data válida (Atual ou posterior). Todos os dados são captados usando o método `StringBuilder()`, para que  caso o usuário aperte a tecla ESC ele possa abortar a operação a qualquer momento.

Após preencher todos os pedidos de input, a Tarefa será armazenada no JSON com sucesso.

O usuário poderá criar quantas tarefas quiser, inclusive podendo repetir o Título, Descrição. 

### Consulta de Tarefas

Caso ele queira consultar as Tarefas armazenadas no sistema, ele irá recorrer a opção `"1. Show Tasks"` no Menu Principal. Ao fazer isso, irá aparecer um submenu indicando as consultas específicas que ele poderá realizar:
- Tarefas para o dia de Hoje
- Todas as tarefas
- Apenas tarefas pendentes
- Apenas tarefas já feitas
- Apenas tarefas em atraso

Uma consulta específica será gerada em cada uma dessas opções ao apertar ENTER. A qualquer momento poderá retornar ao Menu Principal através da tecla ESC.

### Gerenciamento de Tarefas - Edição

Então, o usuário gostaria de retificar alguma informação em uma tarefa existente. Ele irá recorrer a opção `"4. Edit an Task"` e irá aparecer uma lista com todas as tarefas existentes no sistema naquele momento. Ele selecionará a tarefa desejada com ENTER, e fará as edições pontuais que desejar na tarefa. Ao fim, a tarefa será atualizada no JSON com os novos dados alterados. 

### Gerenciamento de Tarefas - Remoção

O usuário que, por algum motivo, deseja remover uma tarefa do sistema, poderá recorrer a opção "5. Delete Task". Irá aparecer uma lista de tarefas existentes no sistema, e o usuário irá escolher com a tecla ENTER a tarefa que deseja remover. Ao fazer isso, haverá uma confirmação com a tecla Y, e a tarefa será removida do JSON. Caso ele queira abortar a operação, poderá apertar a tecla ESC ou qualquer outra tecla exceto o "Y" durante a confirmação de exclusão.

### Finalizar uma Tarefa (Marcar como "Feito")

Quando o usuário quiser, poderá marcar uma tarefa como Feita e tirar do rol de tarefas pendentes. Isso poderá ser feito na opção "2. Mark as DONE" do Menu Principal. Será exibida uma lista de tarefas marcadas como "Pendente" ou "Atrasada" e, ao selecionar com a tecla ENTER, essa tarefa terá seu Status mudado para "Feito" no JSON. 

# Dúvidas?

Sinta-se livre para me chamar no [Discord](https://discord.com/users/cruzfilho)

# English Version:

1001 Tasks is a program for tracking daily tasks, acting as a personal calendar. It allows you to register, track, and manage tasks and appointments in a simple way.

This project was created to practice OOP concepts and data persistence using JSON in .NET CLI applications. The project idea came from the roadmap.sh website, whose proposal can be found in the roadmap projects of [Back-End Developer](https://roadmap.sh/backend). At the time I developed the program and am writing this documentation, I am starting in the .NET stack, so I thought it was a good idea to start the project and get the most out of the technologies and my knowledge.

For more details on the basic proposal: [Task Tracker CLI / Roadmap.sh](https://roadmap.sh/projects/task-tracker)

# Basic Functions
Currently, 1001 Tasks provides users with the following functionalities:
- Create detailed Tasks (In addition to the title, insert a description and the date of the appointment)
- Track your Tasks through Status: "Pending", "Late" or "Done".
- View scheduled Tasks using specific filters
- Manage your Tasks, editing or removing them easily and securely.

- All this in a CLI interface with dynamic selection, making it more practical and easy to use for users with little technological experience.

# Technologies Used
- Console application fully developed in C#, in the .NET 10 framework
- A JSON file is used for data persistence

# How to Run?

Open the Terminal and type:
```
git clone https://github.com/joelmcfilho/1001tarefas.git
cd 1001tarefas
dotnet run
```
If you don't have the necessary GIT dependencies and runtimes and want to test the program directly, I've made a ready-made build of the  *1.0 version*  of this program, compressed in a .ZIP file. Just download the file, extract it to your desktop, and run the executable file.

Note: The final executable file for the program can be found in the "Release" folder of the repository.

Link: [Google Drive](https://drive.google.com/file/d/16kySB7UpqCaxeM1-jVE-zlubQyV-rNFo/view?usp=drive_link)

# Project Structure
```
/Enums -> Stores the Task Status Enum
/Models -> Entity Structure
/Repository -> JSON access and persistence layer
/Sources -> Business Rules
Program.cs -> Program entry point
```
# System Operation
## Basic Flow
### Initialization
The program will start by displaying the Main Menu, as well as useful information for the user in the header such as:
- Today's Date (System)
- Number of Tasks scheduled for Today
- Number of Overdue Tasks

The user will navigate the Main Menu options using the UP and DOWN keys on the keyboard, and select the desired options.

Every time the program starts, it will check if the "tasks.json" file and the Data folder exist. If it doesn't exist, it will create one in the path: `%AppData\Roaming\1001Tasks\tasks.json`

In addition, upon initialization, the program also checks if there are any tasks with a date prior to the current date marked as "Pending", and automatically classifies them as "Late".

Through the Main Menu, the user can access the program's main functionalities by pressing the ENTER key. They can also exit the program by pressing the ESC key or selecting the option `"6. Exit Program"`.

### Creating a New Task

Assuming the user has no tasks, with their database blank (this could be the first time using the program, for example), they will want to create a task. To do this, they should choose the option `"3. Create New Task"`.

The program will ask them for the task's title, a description, and a valid date (current or later). All data is captured using the `StringBuilder()` method, so that if the user presses the ESC key, they can abort the operation at any time.

After filling in all the input requests, the Task will be successfully stored in the JSON.

The user can create as many tasks as they want, including repeating the Title and Description.

### Task Query

If they want to query the Tasks stored in the system, they will use the option "1. Show Tasks" in the Main Menu. Doing so will display a submenu indicating the specific queries they can perform:
- Tasks for Today
- All tasks
- Only pending tasks
- Only completed tasks
- Only overdue tasks

A specific query will be generated for each of these options when pressing ENTER. At any time, they can return to the Main Menu using the ESC key.

### Task Management - Editing

Then, the user would like to correct some information in an existing task. He will use the option "4. Edit a Task" and a list of all tasks currently existing in the system will appear. He will select the desired task with ENTER, and make the necessary edits to the task. Finally, the task will be updated in the JSON with the new data.

### Task Management - Removal

A user who, for some reason, wishes to remove a task from the system can use the option "5. Delete Task". A list of existing tasks in the system will appear, and the user will select the task they wish to remove using the ENTER key. Upon doing so, there will be a confirmation with the Y key, and the task will be removed from the JSON. If they wish to abort the operation, they can press the ESC key or any other key except "Y" during the deletion confirmation.

### Finalizing a Task (Marking as "Done")

Whenever the user wants, they can mark a task as Done and remove it from the list of pending tasks. This can be done in option "2. Mark as DONE" of the Main Menu. A list of tasks marked as "Pending" or "Late" will be displayed, and by selecting one with the ENTER key, that task will have its Status changed to "Done" in the JSON.

# Questions?

Feel free to contact me on [Discord](https://discord.com/users/cruzfilho)

