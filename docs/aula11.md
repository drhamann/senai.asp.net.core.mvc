# Aula 11 

- Introdução
- Conteudo
- Sobre
- Tutorial

# Materiais 


## Exemplo código

```
Exemplo de uso simulando banco em memoria

namespace ProjetoEmTresCamadas.Pizzaria.DAO.Dao.Memory
{
    public class ClienteDaoInMemory : IClienteDao
    {
        public List<ClienteVo> Clientes { get; set; }
        public ClienteDaoInMemory()
        {
            Clientes = new ();

            ClienteVo clienteVo = new ClienteVo()
            {
                Id = 1,
                Nome = "Exemplo de cliente para memoria",
                UserId = Guid.NewGuid()
            };
            Clientes.Add(clienteVo);
        }
        public Task AtualizarRegistro(ClienteVo objetoParaAtualizar)
        {
            var idAtualizar = Clientes.Find(cliente => cliente.Id.Equals(objetoParaAtualizar.Id));
            Clientes.Remove(idAtualizar);
            Clientes.Add(objetoParaAtualizar );
            return Task.CompletedTask;
        }

        public int CriarRegistroAsync(ClienteVo objetoVo)
        {
            objetoVo.Id = Clientes.Count + 1;
            Clientes.Add(objetoVo);
            return objetoVo.Id;

        }

        public Task DeletarRegistro(int ID)
        {
            var idAtualizar = Clientes.Find(cliente => cliente.Id.Equals(ID));
            Clientes.Remove(idAtualizar);
            return Task.CompletedTask;
        }

        public Task<ClienteVo> ObterRegistro(int ID)
        {
            var cliente = Clientes.Find(cliente => cliente.Id.Equals(ID));
            return Task.FromResult(cliente);
        }

        public List<ClienteVo> ObterRegistros(int ID)
        {
            var clientes = Clientes.FindAll(cliente => cliente.Id.ToString().Contains(ID.ToString()));
            return clientes;
        }

        public List<ClienteVo> ObterRegistrosAsync()
        {
            return Clientes;
        }
    }
}

```
## Exercicio

- 01 Fazer a corrida de cachorro em API
- 02 Adicionar documentação no aplicativo da pizzaria 

 ## Próximos

- [próximo](aula2.md)