using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokemonConsoleApp.Model
{
    public class UserPokemon
    {
        public int UserID { get; set; }
        public int PokemonID { get; set; }
        public int Count { get; set; }

        public PokemonModel Pokemon { get; set; } // Mối quan hệ với Pokémon


    }

}
