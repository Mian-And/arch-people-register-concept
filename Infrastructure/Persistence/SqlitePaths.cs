using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client_ConceitoCadastro
{
    public static class SqlitePaths
    {
        public static string GetDbPath()
        {
            var baseDir = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                "Client_ConceitoCadastro");
            Directory.CreateDirectory(baseDir);
            return Path.Combine(baseDir, "app.db");
        }
    }

}
