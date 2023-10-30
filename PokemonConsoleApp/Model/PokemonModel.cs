using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace PokemonConsoleApp.Model
{
    public class PokemonModel
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public float PercentLevel { get; set; }
        public int Level { get; set; }
        public int HP { get; set; }
        public int Damage { get; set; }

    }

}
