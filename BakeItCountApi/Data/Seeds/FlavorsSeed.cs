using BakeItCountApi.Cn.Enum;
using BakeItCountApi.Cn.Flavors;
using Microsoft.EntityFrameworkCore;

namespace BakeItCountApi.Data.Seeds
{
    public static class FlavorSeed
    {
        public static void Seed(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Flavor>().HasData(
               new Flavor { FlavorId = 1, Name = "Fubá com Limão Siciliano", Category = FlavorCategory.Simples, Description = "Massa de fubá com toque de limão siciliano." },
                new Flavor { FlavorId = 2, Name = "Maçã com Castanha", Category = FlavorCategory.Simples, Description = "Combinação de maçã e castanhas." },
                new Flavor { FlavorId = 3, Name = "Fubá com Goiabada", Category = FlavorCategory.Simples, Description = "Massa de fubá com pedaços de goiabada." },
                new Flavor { FlavorId = 4, Name = "Fubá", Category = FlavorCategory.Simples, Description = "Clássico bolo de fubá." },
                new Flavor { FlavorId = 5, Name = "Milho", Category = FlavorCategory.Simples, Description = "Bolo de milho com sabor tradicional." },
                new Flavor { FlavorId = 6, Name = "Iogurte", Category = FlavorCategory.Simples, Description = "Bolo leve com sabor de iogurte." },
                new Flavor { FlavorId = 7, Name = "Fubá Cremoso", Category = FlavorCategory.Simples, Description = "Bolo de fubá com textura cremosa." },
                new Flavor { FlavorId = 8, Name = "Cenoura", Category = FlavorCategory.Simples, Description = "Bolo de cenoura fofinho." },
                new Flavor { FlavorId = 9, Name = "Formigueiro", Category = FlavorCategory.Simples, Description = "Massa de chocolate com granulado." },
                new Flavor { FlavorId = 10, Name = "Banana com Canela", Category = FlavorCategory.Simples, Description = "Combinação de banana e canela." },
                new Flavor { FlavorId = 11, Name = "Milho com Requeijão", Category = FlavorCategory.Caro, Description = "Bolo de milho com recheio de requeijão." },
                new Flavor { FlavorId = 12, Name = "Bolo de Cenoura com Creme de Avelã", Category = FlavorCategory.Caro, Description = "Bolo de cenoura com creme de avelã." },
                new Flavor { FlavorId = 13, Name = "Bolo Piscina de Chocolate com Leitinho", Category = FlavorCategory.Caro, Description = "Camadas de bolo de chocolate com leite condensado." },
                new Flavor { FlavorId = 14, Name = "Bolo Piscina de Milho com Curau", Category = FlavorCategory.Caro, Description = "Camadas de bolo de milho com curau." },
                new Flavor { FlavorId = 15, Name = "Bolo Piscina Romeu e Julieta", Category = FlavorCategory.Caro, Description = "Camadas de bolo de goiabada e queijo." },
                new Flavor { FlavorId = 16, Name = "Cenoura com Gotas de Chocolate", Category = FlavorCategory.Simples, Description = "Bolo de cenoura com gotas de chocolate." },
                new Flavor { FlavorId = 17, Name = "Mandioca", Category = FlavorCategory.Simples, Description = "Bolo de mandioca com toque de queijo." },
                new Flavor { FlavorId = 18, Name = "Iogurte com Limão", Category = FlavorCategory.Simples, Description = "Bolo de iogurte com limão." },
                new Flavor { FlavorId = 19, Name = "Fubá com Erva-Doce", Category = FlavorCategory.Simples, Description = "Massa de fubá com aroma de erva-doce." },
                new Flavor { FlavorId = 20, Name = "Chocolate", Category = FlavorCategory.Simples, Description = "Bolo de chocolate clássico." },
                new Flavor { FlavorId = 21, Name = "Coco", Category = FlavorCategory.Simples, Description = "Bolo com sabor de coco." },
                new Flavor { FlavorId = 22, Name = "Laranja", Category = FlavorCategory.Simples, Description = "Bolo com sabor de laranja." },
                new Flavor { FlavorId = 23, Name = "Limão", Category = FlavorCategory.Simples, Description = "Bolo com sabor de limão." },
                new Flavor { FlavorId = 24, Name = "Maracujá", Category = FlavorCategory.Simples, Description = "Bolo com sabor de maracujá." },
                new Flavor { FlavorId = 25, Name = "Mesclado", Category = FlavorCategory.Simples, Description = "Bolo com mistura de sabores." },
                new Flavor { FlavorId = 26, Name = "Fofinho Chocolate", Category = FlavorCategory.Simples, Description = "Bolo fofinho de chocolate." },
                new Flavor { FlavorId = 27, Name = "Fofinho Fubá com Laranja", Category = FlavorCategory.Simples, Description = "Bolo fofinho de fubá com laranja." },
                new Flavor { FlavorId = 28, Name = "Leitinho", Category = FlavorCategory.Caro, Description = "Bolo com sabor de leite condensado." },
                new Flavor { FlavorId = 29, Name = "Prestígio", Category = FlavorCategory.Caro, Description = "Bolo com coco e chocolate." },
                new Flavor { FlavorId = 30, Name = "Toalha Felpuda", Category = FlavorCategory.Caro, Description = "Bolo com sabor suave e fofinho." },
                new Flavor { FlavorId = 31, Name = "Amendoim", Category = FlavorCategory.Caro, Description = "Bolo com sabor de amendoim." },
                new Flavor { FlavorId = 32, Name = "Amizade", Category = FlavorCategory.Caro, Description = "Bolo com damasco, nozes e uvas passas." },
                new Flavor { FlavorId = 33, Name = "Browie", Category = FlavorCategory.Caro, Description = "Bolo estilo brownie." },
                new Flavor { FlavorId = 34, Name = "Brownie com Nozes", Category = FlavorCategory.Caro, Description = "Brownie com pedaços de nozes." },
                new Flavor { FlavorId = 35, Name = "Ameixa", Category = FlavorCategory.Simples, Description = "Bolo com sabor de ameixa." },
                new Flavor { FlavorId = 36, Name = "Trigo", Category = FlavorCategory.Simples, Description = "Bolo de trigo simples." },
                new Flavor { FlavorId = 37, Name = "Maçã", Category = FlavorCategory.Simples, Description = "Bolo com pedaços de maçã." },
                new Flavor { FlavorId = 38, Name = "Queijo", Category = FlavorCategory.Simples, Description = "Bolo com sabor de queijo." },
                new Flavor { FlavorId = 39, Name = "Crocante Fubá com Laranja", Category = FlavorCategory.Simples, Description = "Bolo de fubá com crocância e laranja." },
                new Flavor { FlavorId = 40, Name = "Luiz Felipe", Category = FlavorCategory.Caro, Description = "Bolo com leite de coco e queijo parmesão." },
                new Flavor { FlavorId = 41, Name = "Queijadinha", Category = FlavorCategory.Caro, Description = "Bolo com coco e queijo." },
                new Flavor { FlavorId = 42, Name = "Natal", Category = FlavorCategory.Caro, Description = "Bolo especial para o Natal." },
                new Flavor { FlavorId = 43, Name = "Bolo Pudim", Category = FlavorCategory.Caro, Description = "Bolo com camada de pudim." },
                new Flavor { FlavorId = 44, Name = "Capuccino", Category = FlavorCategory.Simples, Description = "Bolo com sabor de café." }
            );
        }
    }
}