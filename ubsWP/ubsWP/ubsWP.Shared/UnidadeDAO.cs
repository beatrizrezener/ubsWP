using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace ubsWP
{
    class UnidadeDAO
    {
        //Traz todos os registros da tabela Unidade
        public List<Unidade> Listar()
        {
            return Conexao.Conn().Table<Unidade>().ToList();
        }
        public List<Unidade> Listar(Expression<Func<Unidade, bool>> filtro)
        {
            return Conexao.Conn().Table<Unidade>().Where(filtro).ToList();
        }

    }
}
