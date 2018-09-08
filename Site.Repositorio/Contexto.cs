using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using Dapper;

namespace Site.Repositorio
{
    public class Contexto : IDisposable
    {
        public readonly MySqlConnection _minhaConexao;

        public Contexto()
        {
            _minhaConexao = new MySqlConnection(ConfigurationManager.ConnectionStrings["siteEntities"].ConnectionString);
            _minhaConexao.Open();
        }

        ~Contexto()
        {
            Dispose();
        }

        public void ExecutaComando(string sql)
        {
            var cmdCommand = new MySqlCommand
            {
                CommandText = sql,
                CommandType = CommandType.Text,
                Connection = _minhaConexao
            };

            cmdCommand.ExecuteNonQuery();
        }

        public MySqlDataReader ExecutaComandoComRetorno(string sql)
        {
            var cmdCommand = new MySqlCommand(sql, _minhaConexao);
            return cmdCommand.ExecuteReader();
        }

        public void Dispose()
        {
            if (_minhaConexao.State == ConnectionState.Open)
            {
                _minhaConexao.Close();
            }
        }
    }
}