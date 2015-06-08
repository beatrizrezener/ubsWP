using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace ubsWP
{
    class Conexao
    {
        static string nomeBase = Path.Combine(ApplicationData.Current.LocalFolder.Path,
                                              "saudf.sqlite");

        private static async Task<bool> VerificarBanco(string nomeBanco)
        {
            bool retorno = true;

            try
            {
                await ApplicationData.Current.LocalFolder.GetFileAsync(nomeBanco);
            }
            catch (Exception)
            {
                retorno = false;
            }

            return retorno;
        }

        private static void CriarBaseDados()
        {
            //É importante usar o using pq ele já faz alguns tratamentos
            using (var db = new SQLiteConnection(nomeBase))
            {
                db.CreateTable<Unidade>();
            }
        }
        private static void CriarBase()
        {
            //bool bancoExiste = VerificarBanco(nomeBase).Result; // .Result para nao correr o risco do async nao ter terminado

            // if(!bancoExiste) //A base tá criada? Se sim, sai, se não, cria.
            //  {
            CriarBaseDados();
            // }
        }

        public static SQLiteConnection Conn()
        {
            CriarBase(); // Criar base

            SQLiteConnection sConn = new SQLiteConnection(nomeBase); //Tenta fazer a conexão

            return sConn;
        }
    }
}
