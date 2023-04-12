
# Tutorial de instalação e execução do projeto

Veja abaixo como fazer o setup do projeto e executá-lo utilizando a IDE de sua preferência.

Certifique-se de ter todos os [pré-requisitos](../../README.md#pré-requisitos-para-rodar-o-projeto) instalados antes de seguir com o tutorial.

---

## Executar utilizando Visual Studio

### 1. Clonar o repositório e abrir o projeto

Abra o Visual Studio e clique em `Clone repo` e cole a URL do repositório. 

### 2. Variáveis de ambiente

Renomeie o arquivo `local.settings.sample.json` para `local.settings.json`

### 3. Setup do banco de dados

Para criar o banco de dados, basta aplicar as migrations do Entity Framework Core.

Para isso, clique com o botão direito no projeto `MyCoreBanking.API`, clique para abrir no terminal e execute o comando abaixo:

```bash
dotnet ef database update
```

### 4. Execução do projeto

- Para executar o projeto, clique com o botão direito na solução `MyCoreBanking` e clique em `Configure Startup Projects...`.
- Em seguida, marque a opção `Multiple startup projects` e selecione `Start` para os projetos `MyCoreBanking.API` e `MyCoreBanking.Web`.
- Clique em `OK` para salvar as alterações.
- Agora, basta clicar no botão `Start` ou pressionar `F5` para executar o projeto.

---

## Executar utilizando VS Code

### 1. Clonar o repositório

```bash
git clone <url>
```

### 2. Abrir o projeto no vscode

```bash
code .
```

### 3. Variáveis de ambiente

Renomeie o arquivo `local.settings.sample.json` para `local.settings.json`

### 4. Setup do banco de dados

Para criar o banco de dados, basta aplicar as migrations do Entity Framework Core. Para isso, abra o terminal do vscode, certifique-se de estar na pasta `~\MyCoreBanking\src\MyCoreBanking.API` e execute o comando abaixo:

```bash
dotnet ef database update
```

### 5. Executar a API Azure Functions

Ainda no terminal do vscode, certifique-se de estar na pasta `~\MyCoreBanking\src\MyCoreBanking.API` e execute o comando abaixo:

```bash
func start --csharp
```

### 6. Executar o Blazor WebAssembly

Para o projeto web funcionar corretamente, é necessário que a API esteja rodando.

Então, em outro terminal do vscode, certifique-se de estar na pasta `~\MyCoreBanking\src\MyCoreBanking.Web` e execute o comando abaixo:

```bash
dotnet run
```

### 7. Acessar o sistema

Acesse o endereço `https://localhost:7197` no navegador.

### 8. Testar as requisições (opcional)

Utilize o endereço `https://localhost:7071` no Postman.

### 9. Acessar o banco de dados (opcional)

Utilize as credenciais abaixo para acessar o banco de dados:

- Server: (localdb)\mssqllocaldb
- Authentication: SQL Server Authentication
- Database: MyCoreBanking
