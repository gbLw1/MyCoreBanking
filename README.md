# MyCoreBanking

## ToDO:

- [ ] Criar algum endpoint com 402

## Sobre

MyCoreBanking é um projeto pessoal para estudo de desenvolvimento de software. O objetivo é criar um sistema de controle financeiro pessoal, com funcionalidades básicas de um banco, como cadastro de contas correntes, cartões de crédito, transações, etc.

## Tecnologias

- Framework: .NET 6.0
- API: Azure Functions 4.0
- Web: Blazor WebAssembly
- ORM: Entity Framework Core 6.0
- DB: SQL Server 2022

## Funcionalidades

- [ ] Usuário
  - [x] Cadastro
  - [x] Login
  - [ ] Logout
  - [ ] Alterar senha
  - [ ] Recuperar senha
  - [ ] Alterar dados

- [ ] Transações
  - [x] Cadastro
  - [x] Listagem
    - [ ] Listagem por período
  - [ ] Obter por Id
  - [ ] Alteração
  - [ ] Exclusão

- [ ] Contas correntes
  - [x] Cadastro
  - [x] Listagem
    - [ ] Listagem por período
    - [ ] Listagem por banco
  - [ ] Obter por Id
  - [ ] Alteração
  - [ ] Exclusão

- [ ] Cartões de crédito
  - [x] Cadastro
  - [x] Listagem
    - [ ] Listagem por período
    - [ ] Listagem por bandeira
    - [ ] Listagem por banco
  - [ ] Obter por Id
  - [ ] Alteração
  - [ ] Exclusão

<!-- ## Arquitetura

### API

A API é uma Azure Function, que utiliza o padrões REST 

A API é uma Azure Function, que utiliza o padrão de arquitetura de software DDD (Domain Driven Design). A API é responsável por receber as requisições do cliente, validar os dados e chamar os serviços de domínio. Os serviços de domínio são responsáveis por realizar as regras de negócio e persistir os dados no banco de dados.

### Banco de dados

O banco de dados é um SQL Server 2022, que utiliza o Entity Framework Core para mapear as entidades do domínio para tabelas do banco de dados.

### Cliente

O cliente é uma aplicação Blazor WebAssembly, que utiliza o padrão de arquitetura de software MVVM (Model-View-ViewModel). O cliente é responsável por exibir as informações para o usuário e enviar as requisições para a API. -->

## Diagrama de contexto

<!-- Mermaid context diagram -->

```mermaid
graph LR
    A[Usuário] --> B[Blazor WebAssembly]
    B --> C[API]
    C --> D[Banco de Dados]
    D --> C
    C --> B
    B --> A
```

## Diagrama de sequência

<!-- Mermaid sequence diagram -->

```mermaid
sequenceDiagram
    participant U as Usuário
    participant S as Blazor WebAssembly
    participant R as API
    participant D as Banco de Dados

    U->>S: Cadastrar usuário
    S->>R: Salvar usuário
    R->>D: Salvar usuário
    D->>R: Usuário salvo
    R->>S: Usuário salvo
    S->>U: Usuário salvo
```

## Diagrama de classe

<!-- Mermaid object relationships -->

```mermaid
classDiagram

class TransacaoTipo{
    <<enumeration>>
    Receita
    Despesa
}

class Banco{
    <<enumeration>>
    BancoDoBrasil
    Bradesco
    Inter
    Itau
    Nubank
    Santander
    C6
    Caixa
}

class BandeiraCartao{
    <<enumeration>>
    Visa
    Mastercard
    Elo
    AmericanExpress
    Hipercard
}

class MeioDePagamentoTipo{
    <<enumeration>>
    CartaoDeCreditoEntityDeCredito
    ContaCorrente
}

class BaseEntity{
    Id: Guid
}

class BaseDataEntity{
    Id: Guid
    CriadoEm: DateTime
    UltimaAtualizacaoEm: DateTime
}

class UsuarioEntity{
    Nome: String
    Email: String
    SenhaHash: String
    Transacoes: List~TransacaoEntity~
    MeiosDePagamento: List~MeioDePagamentoEntity~
    ContasCorrente: List~ContaCorrenteEntity~
    CartoesDeCredito: List~CartaoDeCreditoEntity~
    HashSenha(String senha) void
    SenhaValida(String senha) bool
}

class MeioDePagamentoEntity{
    Apelido: String
    Observacao: String
    Tipo: MeioDePagamentoTipo
    Usuario : UsuarioEntity
    Transacoes : List~TransacaoEntity~
    CartaoDeCredito : CartaoDeCreditoEntity
    ContaCorrente : ContaCorrenteEntity
}

class TransacaoEntity{
    Descricao: String
    Observacao: String?
    Valor: Decimal
    DataPagamento: DateTime
    Usuario: UsuarioEntity
    MeioDePagamento: MeioDePagamentoEntity
    Tipo: TransacaoTipo
}

class CartaoDeCreditoEntity{
    NumerosFinais: String
    Banco: Banco
    Bandeira: BandeiraCartao
    MeioDePagamento: MeioDePagamentoEntity
}

class ContaCorrenteEntity{
    Banco: Banco
    Agencia: String
    Conta: String
    MeioDePagamento: MeioDePagamentoEntity
}





%% Relacionamentos:
UsuarioEntity --|> BaseDataEntity
TransacaoEntity --|> BaseDataEntity
MeioDePagamentoEntity --|> BaseDataEntity
CartaoDeCreditoEntity --|> BaseEntity
ContaCorrenteEntity --|> BaseEntity
BaseDataEntity --|> BaseEntity
```

## Diagrama entidade-relacionamento

```mermaid
erDiagram

ContaCorrente{
    uniqueidentifier Id PK, FK
    nvarchar(max) Banco "enum Banco"
    nvarchar(max) Agencia
    nvarchar(max) Conta
}

MeioDePagamento{
    uniqueidentifier Id PK
    datetime2 CriadoEm
    datetime2 UltimaAtualizacaoEm
    nvarchar(max) Apelido
    nvarchar(max) Observacao
    nvarchar(max) Tipo "enum MeioDePagamentoTipo"
    uniqueidentifier UsuarioId FK
}

CartaoDeCredito{
    uniqueidentifier Id PK, FK
    nvarchar(max) NumerosFinais
    nvarchar(max) Bandeira "enum BandeiraCartao"
    nvarcahr(max) Banco "enum Banco"
}

Usuario{
    uniqueidentifier Id PK
    nvarchar(max) Nome
    nvarchar(max) Email UK
    nvarchar(max) SenhaHash
    datetime2 CriadoEm
    datetime2 UltimaAtualizacaoEm
}

Transacao{
    uniqueidentifier Id PK
    datetime2 CriadoEm
    datetime2 UltimaAtualizacaoEm
    nvarchar(max) Descricao
    nvarchar(max) Observacao
    decimal Valor
    datetime2 DataPagamento
    nvarchar(max) Tipo "enum TransacaoTipo"
    uniqueidentifier UsuarioId FK
    uniqueidentifier MeioDePagamentoId FK    
}

Usuario ||--|{ MeioDePagamento : possui
Usuario ||--|{ Transacao : possui
ContaCorrente |o--|| MeioDePagamento : eh
CartaoDeCredito |o--|| MeioDePagamento : eh
Transacao }|--|| MeioDePagamento : usa
```
