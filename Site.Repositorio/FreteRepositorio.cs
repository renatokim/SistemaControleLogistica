﻿using Site.IRepositorio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using System.Data.SqlClient;
using Site.DTO;
using System.Configuration;
using MySql.Data.MySqlClient;
using Site.Entidade.EntidadeModel;
using Site.ActiveRecords;

namespace Site.Repositorio
{
    public class FreteRepositorio : IFreteRepositorio
    {
        public IList<DTOFrete> GetAll()
        {
            using (var contexto = new Contexto())
            {
                var result = contexto._minhaConexao.Query<DTOFrete>(@"
                    SELECT 
                        id AS id,
                        uf AS uf,
                        frota_id AS frota,
                        valor AS valor
                    FROM frete").ToList();

                return result;
            }
        }

        public void Salvar(DTOFrete frete)
        {
            if (frete.Id > 0)
            {
                Editar(frete);
            }
            else
            {
                Incluir(frete);
            }
        }

        private void Incluir(DTOFrete frete)
        {
            var freteModel = FreteModel.Transform(frete);
            var sql = freteModel.QueryInsert();

            using (var contexto = new Contexto())
            {
                var result = contexto.ExecutaComandoComRetorno(sql);
                result.Read();
                frete.Id = Convert.ToInt32(result["LASTID"]);
            }
        }

        private void Editar(DTOFrete frete)
        {
            var freteModel = FreteModel.Transform(frete);
            var sql = ActiveRecord.Update(freteModel);

            using (var contexto = new Contexto())
            {
                contexto.ExecutaComando(sql);
            }
        }

        public void Excluir(int id)
        {
            var freteModel = new FreteModel { Id = id.ToString() };
            var sql = ActiveRecord.Delete(freteModel);

            using (var contexto = new Contexto())
            {
                contexto.ExecutaComando(sql);
            }
        }
    }
}
