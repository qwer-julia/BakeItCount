using BakeItCountApi.Cn.Achievements;
using Microsoft.EntityFrameworkCore;

namespace BakeItCountApi.Data.Seeds
{
    public static class AchievementSeed
    {
        public static void Seed(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Achievement>().HasData(
                new Achievement { AchievementId = 1, Name = "Primeiro Delícia", Description = "Comprou seu primeiro bolo. Volte sempre :)", Criteria = "Comprar 1 bolo" },
                new Achievement { AchievementId = 2, Name = "Connoisseur do Chocolate", Description = "Três bolos de chocolate?", Criteria = "Comprar 3 bolos de chocolate" },
                new Achievement { AchievementId = 3, Name = "O Gourmet", Description = "Tá portando hein.", Criteria = "Comprar 5 bolos premium" },
                new Achievement { AchievementId = 4, Name = "Sem Calote!", Description = "Palmas! O mínimo virou conquista.", Criteria = "Completar 5 pedidos sem cancelamento" },
                new Achievement { AchievementId = 5, Name = "Caçador de Sabores", Description = "Eai, já decidiu qual o melhor?", Criteria = "Comprar pelo menos 1 de cada sabor disponível" },
                new Achievement { AchievementId = 6, Name = "Fidelidade Doce", Description = "Voltou pra comprar mais bolo 7 vezes. Vício tem outro nome.", Criteria = "Realizar compras em 7 dias diferentes" },
                new Achievement { AchievementId = 7, Name = "Bolo Master", Description = "50 bolos. Sério, parabéns.", Criteria = "Comprar 50 bolos cumulativamente" },
                new Achievement { AchievementId = 8, Name = "Evita Trocas", Description = "Impressionante.", Criteria = "Nenhuma troca em 10 pedidos" },
                new Achievement { AchievementId = 9, Name = "Rei do Bolo Surpresa", Description = "Comprou bolo surpresa. Corajoso ou inconsequente? Nunca saberemos.", Criteria = "Comprar bolos com promoções ou sabores surpresa" },
                new Achievement { AchievementId = 11, Name = "Caloteiro!", Description = "Não pagou o pedido. Espero que o boleto pelo menos estava gostoso.", Criteria = "Deixou de pagar 1 pedido" },
                new Achievement { AchievementId = 12, Name = "Troca-Troca", Description = "Não é o que parece...", Criteria = "Realizou 5 ou mais trocas de bolo" },
                new Achievement { AchievementId = 13, Name = "Trem Bão!", Description = "Ninguem vai te culpar, sabores mineiros são os melhores", Criteria = "Comprar 5 bolos dos sabores: fubá com goiabada, queijadinha, fubá com doce de leite ou fubá cremoso" },
                new Achievement { AchievementId = 14, Name = "Luiz Felipe", Description = "Tá né...", Criteria = "Comprar o bolo 'Luiz Felipe'" },
                new Achievement { AchievementId = 15, Name = "Mestre Confeiteiro", Description = "Como é amigo?", Criteria = "Comprar todos os bolos do cardápio pelo menos 1 vez" },
                new Achievement { AchievementId = 16, Name = "Cobertura Extra", Description = "Pediu bolo com cobertura… herói", Criteria = "Comprar bolo com cobertura adicional" },
                new Achievement { AchievementId = 17, Name = "Saudável & Doce", Description = "O Rafael tá sabendo disso?", Criteria = "Comprar bolo funcional/diet" },
                new Achievement { AchievementId = 18, Name = "Rebelde da Confeitaria", Description = "Comprou um sabor que nem está no cardápio… corajoso ou inconsequente? Talvez os dois.", Criteria = "Comprar sabor fora do cardápio" },
                new Achievement { AchievementId = 19, Name = "Seguidor de Multidões", Description = "Escolheu o sabor mais votado da semana… sem criatividade, mas pelo menos acertou.", Criteria = "Comprar o sabor mais votado da semana" }
            );
        }
    }
}
