using System.Collections.Generic;
using System.Threading.Tasks;

namespace Model.WordsServices
{
    public interface IFileReader
    {
        Task<Dictionary<string, int>> ReadFile(string fileName);
    }
}