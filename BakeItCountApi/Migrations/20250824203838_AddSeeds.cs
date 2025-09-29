using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BakeItCountApi.Migrations
{
    /// <inheritdoc />
    public partial class AddSeeds : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Achievements",
                columns: new[] { "AchievementId", "Criteria", "Description", "Name" },
                values: new object[,]
                {
                    { 1, "Comprar 1 bolo", "Comprou seu primeiro bolo. Volte sempre :)", "Primeiro Delícia" },
                    { 2, "Comprar 3 bolos de chocolate", "Três bolos de chocolate?", "Connoisseur do Chocolate" },
                    { 3, "Comprar 5 bolos premium", "Tá portando hein.", "O Gourmet" },
                    { 4, "Completar 5 pedidos sem cancelamento", "Palmas! O mínimo virou conquista.", "Sem Calote!" },
                    { 5, "Comprar pelo menos 1 de cada sabor disponível", "Eai, já decidiu qual o melhor?", "Caçador de Sabores" },
                    { 6, "Realizar compras em 7 dias diferentes", "Voltou pra comprar mais bolo 7 vezes. Vício tem outro nome.", "Fidelidade Doce" },
                    { 7, "Comprar 50 bolos cumulativamente", "50 bolos. Sério, parabéns.", "Bolo Master" },
                    { 8, "Nenhuma troca em 10 pedidos", "Impressionante.", "Evita Trocas" },
                    { 9, "Comprar bolos com promoções ou sabores surpresa", "Comprou bolo surpresa. Corajoso ou inconsequente? Nunca saberemos.", "Rei do Bolo Surpresa" },
                    { 11, "Deixou de pagar 1 pedido", "Não pagou o pedido. Espero que o boleto pelo menos estava gostoso.", "Caloteiro!" },
                    { 12, "Realizou 5 ou mais trocas de bolo", "Não é o que parece...", "Troca-Troca" },
                    { 13, "Comprar 5 bolos dos sabores: fubá com goiabada, queijadinha, fubá com doce de leite ou fubá cremoso", "Ninguem vai te culpar, sabores mineiros são os melhores", "Trem Bão!" },
                    { 14, "Comprar o bolo 'Luiz Felipe'", "Tá né...", "Luiz Felipe" },
                    { 15, "Comprar todos os bolos do cardápio pelo menos 1 vez", "Como é amigo?", "Mestre Confeiteiro" },
                    { 16, "Comprar bolo com cobertura adicional", "Pediu bolo com cobertura… herói", "Cobertura Extra" },
                    { 17, "Comprar bolo funcional/diet", "O Rafael tá sabendo disso?", "Saudável & Doce" },
                    { 18, "Comprar sabor fora do cardápio", "Comprou um sabor que nem está no cardápio… corajoso ou inconsequente? Talvez os dois.", "Rebelde da Confeitaria" },
                    { 19, "Comprar o sabor mais votado da semana", "Escolheu o sabor mais votado da semana… sem criatividade, mas pelo menos acertou.", "Seguidor de Multidões" }
                });

            migrationBuilder.InsertData(
                table: "Flavors",
                columns: new[] { "FlavorId", "Category", "Description", "Name" },
                values: new object[,]
                {
                    { 1, "Simples", "Massa de fubá com toque de limão siciliano.", "Fubá com Limão Siciliano" },
                    { 2, "Simples", "Combinação de maçã e castanhas.", "Maçã com Castanha" },
                    { 3, "Simples", "Massa de fubá com pedaços de goiabada.", "Fubá com Goiabada" },
                    { 4, "Simples", "Clássico bolo de fubá.", "Fubá" },
                    { 5, "Simples", "Bolo de milho com sabor tradicional.", "Milho" },
                    { 6, "Simples", "Bolo leve com sabor de iogurte.", "Iogurte" },
                    { 7, "Simples", "Bolo de fubá com textura cremosa.", "Fubá Cremoso" },
                    { 8, "Simples", "Bolo de cenoura fofinho.", "Cenoura" },
                    { 9, "Simples", "Massa de chocolate com granulado.", "Formigueiro" },
                    { 10, "Simples", "Combinação de banana e canela.", "Banana com Canela" },
                    { 11, "Caro", "Bolo de milho com recheio de requeijão.", "Milho com Requeijão" },
                    { 12, "Caro", "Bolo de cenoura com creme de avelã.", "Bolo de Cenoura com Creme de Avelã" },
                    { 13, "Caro", "Camadas de bolo de chocolate com leite condensado.", "Bolo Piscina de Chocolate com Leitinho" },
                    { 14, "Caro", "Camadas de bolo de milho com curau.", "Bolo Piscina de Milho com Curau" },
                    { 15, "Caro", "Camadas de bolo de goiabada e queijo.", "Bolo Piscina Romeu e Julieta" },
                    { 16, "Simples", "Bolo de cenoura com gotas de chocolate.", "Cenoura com Gotas de Chocolate" },
                    { 17, "Simples", "Bolo de mandioca com toque de queijo.", "Mandioca" },
                    { 18, "Simples", "Bolo de iogurte com limão.", "Iogurte com Limão" },
                    { 19, "Simples", "Massa de fubá com aroma de erva-doce.", "Fubá com Erva-Doce" },
                    { 20, "Simples", "Bolo de chocolate clássico.", "Chocolate" },
                    { 21, "Simples", "Bolo com sabor de coco.", "Coco" },
                    { 22, "Simples", "Bolo com sabor de laranja.", "Laranja" },
                    { 23, "Simples", "Bolo com sabor de limão.", "Limão" },
                    { 24, "Simples", "Bolo com sabor de maracujá.", "Maracujá" },
                    { 25, "Simples", "Bolo com mistura de sabores.", "Mesclado" },
                    { 26, "Simples", "Bolo fofinho de chocolate.", "Fofinho Chocolate" },
                    { 27, "Simples", "Bolo fofinho de fubá com laranja.", "Fofinho Fubá com Laranja" },
                    { 28, "Caro", "Bolo com sabor de leite condensado.", "Leitinho" },
                    { 29, "Caro", "Bolo com coco e chocolate.", "Prestígio" },
                    { 30, "Caro", "Bolo com sabor suave e fofinho.", "Toalha Felpuda" },
                    { 31, "Caro", "Bolo com sabor de amendoim.", "Amendoim" },
                    { 32, "Caro", "Bolo com damasco, nozes e uvas passas.", "Amizade" },
                    { 33, "Caro", "Bolo estilo brownie.", "Browie" },
                    { 34, "Caro", "Brownie com pedaços de nozes.", "Brownie com Nozes" },
                    { 35, "Simples", "Bolo com sabor de ameixa.", "Ameixa" },
                    { 36, "Simples", "Bolo de trigo simples.", "Trigo" },
                    { 37, "Simples", "Bolo com pedaços de maçã.", "Maçã" },
                    { 38, "Simples", "Bolo com sabor de queijo.", "Queijo" },
                    { 39, "Simples", "Bolo de fubá com crocância e laranja.", "Crocante Fubá com Laranja" },
                    { 40, "Caro", "Bolo com leite de coco e queijo parmesão.", "Luiz Felipe" },
                    { 41, "Caro", "Bolo com coco e queijo.", "Queijadinha" },
                    { 42, "Caro", "Bolo especial para o Natal.", "Natal" },
                    { 43, "Caro", "Bolo com camada de pudim.", "Bolo Pudim" },
                    { 44, "Simples", "Bolo com sabor de café.", "Capuccino" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Achievements",
                keyColumn: "AchievementId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Achievements",
                keyColumn: "AchievementId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Achievements",
                keyColumn: "AchievementId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Achievements",
                keyColumn: "AchievementId",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Achievements",
                keyColumn: "AchievementId",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Achievements",
                keyColumn: "AchievementId",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Achievements",
                keyColumn: "AchievementId",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Achievements",
                keyColumn: "AchievementId",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Achievements",
                keyColumn: "AchievementId",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Achievements",
                keyColumn: "AchievementId",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "Achievements",
                keyColumn: "AchievementId",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "Achievements",
                keyColumn: "AchievementId",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "Achievements",
                keyColumn: "AchievementId",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "Achievements",
                keyColumn: "AchievementId",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "Achievements",
                keyColumn: "AchievementId",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "Achievements",
                keyColumn: "AchievementId",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "Achievements",
                keyColumn: "AchievementId",
                keyValue: 18);

            migrationBuilder.DeleteData(
                table: "Achievements",
                keyColumn: "AchievementId",
                keyValue: 19);

            migrationBuilder.DeleteData(
                table: "Flavors",
                keyColumn: "FlavorId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Flavors",
                keyColumn: "FlavorId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Flavors",
                keyColumn: "FlavorId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Flavors",
                keyColumn: "FlavorId",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Flavors",
                keyColumn: "FlavorId",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Flavors",
                keyColumn: "FlavorId",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Flavors",
                keyColumn: "FlavorId",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Flavors",
                keyColumn: "FlavorId",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Flavors",
                keyColumn: "FlavorId",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Flavors",
                keyColumn: "FlavorId",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Flavors",
                keyColumn: "FlavorId",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "Flavors",
                keyColumn: "FlavorId",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "Flavors",
                keyColumn: "FlavorId",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "Flavors",
                keyColumn: "FlavorId",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "Flavors",
                keyColumn: "FlavorId",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "Flavors",
                keyColumn: "FlavorId",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "Flavors",
                keyColumn: "FlavorId",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "Flavors",
                keyColumn: "FlavorId",
                keyValue: 18);

            migrationBuilder.DeleteData(
                table: "Flavors",
                keyColumn: "FlavorId",
                keyValue: 19);

            migrationBuilder.DeleteData(
                table: "Flavors",
                keyColumn: "FlavorId",
                keyValue: 20);

            migrationBuilder.DeleteData(
                table: "Flavors",
                keyColumn: "FlavorId",
                keyValue: 21);

            migrationBuilder.DeleteData(
                table: "Flavors",
                keyColumn: "FlavorId",
                keyValue: 22);

            migrationBuilder.DeleteData(
                table: "Flavors",
                keyColumn: "FlavorId",
                keyValue: 23);

            migrationBuilder.DeleteData(
                table: "Flavors",
                keyColumn: "FlavorId",
                keyValue: 24);

            migrationBuilder.DeleteData(
                table: "Flavors",
                keyColumn: "FlavorId",
                keyValue: 25);

            migrationBuilder.DeleteData(
                table: "Flavors",
                keyColumn: "FlavorId",
                keyValue: 26);

            migrationBuilder.DeleteData(
                table: "Flavors",
                keyColumn: "FlavorId",
                keyValue: 27);

            migrationBuilder.DeleteData(
                table: "Flavors",
                keyColumn: "FlavorId",
                keyValue: 28);

            migrationBuilder.DeleteData(
                table: "Flavors",
                keyColumn: "FlavorId",
                keyValue: 29);

            migrationBuilder.DeleteData(
                table: "Flavors",
                keyColumn: "FlavorId",
                keyValue: 30);

            migrationBuilder.DeleteData(
                table: "Flavors",
                keyColumn: "FlavorId",
                keyValue: 31);

            migrationBuilder.DeleteData(
                table: "Flavors",
                keyColumn: "FlavorId",
                keyValue: 32);

            migrationBuilder.DeleteData(
                table: "Flavors",
                keyColumn: "FlavorId",
                keyValue: 33);

            migrationBuilder.DeleteData(
                table: "Flavors",
                keyColumn: "FlavorId",
                keyValue: 34);

            migrationBuilder.DeleteData(
                table: "Flavors",
                keyColumn: "FlavorId",
                keyValue: 35);

            migrationBuilder.DeleteData(
                table: "Flavors",
                keyColumn: "FlavorId",
                keyValue: 36);

            migrationBuilder.DeleteData(
                table: "Flavors",
                keyColumn: "FlavorId",
                keyValue: 37);

            migrationBuilder.DeleteData(
                table: "Flavors",
                keyColumn: "FlavorId",
                keyValue: 38);

            migrationBuilder.DeleteData(
                table: "Flavors",
                keyColumn: "FlavorId",
                keyValue: 39);

            migrationBuilder.DeleteData(
                table: "Flavors",
                keyColumn: "FlavorId",
                keyValue: 40);

            migrationBuilder.DeleteData(
                table: "Flavors",
                keyColumn: "FlavorId",
                keyValue: 41);

            migrationBuilder.DeleteData(
                table: "Flavors",
                keyColumn: "FlavorId",
                keyValue: 42);

            migrationBuilder.DeleteData(
                table: "Flavors",
                keyColumn: "FlavorId",
                keyValue: 43);

            migrationBuilder.DeleteData(
                table: "Flavors",
                keyColumn: "FlavorId",
                keyValue: 44);
        }
    }
}
