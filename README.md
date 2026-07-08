# e-Agenda

**Atividade 17 - [Academia do Programador](https://www.academiadoprogramador.net/inicio) 2026**

## Funcionalidades

- Cadastro, edição, exclusão e visualização de **contatos**
  - Nome, email, telefone, cargo e empresa
  - Bloqueio de exclusão quando houver compromissos vinculados
- Cadastro, edição, exclusão e visualização de **compromissos**
  - Compromissos remotos ou presenciais
  - Vínculo opcional com contato
  - Validação de conflito de horários
- Cadastro, edição, exclusão e visualização de **categorias**
  - Listagem das despesas vinculadas a uma categoria específica
  - Bloqueio de exclusão quando houver despesas relacionadas
- Cadastro, edição, exclusão e visualização de **despesas**
  - Vínculo com uma ou mais categorias
  - Registro de valor, data de ocorrência e forma de pagamento
- Cadastro, edição, exclusão e visualização de **tarefas**
  - Listagem de tarefas pendentes e concluídas
  - Agrupamento por prioridade
  - Controle de percentual de conclusão
- Controle de **itens de tarefas**
  - Adição e remoção de itens em uma tarefa
  - Conclusão de itens com atualização do percentual da tarefa

## Regras de Negócio

- Contatos devem possuir nome, email e telefone válidos
- Não é permitido cadastrar dois contatos com o mesmo email e/ou telefone
- Não é permitido excluir um contato com compromissos vinculados
- Compromissos devem possuir assunto, data de ocorrência, hora de início, hora de término e tipo
- Compromissos presenciais devem possuir local
- Compromissos remotos devem possuir link
- Não pode haver conflito de horários entre compromissos
- Categorias devem possuir título
- Não é permitido cadastrar duas categorias com o mesmo título
- Não é permitido excluir categorias relacionadas a despesas
- Despesas devem possuir descrição, valor, forma de pagamento e pelo menos uma categoria
- Tarefas devem possuir título, prioridade, data de criação, status e percentual de conclusão
- Itens de tarefas devem possuir título, status de conclusão e tarefa vinculada

## Persistência de Dados

Os dados são armazenados em banco de dados SQL Server.

A aplicação utiliza a connection string `eAgenda`, configurada em:

`eAgenda.WebApp/appsettings.Development.json`

Por padrão, o ambiente de desenvolvimento aponta para:

`Server=(localdb)\MSSQLLocalDB;Database=eAgenda;TrustServerCertificate=True;Trusted_Connection=True;MultipleActiveResultSets=True`

O projeto de banco de dados está em:

`eAgenda.Database`

## Como Executar

### Pré-requisitos

- [.NET 10 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/10.0)
- SQL Server LocalDB ou SQL Server

### Passos

1. Abra a pasta do repositório.
2. Configure/crie o banco de dados `eAgenda` usando o projeto `eAgenda.Database`.
3. Restaure e compile a solução:

```bash
dotnet build eAgenda.slnx
```

4. Execute a aplicação:

```bash
dotnet run --project eAgenda.WebApp
```
