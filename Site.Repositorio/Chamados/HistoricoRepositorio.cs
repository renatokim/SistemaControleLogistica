using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using Site.DTO.Chamado;
using Site.Entidade.Chamados;
using Site.IRepositorio;
using Site.IRepositorio.Chamados;

namespace Site.Repositorio.Chamados
{
    public class HistoricoRepositorio : IHistoricoRepositorio
    {
        readonly IRepositorioGenerico _repositorioGenerico = new RepositorioGenerico();

        public DTOHistorico GetById(int id)
        {
            throw new NotImplementedException();
        }

        public IList<DTOHistorico> GetAll()
        {
            IList<DTOHistorico> listaCategorias;
            string sql = string.Format(@"SELECT * FROM chm_historico");

            using (var contexto = new Contexto())
            {
                var result = contexto.ExecutaComandoComRetorno(sql);
                var categorias = TransformaListaHistorico(result);
                listaCategorias = TransformList(categorias);
            }

            return listaCategorias;
        }

        public void Salvar(DTOHistorico historico)
        {
            var h = Transform(historico);

            var sql = string.Format(@"
                INSERT INTO chm_historico
                    (
                        descricao,
                        data,
                        id_chamado)
                        VALUES
                    (
                         '{0}',
                         '{1}',
                          {2});",
                     h.Descricao,
                     h.Data.ToString("yyyy-MM-dd HH:mm:ss"),
                     h.Chamado.Id);

            _repositorioGenerico.ExecutaComandoSemRetorno(sql);
        }

        public IList<DTOHistorico> GetByFilter(DTOFiltroHistorico filtro)
        {
            IList<DTOHistorico> listaHistorico;
            string sql = string.Format(@"SELECT * FROM chm_historico where id_chamado = {0}", filtro.IdChamado);

            using (var contexto = new Contexto())
            {
                var result = contexto.ExecutaComandoComRetorno(sql);
                var historico = TransformaListaHistorico(result);
                listaHistorico = TransformList(historico);
            }

            return listaHistorico;
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        private IList<Historico> TransformaListaHistorico(MySqlDataReader mySqlDataReader)
        {
            IList<Historico> historicos = new List<Historico>();
            while (mySqlDataReader.Read())
            { 
                var newHistorico = new Historico
                {
                    Id = Convert.ToInt32(mySqlDataReader["id"]),
                    Descricao = mySqlDataReader["descricao"].ToString(),
                    Data = Convert.ToDateTime(mySqlDataReader["data"]),
                    Chamado = new Chamado { Id = (int)mySqlDataReader["id_chamado"] }
                };

                historicos.Add(newHistorico);
            }
            mySqlDataReader.Dispose();
            return historicos;
        }

        public static IList<DTOHistorico> TransformList(IList<Historico> c)
        {
            return c.Select(Transform).ToList();
        }

        public static IList<Historico> TransformList(IList<DTOHistorico> c)
        {
            return c.Select(Transform).ToList();
        }

        public static DTOHistorico Transform(Historico h)
        {
            return new DTOHistorico
            {
                Id = h.Id,
                Descricao = h.Descricao,
                Data = h.Data,
                Chamado = new DTOChamado { Id = h.Chamado.Id }
            };
        }

        public static Historico Transform(DTOHistorico h)
        {
            return new Historico
            {
                Id = h.Id,
                Descricao = h.Descricao,
                Data = h.Data,
                Chamado = new Chamado { Id = h.Chamado.Id }
            };
        }
    }
}
