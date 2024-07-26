Para que a aplicação web funcione corretamente, a API deve estar em execução.

## Pré-requisitos

Antes de começar, verifique se você tem os seguintes requisitos instalados:

- [.NET Core SDK](https://dotnet.microsoft.com/download) 5.0 ou superior
- [SQL Server](https://www.microsoft.com/sql-server)

## Configuração do Projeto

### 1. Clonar o Repositório

Clone o repositório do projeto e navegue até o diretório:

git clone https://github.com/Anderson-MS/UsersApi.git
	
## Configuração do Projeto

##Executar as Migrations

Para configurar o banco de dados, você deve estar dentro da pasta do projeto da API. Execute os comandos abaixo:

dotnet ef migrations add InitialCreate
dotnet ef database update

