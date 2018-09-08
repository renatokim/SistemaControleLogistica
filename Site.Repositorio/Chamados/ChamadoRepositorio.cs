using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using Site.DTO.Chamado;
using Site.Entidade.Chamados;
using Site.Enums;
using Site.IRepositorio;
using Site.IRepositorio.Chamados;

namespace Site.Repositorio.Chamados
{
    public class ChamadoRepositorio : IChamadoRepositorio
    {
        readonly IRepositorioGenerico _repositorioGenerico = new RepositorioGenerico();

        public DTOChamado GetById(int id)
        {
            IList<DTOChamado> listaChamados;
            string sql = string.Format(@"SELECT * FROM chm_chamado WHERE id = {0}", id);

            using (var contexto = new Contexto())
            {
                var result = contexto.ExecutaComandoComRetorno(sql);
                var chamados = TransformaListaChamados(result);
                listaChamados = TransformList(chamados);
            }

            return listaChamados.FirstOrDefault();
        }

        public IList<DTOChamado> GetAll()
        {
            IList<DTOChamado> listaChamados;
            string sql = string.Format(@"SELECT * FROM chm_chamado");

            using (var contexto = new Contexto())
            {
                var result = contexto.ExecutaComandoComRetorno(sql);
                var chamados = TransformaListaChamados(result);
                listaChamados = TransformList(chamados);
            }

            return listaChamados;
        }

        public void Salvar(DTOChamado chamado)
        {
            if (chamado.Id > 0)
            {
                Editar(chamado);
            }
            else
            {
                Incluir(chamado);
            }
        }

        private void Incluir(DTOChamado chamado)
        {
            var c = Transform(chamado);

            var sql = string.Format(@"
                INSERT INTO chm_chamado (
                    sistema, data_criacao, assunto, descricao, status, pendente_com, horas, pg, obs, prioridade, tipo)
                VALUES (
                     {0}, '{1}', '{2}', '{3}', {4}, {5}, {6}, {7}, '{8}', {9}, {10});",
                     c.Sistema,
                     c.Data.ToString("yyyy-MM-dd HH:mm:ss"),
                     c.Assunto,
                     c.Descricao,
                     c.Status,
                     c.PendenteCom,
                     c.Horas,
                     c.Pago,
                     c.Obs,
                     c.Prioridade,
                     c.TipoChamado);

            _repositorioGenerico.ExecutaComandoSemRetorno(sql);
        }

        private void Editar(DTOChamado chamado)
        {
            var e = Transform(chamado);

            var sql = string.Format(@"UPDATE chm_chamado
                            SET
                            sistema = {0},
                            data_criacao = '{1}',
                            assunto = '{2}',
                            descricao = '{3}',
                            status = {4},
                            pendente_com = {5},
                            horas = {6},
                            pg = {7},
                            obs = '{8}',
                            prioridade = {9},
                            tipo = {10}
                            WHERE id = {11};",
                            e.Sistema,
                            e.Data.ToString("yyyy-MM-dd HH:mm:ss"),
                            e.Assunto,
                            e.Descricao,
                            e.Status,
                            e.PendenteCom,
                            e.Horas,
                            e.Pago,
                            e.Obs,
                            e.Prioridade,
                            e.TipoChamado,
                            e.Id);

            _repositorioGenerico.ExecutaComandoSemRetorno(sql);
        }

        public IList<DTOChamado> GetByFilter(DTOFiltroChamado filtro)
        {
            throw new NotImplementedException();
        }

        public void Delete(int id)
        {
            var sql = string.Format(@"DELETE FROM chm_chamado WHERE id = {0}", id);
            _repositorioGenerico.ExecutaComandoSemRetorno(sql);
        }

        private static IList<Chamado> TransformaListaChamados(MySqlDataReader mySqlDataReader)
        {
            IList<Chamado> chamados = new List<Chamado>();

            while (mySqlDataReader.Read())
            {
                var newChamado = new Chamado
                {
                    Id = Convert.ToInt32(mySqlDataReader["id"]),
                    Descricao = mySqlDataReader["descricao"].ToString(),
                    Status = (int)mySqlDataReader["status"],
                    Assunto = mySqlDataReader["assunto"].ToString(),
                    Sistema = (int)mySqlDataReader["sistema"],
                    Data = Convert.ToDateTime(mySqlDataReader["data_criacao"]),
                    PendenteCom = (int)mySqlDataReader["pendente_com"],
                    Horas = (int)mySqlDataReader["horas"],
                    Pago = (int)mySqlDataReader["pg"],
                    Obs = mySqlDataReader["obs"].ToString(),
                    Prioridade = (int)mySqlDataReader["prioridade"],
                    TipoChamado = (int)mySqlDataReader["tipo"]
                };

                chamados.Add(newChamado);
            }
            mySqlDataReader.Dispose();
            return chamados;
        }

        public static IList<DTOChamado> TransformList(IList<Chamado> chamados)
        {
            return chamados.Select(Transform).ToList();
        }

        public static IList<Chamado> TransformList(IList<DTOChamado> chamados)
        {
            return chamados.Select(Transform).ToList();
        }

        public static Chamado Transform(DTOChamado c)
        {
            var chamado = new Chamado
            {
                Id = c.Id,
                Data = c.Data,
                Assunto = c.Assunto,
                Descricao = c.Descricao,
                Obs = c.Obs,
                Prioridade = (int)c.Prioridade,
                Pago = (int)c.Pago,
                Horas = c.Horas,
                PendenteCom = (int)c.PendenteCom,
                TipoChamado = (int)c.TipoChamado,
                Status = (int)c.Status
            };

            return chamado;
        }

        public static DTOChamado Transform(Chamado c)
        {
            var chamado = new DTOChamado
            {
                Id = c.Id,
                Data = c.Data,
                Assunto = c.Assunto,
                Descricao = c.Descricao,
                Obs = c.Obs,
                Prioridade = (PrioridadeChamado)c.Prioridade,
                Pago = (SimNao)c.Pago,
                Horas = c.Horas,
                PendenteCom = (PendenteComChamado)c.PendenteCom,
                TipoChamado = (TipoChamado)c.TipoChamado,
                Status = (StatusChamado)c.Status
            };

            return chamado;
        }
    }
}
