# ds-tarefas - Gerenciamento de tarefas

Abaixo segue os passos para executar os projetos de backend e frontend do gerenciador de tarefas

## Backend

### Requisitos
- .Net 9
- Banco de dados SQL Server

### Passos
1. Acessar a pasta Backend
2. Na pasta API, editar o arquivo Api.csproj ajustar a string de conexão "DefaultConnection".
3. No terminal, acesse a pasta Api do projeto.
4. Para criar a estrutura do banco de dados, utilizando as migrations do Entity Framework, execute o seguinte comando:
```
dotnet ef database update
```
5. Caso o comando não seja encontrado execute o seguinte para instalar os comandos do Entity Eramework:
```
dotnet tool install --global dotnet-ef
```
6. Para executar o projeto e iniciar a API, execute:
```
dotnet run
```
7. O projeto está em execução.
8. Caso deseje visualizar a definição da API (OpenAPI), acesse o endpoint /api

### Executando os testes
1. Pelo terminal acessar a pasta Backend
2. Acessar a pasta Tests
3. Executar o comando:
```
dotnet test
```
Obs: Para visualização mais detalhadas dos testes recomenda-se utilizar o Visual Studio ou VSCode para executar

## Frontend

O projeto foi criado utilizando React.

### Requisitos
- Node 22

### Passos
1. No terminal, acessar a pasta Frontend
2. Caso desejar alterar a url da API, pode ser criado um arquivo .env com a seguinte variável:
```
VITE_API_URL = 'http://localhost:5240/api'
```
3. Por padrão a url padrão da API já está definida como fallback caso não encontre a váriavel definida no .env, portanto, não é preciso informar.
4. Para instalar as dependências, execute o seguinte comando:
```
npm install
```
5. Para iniciar, execute o seguinte comando:
```
npm run dev
```
### Como utilizar
- Ao acessar a página inicial serão listadas todas as tarefas cadastradas
- Selecionando um status no campo "Filtrar por Status:", será recarregado somente as tarefas no status desejado
- Na tela inicial, ao clicar em "Nova Tarefa" poderá ser criado uma tarefa preenchendo os campos. Na criação os campos de status e data de conclusão ficam bloqueados.
- Na tela inicial, ao clicar em "Editar", poderá ser editada a tarefa
- Na tela inicial, ao clicar em "Excluir", a tarefa será apagada
- Caso ocorrer erros, será exibido um alerta com a mensagem.

Obs: Optei por não colocar muitas validações no frontend para que possa ser validado pela API.