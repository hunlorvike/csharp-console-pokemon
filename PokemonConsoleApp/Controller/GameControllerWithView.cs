using PokemonConsoleApp.Config;
using PokemonConsoleApp.Model;
using PokemonConsoleApp.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Data.Entity;

namespace PokemonConsoleApp.Controller
{
    internal class GameControllerWithView
    {
        public void ShowMainMenu(int userId)
        {
            using (var dbContext = new AppDbContext())
            {
                HashSet<PokemonModel> setPokemons = dbContext.pokemons
                    .Where(p => dbContext.userpokemons.Any(up => up.UserID == userId && up.PokemonID == p.Id))
                    .ToHashSet();


                // Sử dụng LINQ để lấy dữ liệu từ bảng Pokemons
                var pikachu = dbContext.pokemons
                    .Where(p => p.Name == "Pikachu")
                .FirstOrDefault();

                List<PokemonModel> pokemons = dbContext.pokemons.ToList();

                bool continueMenu = true;

                while (continueMenu)
                {
                    Console.WriteLine("-------------------------------------");
                    InputUtils.PrintColoredText("Trang chủ - Pokemon Đại Chiến 💘", "Red");
                    Console.WriteLine("-------------------------------------\n");
                    Console.WriteLine("1. Nói chuyện và nhận Pikachu");
                    Console.WriteLine("2. Đi săn pokemon");
                    Console.WriteLine("3. Kho");
                    Console.WriteLine("4. Đấu pokemon với người chơi khác");
                    Console.WriteLine("5. Trị thương cho pokemon");
                    Console.WriteLine("6. Toàn bộ pokemon trong chương trình");
                    Console.WriteLine("7. Xoá màn hình");
                    Console.WriteLine("8. Thoát");
                    int choice = InputUtils.GetValidIntegerInput("Nhập lựa chọn: ");

                    switch (choice)
                    {
                        case 1:
                            // Xử lý chơi game
                            TalkAndGetPikachu(dbContext, userId, setPokemons, pikachu);
                            break;
                        case 2:
                            // Xử lý quản lý tài khoản
                            HuntPokemon(dbContext, userId, setPokemons, pokemons);
                            break;
                        case 3:
                            // Xử lý hiển thị danh sách pokemon có trong kho
                            YourRepository(dbContext, userId);
                            break;
                        case 4:
                            SoloPokemon(dbContext, userId);
                            break;
                        case 5:
                            break;
                        case 6:
                            //Xử lý hiển thị toàn bộ pokemons
                            GetAllPokemons(dbContext, pokemons);
                            break;
                        case 7:
                            Console.Clear();
                            break;
                        case 8:
                            Console.WriteLine("Bạn đã thoát khỏi chương trình.");
                            continueMenu = false;
                            break;
                        default:
                            InputUtils.PrintColoredText("Lựa chọn không hợp lệ.", "Red");
                            break;
                    }
                }
            }

        }

        private void SoloPokemon(AppDbContext dbContext, int userId)
        {
            var userInfos = dbContext.users
                .Select(user => new
                {
                    Id = user.Id,
                    UserName = user.Username,
                    PokemonCount = dbContext.userpokemons.Count(up => up.UserID == user.Id)
                })
                .ToList();
            Console.Clear();
            Console.WriteLine("Danh sách người chơi\n");
            string header = string.Format("{0,-5} | {1,-20} | {2,-15}", "ID", "Tên tài khoản", "Số lượng Pokémon");
            Console.WriteLine(header);
            Console.WriteLine(new string('-', header.Length));

            foreach (var userInfo in userInfos)
            {
                string row = string.Format("{0,-5} | {1,-20} | {2,-15}", userInfo.Id, userInfo.UserName, userInfo.PokemonCount);
                Console.WriteLine(row);
            }
            Console.WriteLine(new string('-', header.Length));


            int idSolo = InputUtils.GetValidIntegerInput("\nNhập ID người chơi muốn varticang: ");
            PKPokemon(dbContext, idSolo, userId);

        }

        private void PKPokemon(AppDbContext dbContext, int idSolo, int userId)
        {
            var soloStrongestPokemon = GetStrongestPokemonForUser(dbContext, idSolo);
            var userStrongestPokemon = GetStrongestPokemonForUser(dbContext, userId);
            double damageModifier = 0.3;  // 30% sát thương

            if (soloStrongestPokemon != null && userStrongestPokemon != null)
            {
                Console.WriteLine($"\nPokemon mạnh nhất của tôi (\u001b[34m{GetUserByID(dbContext, userId).Username}\u001b[0m): {userStrongestPokemon.Pokemon.Name}");
                Console.WriteLine($"HP: {userStrongestPokemon.Pokemon.HP}");
                Console.WriteLine($"Dame: {userStrongestPokemon.Pokemon.Damage}");
                Console.WriteLine($"Sát thương thực: {userStrongestPokemon.Pokemon.Damage * damageModifier}");

                Console.WriteLine($"Pokemon mạnh nhất của đối thủ (\u001b[31m{GetUserByID(dbContext, idSolo).Username}\u001b[0m): {soloStrongestPokemon.Pokemon.Name}");
                Console.WriteLine($"HP: {soloStrongestPokemon.Pokemon.HP}");
                Console.WriteLine($"Dame: {soloStrongestPokemon.Pokemon.Damage}");
                Console.WriteLine($"Sát thương thực: {soloStrongestPokemon.Pokemon.Damage * damageModifier}");

                InputUtils.PrintColoredText("Ấn phím bất kỳ để bắt đầu", "DarkMagenta");
                Console.ReadKey();

                double myPokemonStrongestHp = ConvertIntToDouble(soloStrongestPokemon.Pokemon.HP);
                double yourPokemonStrongestHp = ConvertIntToDouble(userStrongestPokemon.Pokemon.HP);

                var tasks = new List<Task>();

                // Chạy mỗi Pokémon trong một luồng riêng biệt
                tasks.Add(Task.Run(() => ReduceHP(ref myPokemonStrongestHp, soloStrongestPokemon.Pokemon.Damage, damageModifier)));
                tasks.Add(Task.Run(() => ReduceHP(ref yourPokemonStrongestHp, userStrongestPokemon.Pokemon.Damage, damageModifier)));

                Task.WhenAll(tasks).Wait();

                if (myPokemonStrongestHp <= 0)
                {
                    Console.WriteLine("Thua cuộc");
                }
                else
                {
                    Console.WriteLine("Chiến thắng");
                }
            }
            else
            {
                Console.WriteLine("Một trong hai người chơi không có Pokémon.");
            }
        }

        private void ReduceHP(ref double hp, double damage, double damageModifier)
        {
            while (hp > 0)
            {
                hp -= (damage * damageModifier);
            }
        }


        public static double ConvertIntToDouble(int value)
        {
            return (double)value;
        }


        private UserModel GetUserByID(AppDbContext dbContext, int id)
        {
            return dbContext.users.FirstOrDefault(u => u.Id == id);
        }

        private UserPokemon GetStrongestPokemonForUser(AppDbContext dbContext, int userId)
        {
            return dbContext.userpokemons
                .Where(up => up.UserID == userId)
                .OrderByDescending(up => up.Pokemon.HP)
                .ThenByDescending(up => up.Pokemon.Damage)
                .FirstOrDefault();
        }


        private void YourRepository(AppDbContext dbContext, int userId)
        {
            var userPokemonsInfo = dbContext.userpokemons
                .Where(up => up.UserID == userId)
                .Join(
                    dbContext.pokemons,
                    up => up.PokemonID,
                    p => p.Id,
                    (up, p) => new { PokemonName = p.Name, Type = p.Type, Level = p.Level, Count = up.Count }
                )
                .ToList();
            Console.Clear();
            Console.WriteLine("Danh sách Pokemon của người dùng\n");
            Console.WriteLine(string.Format("{0, -20} | {1, -15} | {2, -10} | {3, -10}", "Tên", "Loại", "Cấp Độ", "Số lượng"));
            Console.WriteLine(new string('-', 60));

            foreach (var userPokemon in userPokemonsInfo)
            {
                string formattedRow = string.Format("{0, -20} | {1, -15} | {2, -10} | {3, -10}",
                    userPokemon.PokemonName, userPokemon.Type, userPokemon.Level, userPokemon.Count);
                Console.WriteLine(formattedRow);
            }
        }

        private void GetAllPokemons(AppDbContext dbContext, List<PokemonModel> pokemons)
        {
            Console.Clear();
            InputUtils.PrintColoredText("Danh sách pokemon có trong tự nhiên\n", "DarkYellow");
            Console.WriteLine(string.Format("{0, -3} | {1, -12} | {2, -14} | {3}", "ID", "Tên", "Loại", "Cấp Độ"));
            Console.WriteLine(new string('-', 40));

            foreach (var pokemon in pokemons)
            {
                string formattedRow = string.Format("{0, -3} | {1, -12} | {2, -14} | {3}", pokemon.Id, pokemon.Name, pokemon.Type, pokemon.Level);
                Console.WriteLine(formattedRow);
            }
        }

        private void HuntPokemon(AppDbContext dbContext, int userId, HashSet<PokemonModel> setPokemons, List<PokemonModel> pokemons)
        {
            var user = dbContext.users.FirstOrDefault(u => u.Id == userId);

            if (user != null)
            {
                if (user.CountHuntPokemon > 0)
                {
                    var random = new Random();
                    var ratioHunt = 0.4; // Tỉ lệ 40% bắt trúng Pokemon
                    if (random.NextDouble() < ratioHunt)
                    {
                        // Bắt trúng Pokemon
                        int randomPokemonIndex = random.Next(0, pokemons.Count);

                        PokemonModel caughtPokemon = pokemons[randomPokemonIndex];
                        string pokemonName;

                        if (setPokemons.Contains(caughtPokemon))
                        {
                            // Nếu Pokemon đã tồn tại, tìm Pokémon tương tự và tăng Count lên 1
                            var userPokemon = dbContext.userpokemons.FirstOrDefault(up => up.UserID == userId && up.PokemonID == caughtPokemon.Id);
                            userPokemon.Count++;
                            InputUtils.PrintColoredText($"Bạn đã bắt thêm một {caughtPokemon.Name}!", "Blue");
                        }
                        else
                        {
                            setPokemons.Add(caughtPokemon);
                            // Nếu Pokemon chưa tồn tại, thêm vào setPokemons với Count là 1
                            var newPokemon = new UserPokemon
                            {
                                UserID = userId,
                                PokemonID = caughtPokemon.Id,
                                Count = 1,
                            };

                            dbContext.userpokemons.Add(newPokemon);

                            // Lưu thay đổi vào cơ sở dữ liệu
                            dbContext.SaveChanges();
                            InputUtils.PrintColoredText($"Bạn đã bắt thêm một {caughtPokemon.Name}!", "Blue");

                        }

                    }
                    else
                    {
                        InputUtils.PrintColoredText("Bạn đã bắt hụt!", "Red");
                    }
                    // Giảm giá trị của CountHuntPokemon đi 1
                    user.CountHuntPokemon--;

                    // Lưu thay đổi vào cơ sở dữ liệu
                    dbContext.SaveChanges();

                }
                else
                {
                    InputUtils.PrintColoredText("Bạn đã hết lượt bắt Pokémon.", "Red");
                }
            }
            else
            {
                InputUtils.PrintColoredText("Không tìm thấy người dùng.", "Red");
            }
        }

        private void TalkAndGetPikachu(AppDbContext dbContext, int userId, HashSet<PokemonModel> setPokemons, PokemonModel pikachu)
        {
            // Tìm người dùng dựa trên userId
            var user = dbContext.users.FirstOrDefault(u => u.Id == userId);

            // Kiểm tra xem pikachu có trong setPokemons hay không
            if (!setPokemons.Contains(pikachu))
            {
                // Nếu không có, thêm pikachu vào setPokemons
                setPokemons.Add(pikachu);
                Console.Clear();
                InputUtils.PrintColoredText("Đã nhận pikachu, ấn phím bất kỳ để tiếp tục", "Blue");


                // Nếu Pokemon chưa tồn tại trong bảng UserPokemons, thêm một bản ghi mới
                var newUserPokemon = new UserPokemon
                {
                    UserID = userId,
                    PokemonID = pikachu.Id,
                    Count = 1
                };

                dbContext.userpokemons.Add(newUserPokemon);

                // Lưu thay đổi vào cơ sở dữ liệu
                dbContext.SaveChanges();
            }
            else
            {
                Console.Clear();
                InputUtils.PrintColoredText("Bạn đã nhận Pikachu trước đây", "Red");
            }
        }

    }
}
