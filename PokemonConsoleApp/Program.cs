using PokemonConsoleApp.Config;
using PokemonConsoleApp.Controller;
using PokemonConsoleApp.Model;
using PokemonConsoleApp.Utils;
using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Program
{
    public class Program
    {
        public static void Main(string[] args)
        {
            using (var dbContext = new AppDbContext())
            {
                GameControllerWithView gameController = new GameControllerWithView();

                bool continueMenu = true;

                while (continueMenu)
                {
                    Console.WriteLine("-------------------------------------");
                    InputUtils.PrintColoredText("Authentication - Pokemon Đại Chiến 💘", "Red");
                    Console.WriteLine("-------------------------------------\n");
                    Console.WriteLine("1. Đăng nhập");
                    Console.WriteLine("2. Đăng ký");
                    Console.WriteLine("3. Xoá màn hình");
                    Console.WriteLine("4. Thoát chương trình");

                    int choice = InputUtils.GetValidIntegerInput("Nhập lựa chọn: ");

                    switch (choice)
                    {
                        case 1:
                            // Xử lý đăng nhập
                            LoginUser(dbContext, gameController);
                            break;
                        case 2:
                            // Đăng ký
                            RegisterUser(dbContext);
                            break;
                        case 3:
                            // Xoá màn hình
                            Console.Clear();
                            break;
                        case 4:
                            Console.WriteLine("Bạn đã thoát khỏi chương trình.");
                            continueMenu = false;
                            break;
                        default:
                            Console.WriteLine("Lựa chọn không hợp lệ.");
                            break;
                    }
                }
            }
        }


        private static void LoginUser(AppDbContext dbContext, GameControllerWithView gameController)
        {
            Console.Write("Nhập tài khoản: ");
            var usernameLogin = Console.ReadLine();
            Console.Write("Nhập mật khẩu: ");
            var passwordLogin = Console.ReadLine();

            var user = dbContext.users.FirstOrDefault(u => u.Username == usernameLogin);

            if (user != null && PasswordUtils.VerifyPassword(passwordLogin, user.Password))
            {
                InputUtils.PrintColoredText("Đăng nhập thành công.", "Blue");
                // Sau khi đăng nhập thành công, gọi phương thức ShowMainMenu
                Console.Clear();

                gameController.ShowMainMenu(user.Id);

            }
            else
            {
                InputUtils.PrintColoredText("Sai tên đăng nhập hoặc mật khẩu.", "Red");
            }
        }


        private static void RegisterUser(AppDbContext dbContext)
        {
            string username;
            string password;
            string repassword;

            while (true)
            {
                Console.Write("Nhập tài khoản: ");
                username = Console.ReadLine();

                if (dbContext.users.Any(u => u.Username == username))
                {
                    Console.Clear();
                    InputUtils.PrintColoredText("Tài khoản đã tồn tại trong cơ sở dữ liệu. Vui lòng chọn tài khoản khác.", "Red");
                }
                else
                {
                    break;
                }
            }

            while (true)
            {
                Console.Write("Nhập mật khẩu: ");
                password = Console.ReadLine();
                Console.Write("Nhập lại mật khẩu: ");
                repassword = Console.ReadLine();

                if (password.Equals(repassword))
                {
                    break;
                }
                else
                {
                    InputUtils.PrintColoredText("Mật khẩu không khớp. Vui lòng thử lại.", "Red");
                }
            }

            var hashedPassword = PasswordUtils.HashPassword(password);

            var newUser = new UserModel
            {
                Username = username,
                Password = hashedPassword,
                CountHuntPokemon = 30,
            };

            dbContext.users.Add(newUser);
            dbContext.SaveChanges();
            Console.Clear();
            InputUtils.PrintColoredText("Đăng ký tài khoản thành công.", "Blue");
        }
    }
}



/*
1. Nói chuyện
2. Xem trong túi có những pokemon gì
3. Xem thể trạng chi tiết pokemon
4. Sắp xếp pokemon theo thứ tự (Level)
5. Đi thu thập pokemon (30 lượt)
6. Đi solo pokemon giữa các người chơi với nhau
7. Authentication
8. Đi tập luyện pokemon
9. Thoát
*/