using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Site.Enums
{
    public enum SimNao
    {
        Todos,
        Nao,
        Sim
    };

    public enum TipoMensagem
    {
        Alerta,
        Erro,
        Sucesso
    };

    public enum Status
    {
        Inativo = 0,
        Ativo = 1,
        Todos
    };

    public enum StatusChamado
    {
        Todos,
        Aberto,
        Recebido,
        Desenvolvimento,
        Teste,
        Concluido,
        Cancelado
    };

    public enum PendenteComChamado
    {
        Todos,
        Cliente,
        Empresa
    };

    public enum PrioridadeChamado
    {
        Todos,
        Baixa,
        Media,
        Alta
    };

    public enum TipoChamado
    {
        Todos,
        Erro,
        Melhoria,
        Atendimento
    };

    public enum StatusPedido
    {
        Aberto = 1,
        Concluido = 2,
        Cancelado = 8
    };

    public enum StatusCliente
    {
        Inativo = 0,
        Ativo = 1,
        Todos
    };

    public enum EnvioRetorno
    {
        Envio,
        Retorno,
        SemMovimento,
        Outros
    }

    public enum StatusFluxo
    {
        Pendente,
        Concluido,
        Todos
    }

    public enum OrdemRelatorio
    {
        DataCadastro,
        Cliente,
        DataPrevisaoEntrega
    }

    public enum Acao
    {
        Tudo,
        SoEnvio,
        SoRetorno
    }

    public enum StatusColeta
    {
        Pendente = 0,
        Coletado,
        EmEntrega,
        Entregue,
        Transferido,
        Devolvido,
        Cancelado
    }
}
